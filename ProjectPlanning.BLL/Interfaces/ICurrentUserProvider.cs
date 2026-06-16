using ProjectPlanning.Entities.Models;

namespace ProjectPlanning.BLL.Interfaces
{
    public interface ICurrentUserProvider
    {
        string WindowsAccount { get; }
        User GetOrProvision();
        bool IsInRole(string role);
    }
}
