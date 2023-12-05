using InternetShopMobileApp.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InternetShopMobileApp.Services
{
    public class ProductServiceResult
    {
        public ProductOutput Result { get; set; }
        public dynamic ProductData { get; set; }
    }

    public enum ProductOutput
    {
        SUCCESS,
        ERROR,
        EXCEPTION
    }
    public class ProductService
    {
        private readonly HttpClient _client;

        public ProductService()
        {
            _client = HttpClientInstance.Client;
        }

        public async Task<ProductServiceResult> GetProduct(int productCode)
        {
            try
            {
                var response = await _client.GetAsync(URLHelper.APIURL + $"/api/Product/{productCode}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<ProductData>(content);

                    return new ProductServiceResult { Result = ProductOutput.SUCCESS, ProductData = orders };
                }
                else
                {
                    // Обработка ошибок запроса
                    return new ProductServiceResult { Result = ProductOutput.ERROR };
                }
            }
            catch (Exception ex)
            {
                // В случае исключения
                Console.WriteLine(ex.Message);
                return new ProductServiceResult { Result = ProductOutput.EXCEPTION, ProductData = ex.Message };
            }
        }
    }
}
