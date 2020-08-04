using System;
using Xamarin.Forms;

namespace CG4U.Donate.ClientApp.Med.Views
{
    public partial class LoginPage : ContentPage
    {
        private ViewModels.LoginViewModel _viewModel;

        public LoginPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ViewModels.LoginViewModel();

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
