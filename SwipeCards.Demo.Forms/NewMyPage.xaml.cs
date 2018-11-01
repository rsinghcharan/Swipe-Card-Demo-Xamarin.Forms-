using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace SwipeCards.Demo.Forms
{
    public partial class NewMyPage : ContentPage
    {
        public NewMyPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }
        //void CardStackView_Swiped(object sender, Controls.Arguments.SwipedEventArgs e)
        //{

        //}

        void RestartButton_Clicked(object sender, System.EventArgs e)
        {
            CardStackView.Setup();
        }
    }
}
