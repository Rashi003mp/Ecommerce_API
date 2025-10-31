using Ecommerce_API.Entities;

namespace Ecommerce_API.Reopsitory.Interfaces
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        Task<IEnumerable<Address>> GetUserAddressesAsync(int userId);
        Task<Address?> GetAddressByIdAsync(int id, int userId);
    }

}
