namespace Data.Entities
{
    public class Certificate : BaseEntity
    {
        public string Name { get; set; }
        public CertificateCategory CertificateCategory { get; set; }
        public int ValidForMonths { get; set; }

        public bool Deleted { get; set; }
    }
}
