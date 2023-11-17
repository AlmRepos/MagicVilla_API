using MagicVilla.Models;

namespace MagicVilla.Repository.IRepository
{
    public interface IVillaNumberRepository : IRepository<VillaNumber>
    {
        Task<VillaNumber> UpdateVillaNumberAsync(VillaNumber entity);
    }
}
