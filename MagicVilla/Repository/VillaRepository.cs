using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MagicVilla.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository 
    {
        private readonly Context db;

        public VillaRepository(Context db) : base(db)
        {
            this.db = db;
        }

        public async Task<Villa> UpdateAsync(Villa entity)
        {
            entity.UpdatedDate = DateTime.Now;
            db.Villas.Update(entity);
            await db.SaveChangesAsync();

            return entity;
        }
    }
}
