using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingSource.Models
{
    public class DataToComparison
    {
        public RepairType RepairType { get; set; }
        public ObjectType RepairObject { get; set; }
        public decimal Square { get; set; }
        public Service Designer { get; set; }
        public Service Engeneer { get; set; }
        public decimal FullPrice { get; set; }
        public string Address { get; set; }
    }
}
