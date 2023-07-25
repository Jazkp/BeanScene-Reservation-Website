using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoDB
{
    public class Product
    {
        public string Name { get; set; }

        public string[] Sittings { get; set; }
        public string[] Sizes { get; set; }
        public decimal[] Prices { get; set; }

        public bool Vegan { get; set; }
    }
}
