using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentDAL.Entities
{
    public class Member : GymUser
    {
        // joinDate== CreatedAt of BaseEnity
        public string? Photo { get; set; }

        #region Member - Healthrecord
        public HealthRecord HealthRecord { get; set; } = null!;
        #endregion

        #region MemberShip-Member
        public ICollection<MemberShip> memberShips { get; set; } = null!;
        #endregion

        #region Member-MemberSession
        public ICollection<MemberSession> MemberSession { get; set; } = null!;
        #endregion
    }
}
