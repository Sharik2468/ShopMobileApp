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
    public class OrderServiceResult
    {
        public OrderOutput Result { get; set; }
        public dynamic OrderData { get; set; }
    }

    public enum OrderOutput
    {
        SUCCESS,
        ERROR,
        EXCEPTION
    }

    public class OrderService
    {
        private readonly HttpClient _client;

        public OrderService()
        {
            _client = HttpClientInstance.Client;
        }

        public async Task<OrderServiceResult> GetAllPreparedUserOrders(int userID)
        {
            try
            {
                var response = await _client.GetAsync(URLHelper.APIURL + $"/api/Order/GetAllPreparedUserOrder/{userID}").ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<List<OrderData>>(content); // Предполагается, что OrderData - это ваш DTO

                    return new OrderServiceResult { Result = OrderOutput.SUCCESS, OrderData = orders };
                }
                else
                {
                    // Обработка ошибок запроса
                    return new OrderServiceResult { Result = OrderOutput.ERROR };
                }
            }
            catch (Exception ex)
            {
                // В случае исключения
                Console.WriteLine(ex.Message);
                return new OrderServiceResult { Result = OrderOutput.EXCEPTION, OrderData = ex.Message };
            }
        }

        public async Task<OrderServiceResult> GetAllPreparedOrders()
        {
            try
            {
                var response = await _client.GetAsync(URLHelper.APIURL + "/api/Order/GetAllPreparedOrder").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<List<OrderData>>(content);

                    return new OrderServiceResult { Result = OrderOutput.SUCCESS, OrderData = orders };
                }
                else
                {
                    // Обработка ошибок запроса
                    return new OrderServiceResult { Result = OrderOutput.ERROR };
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений при отправке запроса
                Console.WriteLine(ex.Message);
                return new OrderServiceResult { Result = OrderOutput.EXCEPTION, OrderData = ex.Message };
            }
        }

        public async Task<OrderServiceResult> DeleteOrder(int orderCode)
        {
            try
            {
                var response = await _client.PutAsync(URLHelper.APIURL + $"/api/Order/DeleteOrder/{orderCode}", null).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    // Если запрос успешно обработан
                    return new OrderServiceResult { Result = OrderOutput.SUCCESS };
                }
                else
                {
                    // Обработка ошибок запроса
                    return new OrderServiceResult { Result = OrderOutput.ERROR };
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений при отправке запроса
                Console.WriteLine(ex.Message);
                return new OrderServiceResult { Result = OrderOutput.EXCEPTION, OrderData = ex.Message };
            }
        }

        public async Task<OrderServiceResult> PayOrder(int orderCode, int userID)
        {
            try
            {
                var response = await _client.PutAsync(URLHelper.APIURL + $"/api/Order/PaidOrder/{orderCode}/{userID}", null).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    // Если запрос успешно обработан
                    return new OrderServiceResult { Result = OrderOutput.SUCCESS };
                }
                else
                {
                    // Обработка ошибок запроса
                    return new OrderServiceResult { Result = OrderOutput.ERROR };
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений при отправке запроса
                Console.WriteLine(ex.Message);
                return new OrderServiceResult { Result = OrderOutput.EXCEPTION, OrderData = ex.Message };
            }
        }
    }
}
