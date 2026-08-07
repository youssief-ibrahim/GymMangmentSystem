using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentDAL.Entities
{
    public class Plan :  BaseEntity
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Description { get; set; } = null!;
        public int DurationInDays { get; set; } 
        public bool IsActive { get; set; }

        #region MemberShip-Plan
        public ICollection<MemberShip> PlanMember { get; set; } = null!;
        #endregion
    }
}
