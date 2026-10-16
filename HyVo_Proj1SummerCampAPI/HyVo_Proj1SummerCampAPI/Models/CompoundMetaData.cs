using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HyVo_Proj1SummerCampAPI.Models
{
    public class CompoundMetaData : Auditable
    {
        [Display(Name = "Compound Name")]
        [Required(ErrorMessage = "You cannot leave the compound name blank.")]
        [StringLength(50, ErrorMessage = "Compound name cannot be more than 50 characters long.")]
        public string Name { get; set; }

        [Display(Name = "Campers")]
        public ICollection<Camper> Campers { get; set; }
    }
}
