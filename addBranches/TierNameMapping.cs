using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class TierNameMapping
    {
        public TierNameMapping(int index)
        {
            csvIndex = index + 5;
            if (index <= 5)
            {
                nameSuffix = "TS " + index;
            }
            else
            {
                nameSuffix = "IA " + (index - 5);
            }
            if (index <= 3)
            {
                Tier = 2;
            }
            else if (index > 7)
            {
                Tier = 4;
            }
            else
            {
                Tier = 3;
            }
        }
        public int csvIndex { get; set; }
        public int Tier { get; set; }
        public String nameSuffix { get; set; }
    }
}
