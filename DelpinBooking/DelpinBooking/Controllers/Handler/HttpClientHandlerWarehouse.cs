using DelpinBooking.Models;
using DelpinBooking.Models.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace DelpinBooking.Controllers.Handler
{
    public class HttpClientHandlerWarehouse : IHttpClientHandler<Warehouse>
    {
        private readonly string ApiUrl = "https://localhost:5001/api/WarehouseAPI/";
        public async Task<Warehouse> Create(Warehouse item)
        {
            using (var httpClient = new HttpClient())
            {
                string method = "Create/";
                using (var response = await httpClient.PostAsJsonAsync<Warehouse>(ApiUrl + method, item))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Warehouse>(apiResponse);
                }

            }
        }

        public async Task<Warehouse> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string method = "Delete/";
                using (var response = await httpClient.DeleteAsync(ApiUrl + method + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Warehouse>(apiResponse);
                }
            }
        }

        public async Task<Warehouse> Get(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string method = "GetWarehouse/";
                using (var response = await httpClient.GetAsync(ApiUrl + method + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Warehouse>(apiResponse);
                }
            }
        }

        public async Task<List<Warehouse>> GetAll(string queryString)
        {
            using (var httpClient = new HttpClient())
            {
                string method = "GetAllWarehouses?";

                using (var response = await httpClient.GetAsync(ApiUrl + method + queryString))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Warehouse>>(apiResponse);
                }
            }
        }

        public async Task<Dictionary<string, string>> Update(Warehouse item)
        {
            using (var httpClient = new HttpClient())
            {
                string method = "Update/";
                using (var response = await httpClient.PutAsJsonAsync<Warehouse>(ApiUrl + method, item))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);
                }

            }
        }
    }
}
