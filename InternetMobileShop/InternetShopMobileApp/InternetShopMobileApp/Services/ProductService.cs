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

        public async Task<ProductServiceResult> GetAllCategories()
        {
            try
            {
                var response = await _client.GetAsync(URLHelper.APIURL + "/api/Cathegory").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var categories = JsonConvert.DeserializeObject<List<CategoryData>>(content); // Предполагается, что CategoryData - это ваш DTO для категорий

                    return new ProductServiceResult { Result = ProductOutput.SUCCESS, ProductData = categories };
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

        public async Task<ProductServiceResult> AddProduct(ProductData product)
        {
            try
            {
                product.Category = null;
                var jsonContent = JsonConvert.SerializeObject(product);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(URLHelper.APIURL + "/api/Product", content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var addedProduct = JsonConvert.DeserializeObject<ProductData>(responseContent);

                    return new ProductServiceResult { Result = ProductOutput.SUCCESS, ProductData = addedProduct };
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

        public async Task<ProductServiceResult> DeleteProduct(int productCode)
        {
            try
            {
                var response = await _client.DeleteAsync(URLHelper.APIURL + $"/api/Product/{productCode}").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    // Продукт успешно удален
                    return new ProductServiceResult { Result = ProductOutput.SUCCESS };
                }
                else
                {
                    // Обработка ошибок запроса
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Ошибка сервера: {errorContent}");
                    return new ProductServiceResult { Result = ProductOutput.ERROR };
                }
            }
            catch (Exception ex)
            {
                // В случае исключения
                Console.WriteLine($"Исключение: {ex.Message}");
                return new ProductServiceResult { Result = ProductOutput.EXCEPTION, ProductData = ex.Message };
            }
        }
    }
}
