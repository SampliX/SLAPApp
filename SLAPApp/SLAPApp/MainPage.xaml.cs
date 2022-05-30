using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SLAPApp
{
    public partial class MainPage : ContentPage
    {
        public ICommand tapGesture => new Command(tapGestureA);
        public MainPage()
        {
            InitializeComponent();

            StartConfig();
        }

        private void tapGestureA()
        {
            DisplayAlert("Уведомление", "Пришло новое сообщение", "ОK");
        }

        private void ButtonHome_Clicked(object sender, EventArgs e)
        {
            
        }

        private void ButtonAdd_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new AddBoardPage());
        }

        private void ButtonCalendar_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CalendarPage());
        }

        private void StartRegistrationButton_Clicked(object sender, EventArgs e)
        {
            StartPageGrid.IsVisible = false;
            RegGrid.IsVisible = true;
        }

        private void StartAutentificationButton_Clicked(object sender, EventArgs e)
        {
            StartPageGrid.IsVisible = false;
            AuthGrid.IsVisible = true;
        }

        private void StartConfig()
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
                    if (User.hash_pass == App.Current.Properties["password"].ToString())
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

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
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

        private void LoginAccountButton_Clicked(object sender, EventArgs e)
        {
            if (AuthEmailEntry.Text != null && AuthPasswordEntry.Text != null)
            {
                WebClientClass clientClass = new WebClientClass();
                UserClass User;

                try
                {
                    User = clientClass.getUserData(AuthEmailEntry.Text);
                    if (User.hash_pass == CreateMD5(AuthPasswordEntry.Text))
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
                DisplayAlert("Ошибка", "Поля не заполнены", "Ок");
            }
        }

        private void ForgotPassowordButton_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Ошибка", "Пока не реализовано", "Ок");
        }

        private void EnterAccountButton_Clicked(object sender, EventArgs e)
        {
            StartPageGrid.IsVisible = false;
            AuthGrid.IsVisible = true;
            RegGrid.IsVisible = false;
        }

        private void RegisterAccountButton_Clicked(object sender, EventArgs e)
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
            catch(Exception ex)
            {
                DisplayAlert("Ошибка", ex.ToString(), "Ок");
            }
        }
    }
}
