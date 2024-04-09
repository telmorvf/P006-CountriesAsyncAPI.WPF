using Countries_WPF.Models;
using System.Net;

namespace Countries_WPF.Services
{
    public class NetworkService
    {
        /// <summary>
        /// Check if exists connection with google site
        /// </summary>
        /// <returns></returns>
        public Response CheckConnection()
        {
            var client = new WebClient();
            try
            {
                // http://clients3.google.com/generate_204
                // https://restcountries.com/v3.1/all
                using (client.OpenRead("https://restcountries.com/v3.1/all"))
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
