using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OnlineExaminationSystem_Back_End_DAL.Core.IRepositories;
using OnlineExaminationSystem_Back_End_DAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem_Back_End_DAL.Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly DatabaseContext _dbcontext;
        protected readonly DbSet<T> _dbset;
        protected readonly ILogger _logger;

        public GenericRepository(DatabaseContext dbcontext, DbSet<T> dbset, ILogger logger)
        {
            _dbcontext = dbcontext;
            _dbset = dbset;
            _logger = logger;
        }

        public Task<bool> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> All()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
