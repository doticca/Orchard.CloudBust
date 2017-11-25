using Orchard.Events;

namespace CloudBust.Application.Services {
    public interface IInvitationsProcessor : IEventHandler {
        void Invite(int id, string url);
    }
}