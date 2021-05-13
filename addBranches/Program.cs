using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Data.Context;
using Data.Entities;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using MongoDB.Driver;

namespace addBranches
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // CreateHostBuilder(args).Build().Run();
            string dbString = "mongodb+srv://admin:artemea@artemea-fabss.mongodb.net/test?retryWrites=true&w=majority";
            string dbName = "artemea_prod";
            ArtemeaDb _dbContext = new ArtemeaDb(dbString, dbName);
            Program p = new Program();
            var positions = new List<Position>();
            var branches = new List<Branch>();
            var skills = new List<Skill>();
            var trainings = new List<Training>();

            using (var reader = new StreamReader(@"D:\scripts\ArtemeaParser\addBranches\map.csv"))
            {
                while (!reader.EndOfStream)
                {
                    String line = reader.ReadLine();
                    String[] values = line.Split(',');
                    String unit = values[1];
                    String parentBranchName = unit;
                    String team = values[2];
                    String branchName = values[3]; // job role in the xls
                    String product = values[4];
                    String skillName = values[5]; //
                    if (new Regex("(Skills)|(Trainings)", RegexOptions.IgnoreCase).IsMatch(skillName))
                    {
                        skillName = values[4];
                    }
                    List<String> skillLevels = values.ToList().GetRange(6, values.Length - 6);
                    skillLevels = skillLevels.ToList().Where(_ => p.isValidField(_)).ToList();
                    Skill skill = null;


                    skill = skills.Find(_ => _.Name == skillName);

                    if (skill == null)
                    {
                        skill = new Skill
                        {
                            ID = Guid.NewGuid(),
                            Name = skillName,
                            Description = "",
                            Levels = skillLevels,
                            Category = SkillCategory.Hard,
                            Deleted = false,
                        };
                        skills.Add(skill);
                    }
                    Branch parentBranch = null;


                    parentBranch = branches.Find(_ => _.name == parentBranchName);

                    if (parentBranch == null)
                    {
                        parentBranch = new Branch
                        {
                            ID = Guid.NewGuid(),
                            name = parentBranchName,
                            parentID = Guid.Empty, //add superparent
                            positionIDs = new List<Guid>(),
                            ChildrenIDs = new List<Guid>(),
                        };
                        branches.Add(parentBranch);
                    }
                    Branch branch = null;


                    branch = branches.Find(_ => _.name == branchName);

                    if (branch == null)
                    {
                        branch = new Branch
                        {
                            ID = Guid.NewGuid(),
                            name = branchName,
                            parentID = parentBranch.ID,
                            positionIDs = new List<Guid>(),
                            ChildrenIDs = new List<Guid>(),
                        };
                        parentBranch.ChildrenIDs.Add(branch.ID);
                        branches.Add(branch);
                    }
                    for (int i = 1; i < values.Length - 5; i++)
                    {
                        TierNameMapping t = new TierNameMapping(i);
                        if (!p.isValidField(values[t.csvIndex])) continue;
                        String positionName = branchName + "" + t.nameSuffix;
                        Position position = positions.Find(_ => _.Name == positionName);

                        if (position == null)
                        {
                            position = new Position
                            {
                                ID = Guid.NewGuid(),
                                Name = positionName,
                                Tier = t.Tier,
                                Description = "",
                                RequiredSkills = new List<PositionSkill>(),
                                branchID = branch.ID,
                            };
                            branch.positionIDs.Add(position.ID);
                            positions.Add(position);
                        }
                        int skillLevel = skill.Levels.FindIndex(_ => _ == values[t.csvIndex]);
                        position.RequiredSkills.Add(new PositionSkill
                        {
                            SkillId = skill.ID,
                            Level = skillLevel,
                        });
                    }
                    Console.WriteLine("Parsed a new line, Skill:", skillName);
                }
            }
            // var dbSnapshotPositions = await _dbContext.GetList<Position>("positions");
            // var dbSnapshotSkills = 
            foreach (Position position in positions)
            {
                await _dbContext.Add<Position>("positions", position);
            }
            foreach (Branch branch in branches)
            {
                await _dbContext.Add<Branch>("branches", branch);
            }
            foreach (Skill skill in skills)
            {
                await _dbContext.Add<Skill>("skills", skill);
            }
            Console.ReadLine();
        }

        public bool isValidField(String _)
        {
            return (_ != "" && _ != null && !(new Regex("(not applicable)|(N.?A)", RegexOptions.IgnoreCase).IsMatch(_)));
        }

        public int sortingHelper(Guid id1, Guid id2, List<Position> positions)
        {
            String position1Name = positions.Find(pos => pos.ID == id1).Name;
            String position2Name = positions.Find(pos => pos.ID == id2).Name;

            return sortHelperHelper(position1Name) - sortHelperHelper(position2Name);
        }

        public int sortHelperHelper(String name)
        {
            if (Regex.IsMatch(name, "TS 1"))
            {
                return 1;
            }
            if (Regex.IsMatch(name, "TS 2"))
            {
                return 2;
            }
            if (Regex.IsMatch(name, "TS 3"))
            {
                return 3;
            }
            if (Regex.IsMatch(name, "TS 4"))
            {
                return 4;
            }
            if (Regex.IsMatch(name, "TS 5"))
            {
                return 5;
            }
            if (Regex.IsMatch(name, "IA 1"))
            {
                return 5 + 1;
            }
            if (Regex.IsMatch(name, "IA 2"))
            {
                return 5 + 2;
            }
            if (Regex.IsMatch(name, "IA 3"))
            {
                return 5 + 3;
            }
            if (Regex.IsMatch(name, "IA 4"))
            {
                return 5 + 4;
            }
            if (Regex.IsMatch(name, "IA 5"))
            {
                return 5 + 5;
            }
            return 0;
        }
    }
}
