namespace ProjectPlanning.DAL.Interfaces
{
    public interface ICurrentUserAccessor
    {
        string WindowsAccount { get; }
        int? UserId { get; }
        string IpAddress { get; }
        string UserAgent { get; }
        string ControllerAction { get; }
    }
}
