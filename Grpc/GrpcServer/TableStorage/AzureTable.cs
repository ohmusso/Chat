using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Azure;
using Azure.Data.Tables;
using GrpcServer.Interfaces;
using System;

namespace GrpcServer.TableStorage
{
    public class AzureTable<T> : ITableStorage<T> where T : class, ITableEntity, new()
    {
        private readonly TableServiceClient _tableStorageClient;
        private readonly IConfigurationSection _storageConfig;

        public AzureTable(
            TableServiceClient tableStorageClient,
            IConfigurationSection storageConfig
        )
        {
            _tableStorageClient = tableStorageClient;
            _storageConfig = storageConfig;

            // create table
            _tableStorageClient.CreateTableIfNotExists(_storageConfig["TableChatData"]);
        }

        public async Task Add(string tableName, T entity)
        {
            var tableClient = _tableStorageClient.GetTableClient(tableName);
            await tableClient.AddEntityAsync(entity);
        }

        public async Task Delete(string tableName, string PartitionKey, string RowKey)
        {
            var tableClient = _tableStorageClient.GetTableClient(tableName);
            await tableClient.DeleteEntityAsync(PartitionKey, RowKey);
        }

        public async Task<IEnumerable<T>> Query(string tableName,System.Linq.Expressions.Expression<System.Func<T, bool>> query)
        {
            var result = new List<T>();
            var tableClient = _tableStorageClient.GetTableClient(tableName);
            var q = tableClient.QueryAsync<T>(query);

            await foreach (var entity in q)
            {
                result.Add(entity);
            }
 
            return result;
        }
    }
}
