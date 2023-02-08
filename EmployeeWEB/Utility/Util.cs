using EmployeeWEB.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeWEB.Utility
{
    public class Util<T> where T : class
    {
        private readonly IHttpClientFactory httpClientFactory;

        public Util(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<ModelStateError> CreateAsync(string url, T entity)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, Resource.contentType);

            var httpClient = httpClientFactory.CreateClient();

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound ||
                response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ModelStateError>(json);
            }
            return new ModelStateError()
            {
                Response = new Response()
                {
                    Errors = new List<Errors>()
                }
            };
        }
        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var httpClient = httpClientFactory.CreateClient();

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
            }
            else
            {
                Console.WriteLine($"Error getting data from API, status code: {response.StatusCode}");
                return null;
            }
        }
        public async Task<T> GetAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url + id);

            var httpClient = httpClientFactory.CreateClient();

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
            return null;
        }
        public async Task<ModelStateError> UpdateAsync(string url, T entity)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);

            request.Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, Resource.contentType);

            var httpClient = httpClientFactory.CreateClient();

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                new ModelStateError()
                {
                    Response = new Response()
                    {
                        Errors = new List<Errors>()
                        {
                            new Errors(){ErrorMessage = "No se encontro el empleado a actualizar"}
                        }
                    }
                };
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ModelStateError>(json);
            }
            return new ModelStateError()
            {
                Response = new Response()
                {
                    Errors = new List<Errors>()
                }
            };
        }
        public async Task<ModelStateError> DeleteAsync(string url, int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url + id);

            var httpClient = httpClientFactory.CreateClient();

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                new ModelStateError()
                {
                    Response = new Response()
                    {
                        Errors = new List<Errors>()
                        {
                            new Errors(){ErrorMessage = "No se encontro el empleado a actualizar"}
                        }
                    }
                };
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ModelStateError>(json);
            }
            return new ModelStateError()
            {
                Response = new Response()
                {
                    Errors = new List<Errors>()
                }
            };
        }
        public async Task<ModelStateError> LoginAsync(string url, T entity)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, Resource.contentType);

            var httpClient = httpClientFactory.CreateClient();

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound ||
                response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ModelStateError>(json);
            }
            ///Esta variable nos traera la el usuario, lista de roles y el token
            var content = await response.Content.ReadAsStringAsync();
            var modelError = JsonConvert.DeserializeObject<ModelStateError>(content);

            modelError.Response = new Response()
            {
                Errors = new List<Errors>()
            };
            return modelError;
        }
        public async Task<ModelStateError> RegisterAsync(string url, T entity)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, Resource.contentType);

            var httpClient = httpClientFactory.CreateClient();

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NotFound ||
                response.StatusCode == HttpStatusCode.InternalServerError)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ModelStateError>(json);
            }
            return new ModelStateError()
            {
                Response = new Response()
                {
                    Errors = new List<Errors>()
                }
            };
        }
    }
}
        
       
        