using System;
using System.Collections.Generic;
using CG4U.Donate.ClientApp.Med.Models;
using Xamarin.Forms;

namespace CG4U.Donate.ClientApp.Med.Views
{
    public partial class DesiredPage : ContentPage
    {
        public DesiredPage(Donation donation)
        {
            InitializeComponent();
        }

        void OnMaxGetSliderChanged(Object sender, EventArgs e)
        {
            spanKM.Text = sliderMaxGet.Value.ToString("#.##") + " Kilometros";
        }
    }
}
