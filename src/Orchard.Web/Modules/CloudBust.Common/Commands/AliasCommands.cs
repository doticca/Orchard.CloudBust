using Orchard;
using Orchard.Alias;
using Orchard.Commands;
using System;
using Orchard.Environment;
using System.Linq;

namespace CloudBust.Common.Commands
{
    public class AliasCommands : DefaultOrchardCommandHandler
    {
        private readonly Work<WorkContext> _workContext;
        private readonly IAliasService _aliasService;

        public AliasCommands(Work<WorkContext> workContext, IAliasService aliasService,
            IOrchardServices orchardServices)
        {
            _workContext = workContext;
            _aliasService = aliasService;
            Services = orchardServices;
        }
        public IOrchardServices Services { get; private set; }

        [OrchardSwitch]
        public string AliasPath { get; set; }
        [OrchardSwitch]
        public string RoutePath { get; set; }

        [CommandName("alias add")]
        [CommandHelp("alias add /AliasPath:<alias-path> /RoutePath:<route-path>\r\n\t" + "Add a new alias")]
        [OrchardSwitches("AliasPath,RoutePath")]
        public void Add()
        {
            AliasPath = AliasPath.TrimStart('/', '\\');
            if (String.IsNullOrWhiteSpace(AliasPath))
            {
                AliasPath = "/";
            }
            if (String.IsNullOrWhiteSpace(RoutePath))
            {
                Context.Output.WriteLine(T("Route can't be empty"));
                return;
            }       
            if (CheckAndWarnIfAliasExists(AliasPath))
            {
                Context.Output.WriteLine(T("Alias already exist"));
                return;
            }
            try
            {
                _aliasService.Set(AliasPath, RoutePath, "Custom");
            }
            catch (Exception ex)
            {
                Services.TransactionManager.Cancel();
                Context.Output.WriteLine(T("An error occured while creating the alias {0}: {1}. Please check the values are correct.", AliasPath, ex.Message));
                return;
            }
            Context.Output.WriteLine(T("Alias {0} created.", AliasPath));
        }
        [CommandName("alias delete")]
        [CommandHelp("alias delete /AliasPath:<alias-path>\r\n\t" + "Delete an existing alias")]
        [OrchardSwitches("AliasPath")]
        public void Delete()
        {
            if (AliasPath == "/")
            {
                AliasPath = String.Empty;
            }
            try
            {
                _aliasService.Delete(AliasPath);
                Context.Output.WriteLine(T("Alias {0} deleted.", AliasPath));
            }
            catch (Exception ex)
            {
                Context.Output.WriteLine(T("An error occured while deleting the alias {0}: {1}. Please check the values are correct.", AliasPath, ex.Message));
                return;
            }            
        }
        private string GetExistingPathForAlias(string aliasPath)
        {
            var routeValues = _aliasService.Get(aliasPath.TrimStart('/', '\\'));
            if (routeValues == null) return null;

            return _aliasService.LookupVirtualPaths(routeValues, _workContext.Value.HttpContext)
                .Select(vpd => vpd.VirtualPath)
                .FirstOrDefault();
        }
        private bool CheckAndWarnIfAliasExists(string aliasPath)
        {
            var routePath = GetExistingPathForAlias(aliasPath);
            if (routePath == null) return false;

            return true;
        }
    }
}