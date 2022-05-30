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
        Button tmpButton = new Button();
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

        private void SelectColorButton_Clicked(object sender, EventArgs e)
        {
            tmpButton.BorderWidth = 0;
            tmpButton = sender as Button;
            tmpButton.BorderColor = Color.FromHex("#142453");
            tmpButton.BorderWidth = 3;
            deskFrame.BackgroundColor = tmpButton.BackgroundColor;
        }

        private void saveChangesButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();

            NavigationPage navPage = (NavigationPage)Application.Current.MainPage;
            IReadOnlyList<Page> navStack = navPage.Navigation.NavigationStack;
            MainPage homePage = navStack[navPage.Navigation.NavigationStack.Count - 1] as MainPage;

            if (homePage != null)
            {
                DisplayAlert("Уведомление", "Доска успешно создана", "ОK");
            }
        }
    }
}