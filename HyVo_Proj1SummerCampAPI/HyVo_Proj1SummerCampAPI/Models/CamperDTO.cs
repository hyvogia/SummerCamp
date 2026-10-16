using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HyVo_Proj1SummerCampAPI.Models
{
    [ModelMetadataType(typeof(CamperMetaData))]
    public class CamperDTO : IValidatableObject
    {
        public int ID { get; set; }

        public string Age
        {
            get
            {
                DateTime today = DateTime.Today;
                int a = today.Year - DOB.Year
                    - ((today.Month < DOB.Month || (today.Month == DOB.Month && today.Day < DOB.Day) ? 1 : 0));
                return a.ToString(); /*Note: You could add .PadLeft(3) but spaces disappear in a web page. */
            }
        }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime DOB { get; set; }

        public string Gender { get; set; }

        public string EMail { get; set; }

        public string Phone { get; set; }

        public byte[] RowVersion { get; set; }

        public int CompoundID { get; set; }
        public CompoundDTO Compound { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (int.Parse(Age) < 4)
            {
                yield return new ValidationResult("Camper must be at least 4 years old.", new[] { "DOB" });
            }
        }
    }
}
