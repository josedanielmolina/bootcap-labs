using ApiAdmin.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace ApiAdmin.Repository.Base
{
    public interface IUnitOfWork
    {
        IRepository<Empleado> Empleado { get; set; }
        IRepository<BacklogsEvent> BacklogsEvent { get; set; }

        IDbContextTransaction BeginTransaction();
        void Dispose();
        Task SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IRepository<Empleado> Empleado { get; set; }
        public IRepository<BacklogsEvent> BacklogsEvent { get; set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Empleado = new Repository<Empleado>(context);
            BacklogsEvent = new Repository<BacklogsEvent>(context);
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
