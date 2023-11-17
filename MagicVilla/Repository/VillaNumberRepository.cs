using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Repository.IRepository;

namespace MagicVilla.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly Context db;

        public VillaNumberRepository(Context db) :base(db)
        {
            this.db = db;
        }
        public async Task<VillaNumber> UpdateVillaNumberAsync(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            db.VillaNumbers.Update(entity);
            await db.SaveChangesAsync();
            return entity;
        }
    }
}
