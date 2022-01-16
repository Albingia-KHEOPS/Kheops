using ALBINGIA.Framework.Common.AlbingiaExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ALBINGIA.Framework.Common.Tools
{
    /// <summary>
    ///RestfulServiceHelper
    /// </summary>
    /// <typeparam name="Tmodel">Type réference</typeparam>
    /// <typeparam name="TResourceIdentifier"></typeparam>
    public class RestfulServiceHelper<Tmodel> : IDisposable where Tmodel : class
    {
        #region Fields
        private HttpClient httpClient;
        private static HttpClientHandler clientHandler = new HttpClientHandler();
        private CancellationTokenSource cancellationTokenSource;
        private readonly string serviceBaseAddress;
        #endregion

        #region Propreties
        public string AddressSuffix { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of the RestfulServiceHelper class
        /// </summary>
        /// <param name="serviceBaseAddress">The base address of the Rest Service</param>
        /// <param name="timeout">The duration of the timeout in second</param>
        public RestfulServiceHelper(string serviceBaseAddress, int timeout = 5)
        {
            this.serviceBaseAddress = serviceBaseAddress;
            httpClient = InitHttpClient(serviceBaseAddress);
            cancellationTokenSource = GetCancellationTokenSource(timeout);
        }

        /// <summary>
        /// Creates a new instance of the RestfulServiceHelper class
        /// </summary>
        /// <param name="serviceBaseAddress">The base address of the Rest Service</param>
        /// <param name="addressSuffix">The querystring or extra address data</param>
        /// <param name="timeout">The duration of the timeout in second</param>
        public RestfulServiceHelper(string serviceBaseAddress, string addressSuffix, int timeout = 5) : this(serviceBaseAddress , timeout)
        {
            this.AddressSuffix = addressSuffix;
        }
        #endregion

        #region Methods

        #region Get
        /// <summary>
        /// Récupération asynchrone d'un énumeration du Tmodel
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Tmodel>> GetManyAsync()
        {
            var responseMessage = await httpClient.GetAsync(AddressSuffix);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadAsAsync<IEnumerable<Tmodel>>();
        }
        /// <summary>
        /// Récupération asynchrone du Tmodel
        /// </summary>
        /// <returns></returns>
        public async Task<Tmodel> GetAsync()
        {
            var responseMessage = await httpClient.GetAsync(AddressSuffix);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadAsAsync<Tmodel>();
        }
        /// <summary>
        /// Récupération synchrone du Tmodel
        /// </summary>
        /// <returns></returns>
        public Tmodel Get()
        {

            try
            {
                var response = httpClient.GetAsync(AddressSuffix, cancellationTokenSource.Token).GetAwaiter().GetResult();//.Result;
                // response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode == false)
                {
                    throw new AlbApiException
                    {
                        StatusCode = response.StatusCode,
                        Content = response
                    };
                }

                return response.Content.ReadAsAsync<Tmodel>().Result;
            }
            catch (Exception e)
            {
                var canceled = e as TaskCanceledException;

                if (canceled != null && cancellationTokenSource?.Token.IsCancellationRequested==true)
                {
                    throw new AlbApiException { StatusCode = HttpStatusCode.RequestTimeout };
                }
                throw;
            }


        }
        #endregion

        #region Post
        public async Task<Tresult> PostAsync<Tresult>(Tmodel model)
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            HttpContent content = new ObjectContent<Tmodel>(model, jsonFormatter);
            var response = await httpClient.PostAsync(AddressSuffix, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Tresult>();
        }
        public async Task<Tresult> PostAsJsonAsync<Tresult>(Tmodel model)
        {
            var response = await httpClient.PostAsJsonAsync(AddressSuffix, model);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Tresult>();
        }
        public Tresult Post<Tresult>(Tmodel model)
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            HttpContent content = new ObjectContent<Tmodel>(model, jsonFormatter);
            var response = httpClient.PostAsync(AddressSuffix, content).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsAsync<Tresult>().Result;
        }
        public Tresult PostAsJson<Tresult>(Tmodel model)
        {
            var response = httpClient.PostAsJsonAsync(AddressSuffix, model).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsAsync<Tresult>().Result;
        }
        #endregion

        #region Initialisation client
        protected  HttpClient InitHttpClient(string serviceBaseAddress)
        {
            //httpClient = new HttpClient { BaseAddress = new Uri(serviceBaseAddress) };
            httpClient = new HttpClient(clientHandler,false){ BaseAddress = new Uri(serviceBaseAddress) };
          //  httpClient.DefaultRequestHeaders.ConnectionClose = true;
            //httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse(jsonMediaType));
            //httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("gzip"));
            //httpClient.DefaultRequestHeaders.AcceptEncoding.Add(StringWithQualityHeaderValue.Parse("defalte"));
            // httpClient.BaseAddress = new Uri(serviceBaseAddress);
            return httpClient;
        }
        protected  CancellationTokenSource GetCancellationTokenSource(int timeout)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(timeout));
            return cts;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (httpClient != null)
                {
                    var hc = httpClient;
                    httpClient = null;
                    hc.Dispose();
                }

            }
        }
        #endregion

        #endregion

    }
}
