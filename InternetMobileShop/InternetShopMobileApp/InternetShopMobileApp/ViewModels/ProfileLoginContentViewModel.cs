using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using InternetShopMobileApp.DTOs;
using Newtonsoft.Json;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace InternetShopMobileApp.ViewModels
{
    public class ProfileLoginContentViewModel : ReactiveObject, IRoutableViewModel
    {
        private String _selectedLogin;
        public String SelectedLogin
        {
            get => _selectedLogin;
            set { this.RaiseAndSetIfChanged(ref _selectedLogin, value); }
        }

        private String _selectedPassword;
        public String SelectedPassword
        {
            get => _selectedPassword;
            set { this.RaiseAndSetIfChanged(ref _selectedPassword, value); }
        }
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ReactiveCommand<Unit, IRoutableViewModel> LoginCommand { get; }

        public ProfileLoginContentViewModel(IScreen screen)
        {
            HostScreen = screen;

            // Создайте экземпляр команды и укажите делегат для выполнения
            LoginCommand = ReactiveCommand.CreateFromTask<Unit, IRoutableViewModel>(TryLogin);
        }

        private async Task<IRoutableViewModel> TryLogin(Unit _)
        {
            var loginData = new
            {
                email = SelectedLogin,
                password = SelectedPassword,
                rememberMe = true
            };

            var json = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Создание HttpClientHandler с включенным UseCookies
            var client = HttpClientInstance.Client;

            // Использование HttpClient с созданным HttpClientHandler

            try
            {
                var response = await client.PostAsync(URLHelper.APIURL + "/api/account/login", content);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Created)
                        InteractiveContainer.ShowToast(new TextBlock() { Text = "Произошла ошибка авторизации!", Margin = new Thickness(15, 8) }, 5);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        HostScreen.Router.Navigate.Execute(new ProfileContentViewModel(HostScreen));

                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений при отправке запроса
                Console.WriteLine(ex.Message);
                // Возможно, показать уведомление об ошибке
            }


            return null;
        }

    }
}
