using System;
using System.Collections.Generic;
using CG4U.Security.ClientApp.ViewModels;
using Xamarin.Forms;

namespace CG4U.Security.ClientApp.Views
{
    public partial class LoginPage : ContentPage
    {
        private LoginViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new LoginViewModel();

            Email.Completed += (object sender, EventArgs e) =>
            {
                Password.Focus();
            };

            Password.Completed += (object sender, EventArgs e) =>
            {
                buttonLogin.IsEnabled = false;
                _viewModel.LoginCommand.Execute(null);
                buttonLogin.IsEnabled = true;
            };

            //TODO: Remove these 2 lines!
            Email.Text = "danieldrb@hotmail.com";
            Password.Text = "123";
        }

        private void OnButtonLoginCommand(Object sender, EventArgs e)
        {
            buttonLogin.IsEnabled = false;
            _viewModel.LoginCommand.Execute(null);
            buttonLogin.IsEnabled = true;
        }

        private void OnNotHaveAccontTapped(Object sender, EventArgs e)
        {
            //TODO: Handle that!
        }
    }
}
