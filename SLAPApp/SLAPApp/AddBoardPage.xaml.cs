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
    public partial class AddBoardPage : ContentPage
    {
        /// <summary>
        /// Конструктор класса отвечающий за инициализацию страницы с созданием новых досок
        /// </summary>
        public AddBoardPage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Метод отвечающий за нажатие кнопки вернуться в главное меню
        /// </summary>
        /// <param name="sender">Параметр, который содержит ссылку на объект, который вызвал событие</param>
        /// <param name="e">Содержит дополнительную информацию о вызываемом событии</param>
        private void ButtonBackToMain_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}