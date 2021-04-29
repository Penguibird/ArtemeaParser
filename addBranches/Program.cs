using System;
using System.Collections.Generic;
using System.Linq;
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

            String[] names = { "Integration Technical Specialist",
             "Java Midleware technical specialist",
              "Unix Technical Specialist",
               "ERP Specialist",
                "Server Implementation Specialist",
                 "Technical specialist - Public Cloud",
                  "Backup Technical Specialist",
                  "Storage Technical Specialist",
                  "EDM Technical Specialist",
                   };
            Guid parentID = new Guid();
            Branch parent = new Branch()
            {
                ID = parentID,
                ChildrenIDs = new List<Guid>(),
                name = "Linux",
                positionIDs = new List<Guid>(),
            };
            foreach (String name in names)
            {
                Branch branch = (await _dbContext.FindByAttribute<Branch, String>("branches", "name", name))[0];
                branch.parentID = parentID;
                parent.ChildrenIDs.Add(branch.ID);
                await _dbContext.Replace<Branch>("branches", branch.ID, branch);
                Console.WriteLine(name);
            }
            _dbContext.Add("branches", parent);



            // foreach (Branch branch in branches)
            // {
            //     branch.positions = new List<Position>();
            //     foreach (Guid id in branch.positionIDs)
            //     {
            //         branch.positions.Add(await _dbContext.FindById<Position>("positions", id));
            //     }
            //     List<Guid> posIds = branch.positionIDs.ToList<Guid>();
            //     posIds.Sort(delegate (Guid id1, Guid id2) { return p.sortingHelper(id1, id2, branch.positions.ToList<Position>()); });
            //     branch.positionIDs = posIds;
            //     await _dbContext.Replace<Branch>("branches", branch.ID, branch);
            // }
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
