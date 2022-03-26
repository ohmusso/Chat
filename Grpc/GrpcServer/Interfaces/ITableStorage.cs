using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Data.Tables;
namespace GrpcServer.Interfaces
{
    public interface ITableStorage<T> where T : ITableEntity 
    {
        Task Add(string tableName, T entity);
        Task Delete(string tableName, string PartitionKey, string RowKey);
        Task<IEnumerable<T>> Query(string tableName, System.Linq.Expressions.Expression<System.Func<T, bool>> query);
    }
}
