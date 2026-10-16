using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HyVo_Proj1SummerCampAPI.Models
{
    [ModelMetadataType(typeof(CompoundMetaData))]
    public class CompoundDTO
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public byte[] RowVersion { get; set; }

        public ICollection<CamperDTO> Campers { get; set; }

        public CompoundDTO() => this.Campers = new HashSet<CamperDTO>();
    }
}
