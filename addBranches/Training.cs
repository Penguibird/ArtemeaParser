using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Training : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsExternal { get; set; }
        public DateTime DueUTC { get; set; }
        public DateTime TakesPlaceUTC { get; set; }
        public bool IsActive { get; set; }
        public double Price { get; set; }
        public TrainingType Type { get; set; }
        public string Supplier { get; set; }
        public long Duration { get; set; }
        public double Rating { get; set; }
        public IList<string> Badges { get; set; }
        public IList<Guid> RecomendedTrainings { get; set; }
        public IList<string> Languages { get; set; }
        public int Version { get; set; }
        public IList<string> Topics { get; set; }
        public string Link { get; set; }
        public Certificate Certificate { get; set; }
        public Skill AwardsSkill { get; set; }
        public int AwardsSkillLevel { get; set; }

        public bool Deleted { get; set; }

        //Reviews and curriculum are missing, Badges maybe should be an object
    }

    public enum TrainingType
    {
        ONLINE = 0,
        VIDEO = 1,
        PERSONAL = 2
    }
}
