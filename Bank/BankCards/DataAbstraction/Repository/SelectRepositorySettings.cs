namespace DataAbstraction.Repository
{
    public class SelectRepositorySettings
    {
        public bool UseEntityFramework { get; set; } = true;
        public string DefaultConnection { get; set; }
    }
}
