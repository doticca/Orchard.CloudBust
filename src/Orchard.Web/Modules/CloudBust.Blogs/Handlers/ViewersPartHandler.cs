using Contrib.Voting.Models;
using Contrib.Voting.Services;
using CloudBust.Blogs.Models;
using CloudBust.Blogs.Settings;
using Orchard;
using Orchard.ContentManagement.Handlers;
using System;
using System.Linq;
using Orchard.Environment.Extensions;

namespace CloudBust.Blogs.Handlers
{
    [OrchardFeature("CloudBust.Blogs.Stats")]
    public class ViewersPartHandler : ContentHandler
    {
        private readonly IVotingService _votingService;
        private readonly IOrchardServices _orchardServices;

        public ViewersPartHandler(IOrchardServices orchardServices, IVotingService votingService)
        {
            _votingService = votingService;
            _orchardServices = orchardServices;

            OnInitializing<ViewersPart>((context, part) =>
            {
                part.ShowVoter = part.Settings.GetModel<ViewersTypePartSettings>().ShowVoter;
                part.AllowAnonymousRatings = part.Settings.GetModel<ViewersTypePartSettings>().AllowAnonymousRatings;
            });
            OnLoading<ViewersPart>((context, part) =>
                {
                    part.ResultValue = (_votingService.GetResult(part.ContentItem.Id, "sum", Constants.Dimension_View) ?? new ResultRecord()).Value;
                    part.Count = (_votingService.GetResult(part.ContentItem.Id, "count", Constants.Dimension_View) ?? new ResultRecord()).Count;
                    if (_orchardServices.WorkContext != null)
                    {

                        var currentUser = _orchardServices.WorkContext.CurrentUser;
                        if (currentUser != null)
                        {
                            var userRating = _votingService.Get(vote => vote.Username == currentUser.UserName && vote.ContentItemRecord == part.ContentItem.Record && vote.Dimension == Constants.Dimension_View).FirstOrDefault();
                            part.UserRating = userRating != null ? userRating.Value : 0;
                        }
                    }
                });
            OnGetDisplayShape<ViewersPart>((context, part) =>
                {
                var settings = part.Settings.GetModel<ViewersTypePartSettings>();
                if (context.DisplayType.Equals("Detail", StringComparison.InvariantCultureIgnoreCase))
                    RecordView(part, settings);
                });
        }

        private void RecordView(ViewersPart part, ViewersTypePartSettings settings)
        {
            var currentUser = _orchardServices.WorkContext.CurrentUser;

            if (currentUser != null)
            {
                Vote(currentUser.UserName, part, settings);
            }
            else if (settings.AllowAnonymousRatings)
            {
                var anonHostname = _orchardServices.WorkContext.HttpContext.Request.UserHostAddress;
                if (!string.IsNullOrWhiteSpace(_orchardServices.WorkContext.HttpContext.Request.Headers["X-Forwarded-For"]))
                    anonHostname += "-" + _orchardServices.WorkContext.HttpContext.Request.Headers["X-Forwarded-For"];

                Vote("Anonymous" + anonHostname, part, settings);
            }
        }

        private void Vote(string userName, ViewersPart part, ViewersTypePartSettings settings)
        {
            var currentVote = _votingService.Get(vote =>
                vote.ContentItemRecord == part.ContentItem.Record &&
                vote.Username == userName &&
                vote.Dimension == Constants.Dimension_View).FirstOrDefault();

            if (currentVote != null)
            {
                _votingService.ChangeVote(currentVote, (currentVote.Value + 1));
            }
            else if (currentVote == null)
            {

                _votingService.Vote(part.ContentItem, userName, _orchardServices.WorkContext.HttpContext.Request.UserHostAddress, 1, Constants.Dimension_View);
            }
            part.UserRating = part.UserRating + 1;
            part.ResultValue = (_votingService.GetResult(part.ContentItem.Id, "sum", Constants.Dimension_View) ?? new ResultRecord()).Value;
            part.Count = (_votingService.GetResult(part.ContentItem.Id, "count", Constants.Dimension_View) ?? new ResultRecord()).Count;
        }
    }
}