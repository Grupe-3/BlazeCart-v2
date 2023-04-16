using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLZ.Functions.Extensions
{
    public static class HttpRespExt
    {
        static public async Task<HttpResponseData> Ok(this HttpRequestData req, string data)
        {
            var resp = req.CreateResponse(HttpStatusCode.OK);
            await resp.WriteStringAsync(data);
            return resp;
        }

        static public async Task<HttpResponseData> OkResp<T>(this HttpRequestData req, T data)
        {
            var resp = req.CreateResponse(HttpStatusCode.OK);
            await resp.WriteAsJsonAsync(data);
            return resp;
        }

        static public async Task<T> BodyAs<T>(this HttpRequestData req)
        {
            var obj = await req.ReadFromJsonAsync<T>();
            if (obj == null)
            {
                throw new ArgumentException("Invalid body!");
            }
            return obj;
        }
    }
}
