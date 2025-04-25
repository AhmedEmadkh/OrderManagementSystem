using OrderManagement.Core;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Repository.Contract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderManagementContext _dbContext;
        private Hashtable _repositories;

        public UnitOfWork(OrderManagementContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(key))
            {
                var respository = new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(key, respository);
            }
            return _repositories[key] as IGenericRepository<TEntity>;
        }
        public Task<int> CompleteAsync()
            => _dbContext.SaveChangesAsync();

        public ValueTask DisposeAsync()
            => _dbContext.DisposeAsync();


    }
}
