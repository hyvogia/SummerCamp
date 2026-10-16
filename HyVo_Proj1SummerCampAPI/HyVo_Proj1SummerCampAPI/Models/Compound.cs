using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HyVo_Proj1SummerCampAPI.Models
{
    [ModelMetadataType(typeof(CompoundMetaData))]
    public class Compound : Auditable
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public ICollection<Camper> Campers { get; set; }

        public Compound() => this.Campers = new HashSet<Camper>();
    }
}
