using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentDAL.Entities
{
    public class HealthRecord:BaseEntity
    {
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; }
        // last update is UpdatedAt in DataBase


    }
}
