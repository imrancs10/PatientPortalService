using System;
using System.Net;

namespace SWN.MobileService.Api.Attributes
{
    public class HttpStatusCodeAttribute : Attribute
    {
        public HttpStatusCodeAttribute(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }

    }
}
