using Avalonia.Controls;
using Avalonia;
using InternetShopMobileApp.DTOs;
using InternetShopMobileApp.Views;
using Newtonsoft.Json;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.Net.Http;
using System.Reactive;
using System.Threading.Tasks;
using InternetShopMobileApp.Services;

namespace InternetShopMobileApp.ViewModels
{
    public class ProfileContentViewModel : ReactiveObject, IRoutableViewModel
    {
        private ClientData _user;
        public ClientData User
        {
            get => _user;
            set
            {
                this.RaiseAndSetIfChanged(ref _user, value);
                WelcomeUser = string.Empty;
                RoleText = string.Empty;
                IsLoggedIn = value != null;
            }
        }

        private string _welcomeUser;
        public string WelcomeUser
        {
            get => _welcomeUser;
            private set { this.RaiseAndSetIfChanged(ref _welcomeUser, $"Здравствуйте, {User?.Name ?? "гость"}!"); }
        }

        private string _roleText;
        public string RoleText
        {
            get => _roleText;
            private set
            {
                string outputText = "";
                if (User != null)
                    outputText = User.Roles == "user" ? $"Ваша роль: Пользователь" : $"Ваша роль: Администратор";
                this.RaiseAndSetIfChanged(ref _roleText, outputText);
            }
        }

        private bool _isLoggedIn = false;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            private set
            {
                this.RaiseAndSetIfChanged(ref _isLoggedIn, value);
            }
        }

        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToLogin { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> LogOff { get; }

        public ProfileContentViewModel(IScreen screen)
        {
            HostScreen = screen;
            LoadUser();
            WelcomeUser = "";

            NavigateToLogin = ReactiveCommand.CreateFromObservable(
            () => HostScreen.Router.Navigate.Execute(new ProfileLoginContentViewModel(HostScreen))
        );

            // Создайте экземпляр команды и укажите делегат для выполнения
            LogOff = ReactiveCommand.CreateFromTask<Unit, IRoutableViewModel>(TryLogOff);
        }

        private async Task<IRoutableViewModel> TryLogOff(Unit _)
        {
            AccountService service = new AccountService();

            var result = await service.TryLogOff();

            switch (result.Result)
            {
                case AccountOutput.SUCCESS: User = result.UserData; break;
                case AccountOutput.ERROR: break;
                case AccountOutput.UNAUTHORIZED: break;
            }

            return null;
        }

        public async void LoadUser()
        {
            AccountService service = new AccountService();

            var result = await service.IsAuthentificated();

            switch (result.Result)
            {
                case AccountOutput.SUCCESS:
                    ClientData newUser = new ClientData
                    {
                        ClientCode = result.UserData.userID.ToObject<int>(), // Предполагая, что userID это int
                        Name = result.UserData.userName.ToString(), // Преобразование к строке
                        Roles = result.UserData.userRole.ToString(),
                    };

                    User = newUser;

                    IsLoggedIn = true;
                    WelcomeUser = "";
                    RoleText = "";
                    break;
                case AccountOutput.ERROR:
                    InteractiveContainer.ShowDialog(new TextBlock() { Text = "Произошла ошибка при выполнении запроса!", Margin = new Thickness(15, 8) });
                    break;
                case AccountOutput.UNAUTHORIZED:
                    IsLoggedIn = false;
                    break;
            }
        }
    }
}
