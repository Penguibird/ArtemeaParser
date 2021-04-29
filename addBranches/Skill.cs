using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Skill : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //[BsonIgnore]
        public int MaxLevel
        {
            get
            {
                if (Levels == null) Levels = new List<string>();
                return Levels.Count + 1;
            }
            set { Console.Error.Write("Tried to set Skill.MaxLevel directly. This won't work. To change MaxLevel change the Levels array"); }
        }
        public List<String> Levels { get; set; }
        public SkillCategory Category { get; set; }

        public bool Deleted { get; set; }
    }

    public enum SkillCategory
    {
        Hard = 0,
        Soft = 1,
        Process = 2,
        Customer = 3
    }
}
