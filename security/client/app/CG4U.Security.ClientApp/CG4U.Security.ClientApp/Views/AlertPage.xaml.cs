using System;
using System.Collections.Generic;
using CG4U.Security.ClientApp.Models;
using CG4U.Security.ClientApp.ViewModels;
using Plugin.Badge.Abstractions;
using Xamarin.Forms;

namespace CG4U.Security.ClientApp.Views
{
    public partial class AlertPage : ContentPage
    {
        private AlertViewModel _viewModel;

        public AlertPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new AlertViewModel();

            var itens = new List<Alert>()
            {
                new Alert() { Id = 1, Message="Você possui uma nova mensagem no Chat.", Type=AlertType.Warning, ProcessingMethod=AlertProcessingMethod.SceneChange, IdImageProcesses=1 },
                new Alert() { Id = 2, Message="Mudança de cena detectada, verifique.", Type=AlertType.Critical, ProcessingMethod=AlertProcessingMethod.SceneChange, IdImageProcesses=2 },
                new Alert() { Id = 3, Message="Carro desconhecido detectado, verifique.", Type=AlertType.Critical, ProcessingMethod=AlertProcessingMethod.UnkownCar, IdImageProcesses=3 },
                new Alert() { Id = 4, Message="Pessoa desconhecida detectada, verifique com urgência!", Type=AlertType.Panic, ProcessingMethod=AlertProcessingMethod.UnkownPeople, IdImageProcesses=4 }
            };
            _viewModel.Alerts.AddRange(itens);
            _viewModel.Alerts.CollectionChanged += (sender, e) =>
            {
                var target = _viewModel.Alerts[_viewModel.Alerts.Count - 1];
                AlertListView.ScrollTo(target, ScrollToPosition.End, true);
            };

        }
    }
}
