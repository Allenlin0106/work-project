using WorkProject.Contracts.Dtos;

namespace WorkProject.Contracts.Services
{
    public interface IPasswordHasher
    {
        HashedPassword Hash(string password);
        bool Verify(string password, byte[] hash, byte[] salt, int iterations);
    }
}
