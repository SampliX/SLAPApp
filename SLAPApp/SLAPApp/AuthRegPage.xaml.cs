using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLAPApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthRegPage : ContentPage
    {
        public AuthRegPage()
        {
            InitializeComponent();

            StartConfig();
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

            //App.Current.Properties.Add("email", "one");
            //App.Current.Properties.Add("password", "one");

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
                        //Navigation.PushModalAsync(new MainPage());
                        //Navigation.RemovePage(this);
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
            else if(App.Current.Properties.TryGetValue("email", out email))
            {
                StartPageGrid.IsVisible = false;
                AuthGrid.IsVisible = true;
                RegGrid.IsVisible = false;
            }
            else
            {
                StartPageGrid.IsVisible = false;
                AuthGrid.IsVisible = false;
                RegGrid.IsVisible = true;
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
    }
}