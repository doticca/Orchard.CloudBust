using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Web;
using Autofac;
using Orchard.Logging;
using Orchard.Mvc;
using Orchard.Mvc.Extensions;

namespace Orchard.Environment {
    public class WorkContextAccessor : ILogicalWorkContextAccessor {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ILifetimeScope _lifetimeScope;

        // a different symbolic key is used for each tenant.
        // this guarantees the correct accessor is being resolved.
        private readonly object _workContextKey = new object();
        private readonly string _workContextSlot;

        public WorkContextAccessor(
            IHttpContextAccessor httpContextAccessor,
            ILifetimeScope lifetimeScope, IHostEnvironment hostEnvironment) {
            _httpContextAccessor = httpContextAccessor;
            _lifetimeScope = lifetimeScope;
            _hostEnvironment = hostEnvironment;
            _workContextSlot = "WorkContext." + Guid.NewGuid().ToString("n");
        }

        public WorkContext GetLogicalContext() {
            var context = CallContext.LogicalGetData(_workContextSlot) as ObjectHandle;
            return context?.Unwrap() as WorkContext;
        }

        public IWorkContextScope CreateWorkContextScope(HttpContextBase httpContext) {
            try {
                var workLifetime = _lifetimeScope.BeginLifetimeScope("work");

                var events = workLifetime.Resolve<IEnumerable<IWorkContextEvents>>();
                events.Invoke(e => e.Started(), NullLogger.Instance);

                if (!httpContext.IsBackgroundContext())
                    return new HttpContextScopeImplementation(events, workLifetime, httpContext, _workContextKey);

                return new CallContextScopeImplementation(events, workLifetime, _workContextSlot);
            }
            catch (Exception) {
                _hostEnvironment.RestartAppDomain();
                return null;
            }
        }

        public IWorkContextScope CreateWorkContextScope() {
            var httpContext = _httpContextAccessor.Current();
            return CreateWorkContextScope(httpContext);
        }

        public WorkContext GetContext(HttpContextBase httpContext) {
            if (!httpContext.IsBackgroundContext())
                return httpContext.Items[_workContextKey] as WorkContext;

            return GetLogicalContext();
        }

        public WorkContext GetContext() {
            var httpContext = _httpContextAccessor.Current();
            return GetContext(httpContext);
        }

        private class HttpContextScopeImplementation : IWorkContextScope {
            private readonly Action _disposer;

            public HttpContextScopeImplementation(IEnumerable<IWorkContextEvents> events, ILifetimeScope lifetimeScope, HttpContextBase httpContext, object workContextKey) {
                WorkContext = lifetimeScope.Resolve<WorkContext>();
                httpContext.Items[workContextKey] = WorkContext;

                _disposer = () => {
                    events.Invoke(e => e.Finished(), NullLogger.Instance);
                    httpContext.Items.Remove(workContextKey);
                    lifetimeScope.Dispose();
                };
            }

            void IDisposable.Dispose() {
                _disposer();
            }

            public TService Resolve<TService>() {
                return WorkContext.Resolve<TService>();
            }

            public bool TryResolve<TService>(out TService service) {
                return WorkContext.TryResolve(out service);
            }

            public WorkContext WorkContext { get; }
        }

        private class CallContextScopeImplementation : IWorkContextScope {
            private readonly Action _disposer;

            public CallContextScopeImplementation(IEnumerable<IWorkContextEvents> events, ILifetimeScope lifetimeScope, string workContextSlot) {
                CallContext.LogicalSetData(workContextSlot, null);

                WorkContext = lifetimeScope.Resolve<WorkContext>();
                var httpContext = lifetimeScope.Resolve<HttpContextBase>();
                WorkContext.HttpContext = httpContext;

                CallContext.LogicalSetData(workContextSlot, new ObjectHandle(WorkContext));

                _disposer = () => {
                    events.Invoke(e => e.Finished(), NullLogger.Instance);
                    CallContext.FreeNamedDataSlot(workContextSlot);
                    lifetimeScope.Dispose();
                };
            }

            void IDisposable.Dispose() {
                _disposer();
            }

            public TService Resolve<TService>() {
                return WorkContext.Resolve<TService>();
            }

            public bool TryResolve<TService>(out TService service) {
                return WorkContext.TryResolve(out service);
            }

            public WorkContext WorkContext { get; }
        }
    }
}