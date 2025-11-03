namespace Ecommerce_API.Reopsitory.Interfaces
{
    public interface IUserRepository
    {
        Task BlockUnblockUserAsync(int id);
        Task SoftDeleteUserAsync(int id);
    }
}
