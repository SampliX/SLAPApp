using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

//[assembly: Xamarin.Forms.Dependency(typeof(Toast_Android))]

namespace SLAPApp
{
    public partial class MainPage : ContentPage
    {
        public ICommand tapGesture => new Command(tapGestureA);

        /// <summary>
        /// Конструктор класса отвечающий за инициализацию начального окна
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            LoadStartConfig();
        }
        /// <summary>
        /// Внутренний метод для обработки события нажатия на desc
        /// </summary>
        public void tapGestureA()
        {
            DisplayAlert("Уведомление", "Пришло новое сообщение", "ОK");
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки домой
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void ButtonHome_Clicked(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки добавить доску
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void ButtonAdd_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AddBoardPage());
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки открыть календарь
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void ButtonCalendar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CalendarPage());
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки регистрации
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void StartRegistrationButton_Clicked(object sender, EventArgs e)
        {
            StartPageGrid.IsVisible = false;
            RegGrid.IsVisible = true;
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки начать авторизацию
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void StartAutentificationButton_Clicked(object sender, EventArgs e)
        {
            StartPageGrid.IsVisible = false;
            AuthGrid.IsVisible = true;
        }
        /// <summary>
        /// Метод отвечающий за загрузку стартовой конфигурации приложения
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void LoadStartConfig()
        {
            object email;
            object password;

            StartPageGrid.IsVisible = true;
            AuthGrid.IsVisible = false;
            RegGrid.IsVisible = false;
            MainPanelGrid.IsVisible = false;

            if (App.Current.Properties.TryGetValue("email", out email) &&
               App.Current.Properties.TryGetValue("password", out password))
            {
                WebClientClass clientClass = new WebClientClass();
                UserClass User;

                try
                {
                    User = clientClass.getUserData(email.ToString());
                    if (User.hash_pass == /*CreateMD5(App.Current.Properties["password"].ToString())*/ App.Current.Properties["password"].ToString())
                    {
                        MainRegAuthGrid.IsVisible = false;
                        MainPanelGrid.IsVisible = true;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    DisplayAlert("Ошибка", "Произошла непредвиденная ошибка, повторите попытку снова", "Ок");
                }
            }
            else if (App.Current.Properties.TryGetValue("email", out email))
            {
                StartPageGrid.IsVisible = false;
                AuthGrid.IsVisible = true;
                RegGrid.IsVisible = false;
            }
            else
            {
                StartPageGrid.IsVisible = true;
                AuthGrid.IsVisible = false;
                RegGrid.IsVisible = false;
            }
        }
        /// <summary>
        /// Метод отвечающий за хеширование строки алгоритмом MD5
        /// </summary>
        /// <param name="input">Строка, которую необходимо преобразовать</param>
        /// <returns>Возвращает захешированную строку</returns>
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Метод отвечающий за нажатие кнопки авторизоваться
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void LoginAccountButton_Clicked(object sender, EventArgs e)
        {
            if (AuthEmailEntry.Text != null && AuthPasswordEntry.Text != null)
            {
                WebClientClass clientClass = new WebClientClass();
                UserClass User;

                try
                {
                    User = clientClass.getUserData(AuthEmailEntry.Text);
                    if (User.hash_pass == /*CreateMD5(App.Current.Properties["password"].ToString())*/ AuthPasswordEntry.Text)
                    {
                        if(SwitchKeepLoggedMe.IsToggled == true)
                        {
                            App.Current.Properties.Add("email", AuthEmailEntry.Text);
                            App.Current.Properties.Add("password", AuthPasswordEntry.Text);
                        }
                        MainRegAuthGrid.IsVisible = false;
                        MainPanelGrid.IsVisible = true;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    DisplayAlert("Ошибка", "Произошла непредвиденная ошибка, повторите попытку снова", "Ок");
                }
            }
            else
            {
                DisplayAlert("Ошибка", "Не все поля заполнены", "Ок");
            }
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки забыл пароль
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void ForgotPassowordButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Ошибка", "Не реализованно", "Ок");
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки перейти в меню авторизации
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void EnterAccountButton_Clicked(object sender, EventArgs e)
        {
            StartPageGrid.IsVisible = false;
            AuthGrid.IsVisible = true;
            RegGrid.IsVisible = false;
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки зарегистрировать пользователя
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void RegisterAccountButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (RegEmailEntry.Text != null && RegPasswordEntry.Text != null && RegPasswordCopyEntry.Text != null)
                {
                    if (RegPasswordEntry.Text == RegPasswordCopyEntry.Text)
                    {
                        WebClientClass webClient = new WebClientClass();
                        webClient.postRegUserData(RegEmailEntry.Text, CreateMD5(RegPasswordEntry.Text));
                    }
                }
                else
                {
                    DisplayAlert("Ошибка", "Поля не заполнены", "Ок");
                }
            }
            catch
            {
                DisplayAlert("Ошибка", "Произошла непредвиденная ошибка, повторите попытку снова", "Ок");
            }
        }
    }
}
