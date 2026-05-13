namespace WorkProject.Contracts.Services
{
    public interface ICurrentUserAccessor
    {
        int? CurrentUserId { get; }
        string CurrentUserName { get; }
    }
}
