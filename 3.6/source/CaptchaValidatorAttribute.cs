using System.Web.Mvc;
using Nop.Core.Infrastructure;
using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Nop.Services.Tasks;
using System.Threading.Tasks;

namespace Nop.Web.Framework.Security.Captcha
{
    public class CaptchaValidatorAttribute : ActionFilterAttribute
    {
        private const string RESPONSE_FIELD_KEY = "g-reCAPTCHA-response";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var valid = false;
            var captchaResponseValue = filterContext.HttpContext.Request.Form[RESPONSE_FIELD_KEY];
            if (!string.IsNullOrEmpty(captchaResponseValue))
            {
                var captchaSettings = EngineContext.Current.Resolve<CaptchaSettings>();
                if (captchaSettings.Enabled)
                    valid = System.Threading.Tasks.Task.Factory.StartNew(async () => await ValidateResponse(captchaResponseValue, captchaSettings.ReCaptchaPrivateKey).ConfigureAwait(false)).Unwrap().Result;
            }

            //this will push the result value into a parameter in our Action  
            filterContext.ActionParameters["captchaValid"] = valid;

            base.OnActionExecuting(filterContext);
        }

        private async Task<bool> ValidateResponse(string captchaResponseValue, string key)
        {
            var valid = false;
            var uribuilder = new UriBuilder("https://www.google.com/recaptcha/api/siteverify")
            {
                Query = string.Format("secret={0}&response={1}", key, captchaResponseValue)
            };
            using (var httpClient = new HttpClient())
            {
                var async = await httpClient.GetAsync(uribuilder.Uri);
                async.EnsureSuccessStatusCode();
                var readstring = await async.Content.ReadAsStringAsync();
                var resultobject = JObject.Parse(readstring);
                valid = resultobject.Value<bool>("success");
            }
            return valid;
        }
    }
}