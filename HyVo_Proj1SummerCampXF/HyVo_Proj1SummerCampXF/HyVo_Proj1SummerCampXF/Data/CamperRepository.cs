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
    public class CamperRepository : ICamperRepository
    {
        readonly HttpClient client = new HttpClient();

        public CamperRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<List<Camper>> GetCampers()
        {
            var response = await client.GetAsync("api/campers");
            if (response.IsSuccessStatusCode)
            {
                List<Camper> campers = await response.Content.ReadAsAsync<List<Camper>>();
                return campers;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task<List<Camper>> GetCampersByCompound(int CompoundID)
        {
            var response = await client.GetAsync($"api/campers/byCompound/{CompoundID}");
            if (response.IsSuccessStatusCode)
            {
                List<Camper> campers = await response.Content.ReadAsAsync<List<Camper>>();
                return campers;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task<Camper> GetCamper(int ID)
        {
            var response = await client.GetAsync($"api/campers/{ID}");
            if (response.IsSuccessStatusCode)
            {
                Camper camper = await response.Content.ReadAsAsync<Camper>();
                return camper;
            }
            else
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task AddCamper(Camper camperToAdd)
        {
            camperToAdd.Compound = null;
            var response = await client.PostAsJsonAsync("api/campers", camperToAdd);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task UpdateCamper(Camper camperToUpdate)
        {
            camperToUpdate.Compound = null;
            var response = await client.PutAsJsonAsync($"api/campers/{camperToUpdate.ID}", camperToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task DeleteCamper(Camper camperToDelete)
        {
            var response = await client.DeleteAsync($"api/campers/{camperToDelete.ID}");
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }
    }
}
