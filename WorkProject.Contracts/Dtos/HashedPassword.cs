namespace WorkProject.Contracts.Dtos
{
    public class HashedPassword
    {
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
        public int Iterations { get; set; }
    }
}
