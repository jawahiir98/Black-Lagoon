using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackLagoon.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext db;
        public IVillaRepository Villa { get; private set; }

        public UnitOfWork(ApplicationDbContext _db)
        {
            db = _db;
            Villa = new VillaRepository(db);
        }

    }
}
