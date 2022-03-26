using System;
using MagicOnion;
using MagicOnion.Server;
using GrpcServer.Interfaces;
using GrpcServer.TableStorage;
using Chat.Shared.ChatData;
using Chat.Shared.Services;

namespace GrpcServer.Services
{
    public class ChatService
    {
        // Implements RPC service in the server project.
        // The implementation class must inehrit `ServiceBase<IMyFirstService>` and `IMyFirstService`
        public class MyFirstService : ServiceBase<IMyFirstService>, IMyFirstService
        {
            private readonly ITableStorage<TableEntityChatData> _table;
            public MyFirstService(
                ITableStorage<TableEntityChatData> table
            )
            {
                _table = table;
            }

            // `UnaryResult<T>` allows the method to be treated as `async` method.
            public async UnaryResult<int> SumAsync(int x, int y)
            {
                Console.WriteLine($"Received:{x}, {y}");
                return x + y;
            }

            public async UnaryResult<int> TestStorageAdd(string table, string data)
            {
                var entity = new TableEntityChatData()
                {
                    PartitionKey = "P1",
                    RowKey = data,
                    name = "name_" + data,
                    text = "text_" + data
                };

                Console.WriteLine($"TestStorageAdd:{table}, {data}");

                await _table.Add(table, entity);

                return 1;
            }

            public async UnaryResult<int> TestStorageDelete(string table, string data)
            {
                Console.WriteLine($"TestStorageDelete:{table}, {data}");

                await _table.Delete(table, "P1", data);

                return 1;
            }

            public async UnaryResult<int> TestStorageQuery(string table, string data)
            {
                System.Linq.Expressions.Expression<Func<TableEntityChatData, bool>> q = x => x.PartitionKey == data;

                Console.WriteLine($"TestStorageQuery:{table}, {data}");

                await _table.Query(table, q);

                return 1;
            }
        }
    }
}
