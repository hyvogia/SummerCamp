using HyVo_Proj1SummerCampXF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HyVo_Proj1SummerCampXF.Data
{
    interface ICompoundRepository
    {
        Task<List<Compound>> GetCompounds();
        Task<Compound> GetCompound(int ID);
        Task AddCompound(Compound compoundToAdd);
        Task UpdateCompound(Compound compoundToUpdate);
        Task DeleteCompound(Compound compoundToDelete);
    }
}
