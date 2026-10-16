using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HyVo_Proj1SummerCampXF.Utilities
{
    public static class Jeeves
    {
        //Local
        //public static Uri DBUri = new Uri("http://localhost:48381/");
        //Azure
        public static Uri DBUri = new Uri("https://hyvoproj1summercampapi.azurewebsites.net");

        public static ApiException CreateApiException(HttpResponseMessage response)
        {
            var httpErrorObject = response.Content.ReadAsStringAsync().Result;

            var anonymousErrorObject = new { message = "", errors = new Dictionary<string, string[]>() };

            //deserialize
            var deserializedErrorObject = JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

            //exception
            var ex = new ApiException(response);

            if (deserializedErrorObject.message != null)
            {
                ex.Data.Add(-1, deserializedErrorObject.message);
            }
            if (deserializedErrorObject.errors != null)
            {
                foreach (var err in deserializedErrorObject.errors)
                {
                    ex.Data.Add(err.Key, err.Value[0]);
                }
            }
            return ex;
        }
    }
}
