using Api.Empresa.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace ApiAdmin.Repository.Base
{
    public interface IUnitOfWork
    {
        IRepository<Empleado> EmpleadoRepository { get; set; }
        IRepository<Backlogsevent> BacklogsEvent { get; set; }
        IRepository<Areasempresa> AreasempresaRepository { get; set; }

        IDbContextTransaction BeginTransaction();
        void Dispose();
        Task SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IRepository<Empleado> EmpleadoRepository { get; set; }
        public IRepository<Backlogsevent> BacklogsEvent { get; set; }
        public IRepository<Areasempresa> AreasempresaRepository { get; set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            EmpleadoRepository = new Repository<Empleado>(context);
            BacklogsEvent = new Repository<Backlogsevent>(context);
            AreasempresaRepository = new Repository<Areasempresa>(context);
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
