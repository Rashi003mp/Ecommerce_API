using Ecommerce_API.Data;
using Ecommerce_API.Entities;
using Ecommerce_API.Reopsitory.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_API.Reopsitory.Implementation
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        private readonly AppDbContext _context;

        public AddressRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        // Get all addresses for a user
        public async Task<IEnumerable<Address>> GetUserAddressesAsync(int userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        // Get a single address by id (must belong to the user)
        public async Task<Address?> GetAddressByIdAsync(int id, int userId)
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        }
    }
}
