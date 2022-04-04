using System;
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
        public MainPage()
        {
            InitializeComponent();
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

        }
    }
}
