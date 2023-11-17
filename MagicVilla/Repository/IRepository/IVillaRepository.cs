using MagicVilla.Models;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MagicVilla.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa villa);
    }
}
