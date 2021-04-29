using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Position : BaseEntity
    {
        private Job job;

        public int Tier { get; set; }
        public int VerticalPosition { get; set; }
        public int HorizontalPosition { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Guid> Parents { get; set; } //We just don't use this one, all info is in children
        public IList<Guid> Children { get; set; }
        public List<Guid> SuggestedTrainingIDs { get; set; }
        //[BsonIgnore] //is this right
        public List<Training> SuggestedTrainings { get; set; } //deprecated
        public IList<PositionSkill> RequiredSkills { get; set; }
        public String Color { get; set; }
        public Job Job { get => job; set => job = value; }
        public List<PositionDescription> Descriptions { get; set; }
        [BsonIgnore]
        public bool Current { get; set; }
        [BsonIgnore]
        public bool? Planned { get; set; }
        public SfiaLevel? SfiaLevel { get; set; }
        public Guid branchID { get; set; }
        [BsonIgnore]
        public Branch branch { get; set; }
    }

    public class PositionSkill : BaseEntity
    {
        public Guid SkillId { get; set; }
        [BsonIgnore]
        public Skill Skill { get; set; }
        public bool Achieved { get; set; }
        public int Level { get; set; }
    }

    public enum SfiaLevel
    {
        None = 0,
        Follow = 1,
        Assist = 2,
        Apply = 3,
        Enable = 4,
        EnsureAdvice = 5,
        InitiateInfluence = 6,
        SetInspireMobilise = 7
    }
}
