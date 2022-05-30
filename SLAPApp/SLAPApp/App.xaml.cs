using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SLAPApp
{
    public partial class App : Application
    {
        /// <summary>
        /// Конструктор класса отвечающий за инициализацию приложения
        /// </summary>
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }
        /// <summary>
        /// Метод срабатывающий при старте приложения
        /// </summary>
        protected override void OnStart()
        {
        }
        /// <summary>
        /// Метод срабатывающий при закрытии приложения
        /// </summary>
        protected override void OnSleep()
        {
        }
        /// <summary>
        /// Метод срабатывающий при возобновлении работы с приложением
        /// </summary>
        protected override void OnResume()
        {
        }
    }
}
