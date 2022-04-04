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
        public AddBoardPage()
        {
            InitializeComponent();
        }

        private void ButtonBackToMain_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}