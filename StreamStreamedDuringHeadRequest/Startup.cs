using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(StreamStreamedDuringHeadRequest.Startup))]

namespace StreamStreamedDuringHeadRequest
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            app.UseWebApi(httpConfiguration);
        }
    }
}
