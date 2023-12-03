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
                if (value != null)
                {
                    if (value.Roles == "user")
                    {
                        IsLoggedIn = true;
                        IsLoggedInAsUser = true;
                        IsLoggedInAsAdmin = false;
                    }

                    if (value.Roles == "admin")
                    {
                        IsLoggedIn = true;
                        IsLoggedInAsUser = false;
                        IsLoggedInAsAdmin = true;
                    }
                }
                else
                {
                    IsLoggedIn = false;
                    IsLoggedInAsUser = false;
                    IsLoggedInAsAdmin = false;
                }
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

        private bool _isLoggedInAsAdmin = false;
        public bool IsLoggedInAsAdmin
        {
            get => _isLoggedInAsAdmin;
            private set
            {
                this.RaiseAndSetIfChanged(ref _isLoggedInAsAdmin, value);
            }
        }

        private bool _isLoggedInAsUser = false;
        public bool IsLoggedInAsUser
        {
            get => _isLoggedInAsUser;
            private set
            {
                this.RaiseAndSetIfChanged(ref _isLoggedInAsUser, value);
            }
        }

        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToLogin { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToPreparedPage { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> NavigateToOrderManagementPage { get; }
        public ReactiveCommand<Unit, IRoutableViewModel> LogOff { get; }

        public ProfileContentViewModel(IScreen screen)
        {
            HostScreen = screen;
            WelcomeUser = "";
            LoadUser();

            NavigateToLogin = ReactiveCommand.CreateFromObservable(
            () => HostScreen.Router.Navigate.Execute(new ProfileLoginContentViewModel(HostScreen))
        );

            NavigateToPreparedPage = ReactiveCommand.CreateFromObservable(
            () => HostScreen.Router.Navigate.Execute(new PreparedContentViewModel(HostScreen))
        );

            NavigateToOrderManagementPage = ReactiveCommand.CreateFromObservable(
         () => HostScreen.Router.Navigate.Execute(new OrderManagementContentViewModel(HostScreen))
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
                    break;
                case AccountOutput.ERROR:
                    InteractiveContainer.ShowDialog(new TextBlock() { Text = "Произошла ошибка при выполнении запроса!", Margin = new Thickness(15, 8) });
                    break;
                case AccountOutput.UNAUTHORIZED:
                    IsLoggedIn = false;
                    IsLoggedInAsAdmin = false;
                    break;
            }
        }
    }
}
