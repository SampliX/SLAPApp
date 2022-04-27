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
    public partial class CalendarPage : ContentPage
    {
        /// <summary>
        /// Конструктор класса отвечающий за инициализацию страницы с календарем
        /// </summary>
        public CalendarPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки вернуться в главное меню
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        public void ButtonBackToMain_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}