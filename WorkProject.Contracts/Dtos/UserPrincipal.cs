namespace WorkProject.Contracts.Dtos
{
    public class UserPrincipal
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
