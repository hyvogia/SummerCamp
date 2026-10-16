using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace HyVo_Proj1SummerCampXF.Utilities
{
    public class ApiException : Exception
    {
        public HttpResponseMessage Response { get; set; }

        public HttpStatusCode StatusCode => this.Response.StatusCode;

        public IEnumerable<string> Errors => this.Data.Values.Cast<string>().ToList();

        public ApiException(HttpResponseMessage response)
        {
            this.Response = response;
        }
    }
}
