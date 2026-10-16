using System;
using System.Collections.Generic;
using System.Text;

namespace HyVo_Proj1SummerCampXF.Models
{
    public class Compound
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public byte[] RowVersion { get; set; }

        public ICollection<Camper> Campers { get; set; }

        public Compound() => this.Campers = new HashSet<Camper>();
    }
}
