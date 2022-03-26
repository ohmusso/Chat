using MagicOnion;

namespace Chat.Shared.Services
{
    // Defines .NET interface as a Server/Client IDL.
    // The interface is shared between server and client.
    public interface IMyFirstService : IService<IMyFirstService>
    {
        // The return type must be `UnaryResult<T>`.
        UnaryResult<int> SumAsync(int x, int y);
        UnaryResult<int> TestStorageAdd(string table, string data);
        UnaryResult<int> TestStorageDelete(string table, string data);
        UnaryResult<int> TestStorageQuery(string table, string data);
    }
}
