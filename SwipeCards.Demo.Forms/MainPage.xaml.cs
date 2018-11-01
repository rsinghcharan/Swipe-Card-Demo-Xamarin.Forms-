using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace SwipeCards.Demo.Forms
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
        void RestartButton_Clicked(object sender, System.EventArgs e)
        {
            CardStackView.Setup();
        }
    }
}
