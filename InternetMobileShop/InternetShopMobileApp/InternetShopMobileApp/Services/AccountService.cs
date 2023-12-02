using Avalonia.Controls;
using Avalonia;
using InternetShopMobileApp.DTOs;
using Newtonsoft.Json;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InternetShopMobileApp.Services
{
    public class AuthenticationResult
    {
        public AccountOutput Result { get; set; }
        public dynamic UserData { get; set; }
    }

    public enum AccountOutput
    {
        SUCCESS,
        ERROR,
        UNAUTHORIZED
    }

    public class AccountService
    {
        HttpClient client;
        public AccountService()
        {
            client = HttpClientInstance.Client;
        }

        public async Task<AuthenticationResult> TryLogOff()
        {
            try
            {
                // Отправка POST-запроса на сервер для выхода из аккаунта
                var response = await client.PostAsync(URLHelper.APIURL + "/api/account/logoff", new StringContent("")).ConfigureAwait(false);
                if (response != null && response.ReasonPhrase == "Unauthorized")
                {
                    return new AuthenticationResult { Result = AccountOutput.UNAUTHORIZED };
                }

                if (response.IsSuccessStatusCode)
                {
                    // Обработка успешного выхода из аккаунта
                    // Например, очистить данные пользователя и перенаправить на страницу входа
                    //User = null;
                    return new AuthenticationResult { Result = AccountOutput.SUCCESS, UserData = null };
                    // Перенаправление на страницу входа или обновление UI
                }
                else
                {
                    // Обработка ошибок
                    Console.WriteLine("Ошибка выхода из аккаунта");
                    return new AuthenticationResult { Result = AccountOutput.ERROR };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении запроса: {ex.Message}");
                return new AuthenticationResult { Result = AccountOutput.ERROR };
            }
        }

        public async Task<AuthenticationResult> IsAuthentificated()
        {
            try
            {
                // Проверка авторизации пользователя
                var authResponse = await client.GetAsync(URLHelper.APIURL + "/api/account/isauthenticated").ConfigureAwait(false);
                if (authResponse != null && authResponse.ReasonPhrase == "Unauthorized")
                {
                    return new AuthenticationResult { Result = AccountOutput.UNAUTHORIZED };
                    //IsLoggedIn = false;
                }
                if (authResponse.IsSuccessStatusCode)
                {
                    var authContent = await authResponse.Content.ReadAsStringAsync();
                    var userData = JsonConvert.DeserializeObject<dynamic>(authContent);

                    return new AuthenticationResult { Result = AccountOutput.SUCCESS, UserData = userData };

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //InteractiveContainer.ShowDialog(new TextBlock() { Text = "Произошла ошибка при выполнении запроса!", Margin = new Thickness(15, 8) });
                return new AuthenticationResult { Result = AccountOutput.ERROR };
            }
            return new AuthenticationResult { Result = AccountOutput.ERROR };
        }

    }
}
