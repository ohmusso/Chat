using System;
using Azure;
using Azure.Data.Tables;

namespace GrpcServer.TableStorage
{
    public class TableEntityChatData : ITableEntity
    {
        // mandatory
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
        // user data
        public string name { get; set; }
        public string text { get; set; }

        public TableEntityChatData()
        {
            // nop
        }
    }
}
