using BestDecision.DataModels;
using BestDecision.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace BestDecision.Dialogs
{
    public sealed partial class CriteriaDialog : ContentDialog
    {
        DecisionDetailsViewModel viewModel;

        public CriteriaDialog(DecisionDetailsViewModel viewModel)
        {
            this.InitializeComponent();
            this.viewModel = viewModel;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var criteria = new Criteria { DecisionId = viewModel.Decision.ID, Title = name.Text, Weight = (int)weight.Value };
            await viewModel.AddCriteria(criteria);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
