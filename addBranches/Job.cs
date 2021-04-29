namespace Data.Entities
{
    public class Job : BaseEntity
    {
        public string Name { get; set; }
        public int LevelFrom { get; set; }
        public int LevelTo { get; set; }
    }
}
