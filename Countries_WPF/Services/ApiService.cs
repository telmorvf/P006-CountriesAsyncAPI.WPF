using Countries_WPF.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Countries_WPF.Services
{
    class ApiService
    {
        /// <summary>
        /// API Sercice - Connection and Download API Data
        /// </summary>
        /// <param name="urlBase"></param>
        /// <param name="controller"></param>
        /// <returns>A Task List</returns>
        public async Task<Response> GetCountries(string urlBase, string controller)
        {
            try
            {
                //http para fazer uma ligação externa
                var client = new HttpClient();

                // Endereço Base onde está a API
                client.BaseAddress = new Uri(urlBase);

                //Onde está o controlador API
                var response = await client.GetAsync(controller);

                //carrego os dados, numa string result
                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }
                //Converto todo o Jason para dentro de uma Lista objeto Country)
                var countries = JsonConvert.DeserializeObject<List<Country>>(result);

                return new Response
                {
                    IsSuccess = true,
                    //o meu objeto Result vai receber a Lista do Janson acima
                    Result = countries
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

        }

    }
}
