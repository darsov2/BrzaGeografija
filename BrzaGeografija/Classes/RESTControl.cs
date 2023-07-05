using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace BrzaGeografija.Classes
{
    public class RESTControl
    {
        private static string URL = "https://geo-kviz-default-rtdb.europe-west1.firebasedatabase.app/countries";
        public static async Task<string> GetAll()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                using(HttpResponseMessage res = await httpClient.GetAsync(URL))
                {
                    using(HttpContent httpContent = res.Content)
                    {
                        string data = await httpContent.ReadAsStringAsync();
                        if(data != null)
                        {
                            return data;
                        }
                    }
                }
            }
            return String.Empty;
        }
    }
}
