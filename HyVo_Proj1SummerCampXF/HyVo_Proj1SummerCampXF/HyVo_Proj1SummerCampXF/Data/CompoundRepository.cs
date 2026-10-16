using HyVo_Proj1SummerCampXF.Models;
using HyVo_Proj1SummerCampXF.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HyVo_Proj1SummerCampXF.Data
{
    public class CompoundRepository : ICompoundRepository
    {
        readonly HttpClient client = new HttpClient();

        public CompoundRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Compound>> GetCompounds()
        {
            var response = await client.GetAsync("api/compounds");
            if (response.IsSuccessStatusCode)
            {
                List<Compound> compounds = await response.Content.ReadAsAsync<List<Compound>>();
                return compounds;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }

        }

        public async Task<Compound> GetCompound(int ID)
        {
            var response = await client.GetAsync($"api/compounds/{ID}");
            if (response.IsSuccessStatusCode)
            {
                Compound compound = await response.Content.ReadAsAsync<Compound>();
                return compound;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task AddCompound(Compound compoundToAdd)
        {
            var response = await client.PostAsJsonAsync("api/compounds", compoundToAdd);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task UpdateCompound(Compound compoundToUpdate)
        {
            var response = await client.PutAsJsonAsync($"api/compounds/{compoundToUpdate.ID}", compoundToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task DeleteCompound(Compound compoundToDelete)
        {
            var response = await client.DeleteAsync($"api/compounds/{compoundToDelete.ID}");
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }

        }
    }
}
