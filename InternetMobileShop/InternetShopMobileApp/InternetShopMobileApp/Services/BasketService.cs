using Avalonia.Threading;
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
    public class BasketServiceResult
    {
        public BasketOutput Result { get; set; }
        public dynamic BasketData { get; set; }
    }
    public enum BasketOutput
    {
        SUCCESS,
        ERROR,
        EXCEPTION
    }

    public class BasketService
    {

        HttpClient client;
        public BasketService()
        {
            client = HttpClientInstance.Client;
        }

        public async Task<BasketServiceResult> LoadItems(int userID)
        {
            HttpClient client = HttpClientInstance.Client;

            try
            {
                var response = await client.GetAsync(URLHelper.APIURL + $"/api/OrderItem/{userID}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var orderItems = JsonConvert.DeserializeObject<List<OrderItemData>>(content);

                    return new BasketServiceResult { Result = BasketOutput.SUCCESS, BasketData = orderItems };
                }
                else
                {
                    // Обработка ошибок запроса
                    return new BasketServiceResult { Result = BasketOutput.ERROR };
                }

            }
            catch (Exception ex)
            {
                // Обработка исключений при отправке запроса
                Console.WriteLine(ex.Message);

                return new BasketServiceResult { Result = BasketOutput.EXCEPTION };
            }
        }

        public async Task<BasketServiceResult> DeleteOrderItem(int orderItemCode)
        {
            try
            {
                var response = await client.DeleteAsync(URLHelper.APIURL + $"/api/OrderItem/{orderItemCode}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    // Если успешно, возвращаем успех
                    return new BasketServiceResult { Result = BasketOutput.SUCCESS };
                }
                else
                {
                    // Если статус код не успешный, возвращаем ошибку
                    return new BasketServiceResult { Result = BasketOutput.ERROR };
                }
            }
            catch (Exception ex)
            {
                // В случае исключения возвращаем EXCEPTION с дополнительной информацией
                Console.WriteLine(ex.Message);
                return new BasketServiceResult { Result = BasketOutput.EXCEPTION, BasketData = ex.Message };
            }
        }

        public async Task<BasketServiceResult> AddProductToBasket(int userID, int productCode)
        {
            try
            {
                var content = new StringContent("", Encoding.UTF8, "application/json");
                var response = await client.PostAsync(URLHelper.APIURL + $"/api/OrderItem/{userID}.{productCode}", content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    // Если успешно, возвращаем успех
                    return new BasketServiceResult { Result = BasketOutput.SUCCESS };
                }
                else
                {
                    // Если статус код не успешный, возвращаем ошибку
                    return new BasketServiceResult { Result = BasketOutput.ERROR };
                }
            }
            catch (Exception ex)
            {
                // В случае исключения возвращаем EXCEPTION
                Console.WriteLine(ex.Message);
                return new BasketServiceResult { Result = BasketOutput.EXCEPTION, BasketData = ex.Message };
            }
        }

        public async Task<BasketServiceResult> UpdateOrderItemQuantity(int orderItemCode, int quantity)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(new { Quantity = quantity }), Encoding.UTF8, "application/json");
                var response = await client.PutAsync(URLHelper.APIURL + $"/api/OrderItem/{orderItemCode}/{quantity}", content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    // Если успешно, возвращаем успех
                    return new BasketServiceResult { Result = BasketOutput.SUCCESS };
                }
                else
                {
                    // Если статус код не успешный, возвращаем ошибку
                    return new BasketServiceResult { Result = BasketOutput.ERROR };
                }
            }
            catch (Exception ex)
            {
                // В случае исключения возвращаем EXCEPTION
                Console.WriteLine(ex.Message);
                return new BasketServiceResult { Result = BasketOutput.EXCEPTION, BasketData = ex.Message };
            }
        }

        public async Task<BasketServiceResult> PrepareOrder(int? orderCode)
        {
            try
            {
                var response = await client.PutAsync(URLHelper.APIURL + $"/api/Order/PrepareOrder/{orderCode}", null).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    // Если успешно, возвращаем успех
                    return new BasketServiceResult { Result = BasketOutput.SUCCESS };
                }
                else
                {
                    // Если статус код не успешный, возвращаем ошибку
                    return new BasketServiceResult { Result = BasketOutput.ERROR };
                }
            }
            catch (Exception ex)
            {
                // В случае исключения возвращаем EXCEPTION с дополнительной информацией
                Console.WriteLine(ex.Message);
                return new BasketServiceResult { Result = BasketOutput.EXCEPTION, BasketData = ex.Message };
            }
        }

        public async Task<BasketServiceResult> GetAllOrderItemStatuses()
        {
            try
            {
                var response = await client.GetAsync(URLHelper.APIURL + "/api/OrderItem/GetAllOrderItemStatuses").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<List<dynamic>>(content); // Адаптируйте тип данных, если необходимо

                    return new BasketServiceResult { Result = BasketOutput.SUCCESS, BasketData = data };
                }
                else
                {
                    // Обработка ошибок запроса
                    return new BasketServiceResult { Result = BasketOutput.ERROR };
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений при отправке запроса
                Console.WriteLine(ex.Message);
                return new BasketServiceResult { Result = BasketOutput.EXCEPTION, BasketData = ex.Message };
            }
        }

        public async Task<BasketServiceResult> UpdateOrderItemStatus(int orderItemCode, int newStatus)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(new { NewStatus = newStatus }), Encoding.UTF8, "application/json");
                var response = await client.PutAsync(URLHelper.APIURL + $"/api/OrderItem/PutNewStatusID/{orderItemCode}/{newStatus}", content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    // Если успешно, возвращаем успех
                    return new BasketServiceResult { Result = BasketOutput.SUCCESS };
                }
                else
                {
                    // Если статус код не успешный, возвращаем ошибку
                    return new BasketServiceResult { Result = BasketOutput.ERROR };
                }
            }
            catch (Exception ex)
            {
                // В случае исключения возвращаем EXCEPTION с дополнительной информацией
                Console.WriteLine(ex.Message);
                return new BasketServiceResult { Result = BasketOutput.EXCEPTION, BasketData = ex.Message };
            }
        }

    }
}
