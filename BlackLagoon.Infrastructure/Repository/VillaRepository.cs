using BlackLagoon.Application.Common.Interfaces;
using BlackLagoon.Domain.Entities;
using BlackLagoon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlackLagoon.Infrastructure.Repository
{
    public class VillaRepository : IVillaRepository
    {
        private readonly ApplicationDbContext db;
        public VillaRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public void Add(Villa villa)
        {
            db.Add(villa);
        }

        public Villa Get(Expression<Func<Villa, bool>>? filter, string? includeProperties = null)
        {
            IQueryable<Villa> query = db.Set<Villa>();
            if (filter != null) query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(new char[','], StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Villa> query = db.Set<Villa>();
            if(filter != null) query = query.Where(filter);
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var property in includeProperties.Split(new char[','], StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.ToList();
        }

        public void Remove(Villa villa)
        {
            db.Remove(villa);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Update(Villa villa)
        {
            db.Update(villa);
        }
    }
}
