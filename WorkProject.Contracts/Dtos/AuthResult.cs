namespace WorkProject.Contracts.Dtos
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public UserPrincipal User { get; set; }
    }
}
