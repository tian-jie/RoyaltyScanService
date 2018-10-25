using Infrastructure.Web.MVC;
using Innocellence.Owin.Security.SAML;
using Microsoft.IdentityModel.Protocols;
using Owin;
using System.Collections.Generic;
using System.IdentityModel.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Configuration;
//using Innocellence.Owin.Security.OAuthClient;

namespace Innocellence.WeChat.Domain.Authentication
{
    public partial class OWinAuthorizeProvider : IOWinAuthorizeProvider
    {
      public  void ConfigureAuth(IAppBuilder app)
        {
            var ssoPartnerSpId = Infrastructure.Web.Domain.Service.CommonService.GetSysConfig("SSOPartnerSpId", "CN_WeChat_Ent");
            //XXX SAML2
            app.UseSAML2Authentication(new SAML2AuthenticationOptions()
            {

                CallbackPath = new Microsoft.Owin.PathString("/sso/ssoresult"),
                LoginBackUrl = "/OWinLogin/SsoResult",
                //  CertFilePath =HttpContext.Current.Server.MapPath("~/cert.crt"),
                SigningKeys = new List<X509Certificate>() { new X509Certificate2(HttpContext.Current.Server.MapPath("~/cert.crt")) },
                AllowUnsolicitedAuthnResponse = true,
                //EntityId = new EntityId("lly-qa:saml2:idp"),
                EntityId = new EntityId(ssoPartnerSpId),
                Configuration = new WsFederationConfiguration()
                {
                    Issuer = WebConfigurationManager.AppSettings["SSOUrl"],
                    // SigningKeys= "",
                    TokenEndpoint = WebConfigurationManager.AppSettings["SSOUrl"]

                }
            });





            ////丁香园OAuth2
            //app.UseOAuthClientAuthentication(
            //  new OAuthClientAuthenticationOptions
            //  {
            //      AppId = "228714892725",
            //      AppSecret = "f5947310789ab6c91f3b8fa8642deb00",
            //      Endpoints = new OAuthClientAuthenticationOptions.OAuthClientAuthenticationEndpoints()
            //      {
            //          AuthorizationEndpoint = "https://auth.dxy.cn/conn/oauth2/authorize",
            //          TokenEndpoint = "https://auth.dxy.cn/conn/oauth2/accessToken",
            //          UserInfoEndpoint = "https://auth.dxy.cn/conn/oauth2/profile"
            //      }
            //  });
        }
    }
}
