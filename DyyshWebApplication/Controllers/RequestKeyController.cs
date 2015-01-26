using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Web.Http;

namespace Dyysh.Controllers
{
    public class RequestKeyController : ApiController
    {
        [Route("api/RequestKey")]
        public HttpResponseMessage Get()
        {
            try
            {
                // Instantiate RSA CSP and export
                var cspParams = new CspParameters
                {
                    KeyContainerName = "serverContainer",
                    Flags = CspProviderFlags.UseArchivableKey |
                            CspProviderFlags.UseMachineKeyStore,
                };

                var rsa = new RSACryptoServiceProvider(2048, cspParams);
                var serverKey = rsa.ExportCspBlob(false);

                // Form response message
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(serverKey);
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
