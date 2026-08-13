using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymMangmentDAL.Entities.Enums;
using static System.Collections.Specialized.BitVector32;

namespace GymMangmentDAL.Entities
{
    public class Trainer : GymUser
    {
        // HireDate is Created At of BasEntity
        public Specialist Specialist { get; set; }
        #region Session-Trainer
        public ICollection<Session> TrainerSession { get; set; } = null!;
     
        #endregion
    }
}
