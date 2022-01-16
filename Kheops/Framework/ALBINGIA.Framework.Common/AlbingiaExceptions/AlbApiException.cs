using System;
using System.Net;
using System.Net.Http;


namespace ALBINGIA.Framework.Common.AlbingiaExceptions
{
    public class AlbApiException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public HttpResponseMessage Content { get; set; }
    }
}
