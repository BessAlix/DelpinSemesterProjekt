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
    public class HttpClientHandlerMachine : IHttpClientHandler<Machine>
    {
        private readonly string ApiUrl = "https://localhost:5001/api/MachineAPI/";
        public async Task<Machine> Create(Machine item)
        {
            using (var httpClient = new HttpClient())
            {
                string method = "Create/";
                using (var response = await httpClient.PostAsJsonAsync<Machine>(ApiUrl + method, item))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Machine>(apiResponse);
                }

            }
        }
        public async Task<Machine> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string method = "Delete/";
                using (var response = await httpClient.DeleteAsync(ApiUrl + method + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Machine>(apiResponse);
                }
            }
        }

        public async Task<Machine> Get(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string method = "GetMachine/";
                using (var response = await httpClient.GetAsync(ApiUrl + method + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Machine>(apiResponse);
                }
            }
        }

        public async Task<List<Machine>> GetAll(string queryString)
        {
            using (var httpClient = new HttpClient())
            {
                string method = "GetAllMachines?";

                using (var response = await httpClient.GetAsync(ApiUrl + method + queryString))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Machine>>(apiResponse);
                }
            }
        }

        public async Task<Dictionary<string, string>> Update(Machine item)
        {
            using (var httpClient = new HttpClient())
            {
                string method = "Update/";
                using (var response = await httpClient.PutAsJsonAsync<Machine>(ApiUrl + method, item))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);
                }

            }
        }
    }
}
