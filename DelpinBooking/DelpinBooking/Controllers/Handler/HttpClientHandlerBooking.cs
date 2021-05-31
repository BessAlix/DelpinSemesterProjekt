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
    public class HttpClientHandlerBooking : IHttpClientHandler<Booking>

    {
        private readonly string ApiUrl = "https://localhost:5001/api/BookingAPI/";
        public async Task<Booking> Create(Booking booking)
        {

            using (var httpClient = new HttpClient())
            {
                string method = "Create/";
                using (var response = await httpClient.PostAsJsonAsync<Booking>(ApiUrl + method, booking))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Booking>(apiResponse);
                }

            }
        }
        public async Task<Booking> Delete(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string method = "Delete/";
                Booking booking = new Booking {Id = id };
                using (var response = await httpClient.PostAsJsonAsync<Booking>(ApiUrl + method, booking)) 
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Booking>(apiResponse);
                }
            }
        }

        public async Task<Booking> Get(int id)
        {
            using (var httpClient = new HttpClient())
            {
                string method = "GetBooking/";
                using (var response = await httpClient.GetAsync(ApiUrl + method + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Booking>(apiResponse);
                }
            }
        }

        public async Task<List<Booking>> GetAll(string queryString)
        {
            using (var httpClient = new HttpClient())
            {
                string method = "GetAllBookings?";

                using (var response = await httpClient.GetAsync(ApiUrl + method + queryString))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<Booking>>(apiResponse);
                }
            }

        }
       public async Task <Dictionary<string,string>> Update(Booking item)
       {
            using (var httpClient = new HttpClient())
            {
                string method = "Update/";
                using (var response = await httpClient.PutAsJsonAsync<Booking>(ApiUrl + method, item))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<Dictionary<string, string>>(apiResponse);
                }

            }

       }
    }
}

