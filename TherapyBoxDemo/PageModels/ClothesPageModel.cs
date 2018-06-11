using FreshMvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TherapyBoxDemo.Services;
using Xamarin.Forms;

namespace TherapyBoxDemo.PageModels
{
	public class ClothesPageModel : FreshBasePageModel
    {
		public ClothesPageModel ()
		{
            GetSalesAsync();

        }
        private const string WebServiceUrl = "https://therapy-box.co.uk/hackathon/clothing-api.php?username=swapnil";

        public async Task<List<RootObject>> GetSalesAsync()
        {
            try
            {
                var httpClient = new HttpClient();

                var json = await httpClient.GetStringAsync(WebServiceUrl);

                var taskModels = JsonConvert.DeserializeObject<List<RootObject>>(json);

                return taskModels;
            }
            catch(Exception e)
            {
                var exc = e.ToString();
                return null;
            }
        }
    }
}