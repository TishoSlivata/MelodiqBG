using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using MelodiaBG.Extensions;

namespace MelodiaBG.Extensions
{
    public static class CookieExtensions
    {
        public static void SetObject(this IResponseCookies cookies, string key, object value)
        {
            cookies.Append(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this IRequestCookieCollection cookies, string key)
        {
            var value = cookies[key];
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
