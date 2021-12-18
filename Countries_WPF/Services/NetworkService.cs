using Countries_WPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Countries_WPF.Services
{
    public class NetworkService
    {
        /// <summary>
        /// Check if exists connection with google site
        /// </summary>
        /// <returns></returns>
        public Response CheckConnection() //esta class só tem 1 método que vai checar se há internet
        {

            var client = new WebClient();

            try
            {
                //Link que vou usar para saber se tenho ligação à internet
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return new Response
                    {
                        IsSuccess = true,
                    };
                }
            }
            catch (System.Exception)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = "Check your Internet Connection"
                };
            }
        }


    }
}
