using ApiAuth.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace ApiAuth.Repository.Base
{
    public interface IUnitOfWork
    {
        IRepository<Usuario> Usuario { get; set; }

        IDbContextTransaction BeginTransaction();
        void Dispose();
        Task SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IRepository<Usuario> Usuario { get; set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Usuario = new Repository<Usuario>(context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
