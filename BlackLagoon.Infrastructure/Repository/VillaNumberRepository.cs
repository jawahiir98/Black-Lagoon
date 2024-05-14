using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Domain.Entities;
using BlackLagoon.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackLagoon.Infrastructure.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext db;
        public VillaNumberRepository(ApplicationDbContext _db) : base(_db)
        {
            db = _db;
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(VillaNumber entity)
        {
            db.Update(entity);
        }
    }
}
