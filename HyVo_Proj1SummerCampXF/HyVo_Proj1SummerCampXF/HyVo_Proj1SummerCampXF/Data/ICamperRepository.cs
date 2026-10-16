using HyVo_Proj1SummerCampXF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HyVo_Proj1SummerCampXF.Data
{
    interface ICamperRepository
    {
        Task<List<Camper>> GetCampers();
        Task<Camper> GetCamper(int ID);
        Task<List<Camper>> GetCampersByCompound(int CompoundID);
        Task AddCamper(Camper camperToAdd);
        Task UpdateCamper(Camper camperToUpdate);
        Task DeleteCamper(Camper camperToDelete);
    }
}
