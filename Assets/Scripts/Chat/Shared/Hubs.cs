using MagicOnion;
using System.Threading.Tasks;

namespace Chat.Shared.Hubs
{
    // Server -> Client definition
    public interface IChatHubReceiver
    {
        void OnJoin(string name);
        void OnLeave(string name);
        void OnSendMessage(string name, string message);
    }

    // Client -> Server definition
    // implements `IStreamingHub<TSelf, TReceiver>`  and share this type between server and client.
    public interface IChatHub : IStreamingHub<IChatHub, IChatHubReceiver>
    {
        Task JoinAsync(string userName);
        Task LeaveAsync();
        Task SendMessageAsync(string message);
    }
}
