using Windows.UI.Xaml.Controls;
using BestDecision.ViewModels;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace BestDecision.Dialogs
{
    public sealed partial class NewDecisionDialog : ContentDialog
    {
        DecisionsViewModel decisionsViewModel;

        public NewDecisionDialog(DecisionsViewModel decisionsViewModel)
        {
            this.InitializeComponent();
            this.decisionsViewModel = decisionsViewModel;
            
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            await decisionsViewModel.SaveDecisionAsync(new DataModels.Decision { Title = DecisionTitle.Text });
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
