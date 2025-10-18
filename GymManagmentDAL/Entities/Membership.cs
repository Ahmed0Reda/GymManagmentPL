﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagmentDAL.Entities
{
    public class Membership : BaseEntity
    {
        public DateTime EndDate { get; set; }
        public string Status
        {
            get
            {
                if (EndDate >= DateTime.Now)
                    return "Active";
                else
                    return "Expired";
            }
        }
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int planId   { get; set; }
        public Plan Plan { get; set; }

    }
}
