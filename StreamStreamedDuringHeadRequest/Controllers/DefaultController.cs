using StreamStreamedDuringHeadRequest.IO;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace StreamStreamedDuringHeadRequest.Controllers
{
    public class DefaultController : ApiController
    {
        [HttpGet]
        [HttpHead]
        public HttpResponseMessage Get()
        {
            var response = Request.CreateResponse();
            var (stream, encoding) = GetStream();
            stream = new LoggingWrappingStream(stream);
            response.Content = new StreamContent(stream);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/plain")
            {
                CharSet = encoding.WebName,
            };
            return response;
        }

        (Stream, Encoding) GetStream()
        {
            var ms = new MemoryStream();
            var encoding = new UTF8Encoding(false, true);
            using (var writer = new StreamWriter(ms, encoding, 8192, true))
            {
                foreach (var i in Enumerable.Range(0, 24))
                {
                    writer.WriteLine("All play and no work makes Jack a poor boy.");
                }
            }
            ms.Position = 0;
            return (ms, encoding);
        }
    }
}
