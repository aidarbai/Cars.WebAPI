using Cars.COMMON.DTOs;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace Cars.BLL.Services.Import
{
    public class ImportService : IImportService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        

        public ImportService(
            IConfiguration configuration,
            HttpClient client)
        {
            _configuration = configuration;
            _client = client;
        }
        
        public async Task<List<CarImportDTO>> GetListAsync()
        {
            string url = _configuration["CarsDBListings"];
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var myJObject = JObject.Parse(responseBody);

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var importedCars = JsonConvert.DeserializeObject<List<CarImportDTO>>(myJObject["records"].ToString(), settings);
            return importedCars;
        }
    }
}
