using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Domain.Entities;
using WebApi.Domain.Repositories;
using WebApi.EfInfraData.Contexts;
using WebApi.EfInfraData.Extensions;
using WebApi.EfInfraData.Models;
using WebApi.WarmUp.Abstractions;

namespace WebApi.EfInfraData.Repositories
{
    public class SampleRepository : IWarmUpCommand, ISampleRepository
    {
        private readonly WebApiDbContext _context;

        public SampleRepository(WebApiDbContext context) =>
          _context = context;

        public void Add(SampleEntity model)
        {
            var modelTable = model.AsTable();
            _context.SampleTables.Add(modelTable);
        }

        public SampleEntity GetById(int id) => _context
            .SampleTables
                .FirstOrDefault(x => x.Id == id)
                .AsEntity();

        public void Remove(SampleEntity model)
        {
            var register = model.AsTable();
            register.Active = false;
            UpdateRegister(register);
        }

        public void Update(SampleEntity model)
        {
            var register = model.AsTable();
            UpdateRegister(register);
        }

        async Task IWarmUpCommand.Execute()
        {
            await _context.SampleTables.FindAsync(-1);
        }

        private void UpdateRegister(SampleTable register)
        {
            _context.Entry(register).State = EntityState.Modified;
            _context.SampleTables.Update(register);
        }
    }
}
