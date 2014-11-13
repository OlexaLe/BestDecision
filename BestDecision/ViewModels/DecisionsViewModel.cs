using BestDecision.Common;
using BestDecision.DataModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BestDecision.Repository;

namespace BestDecision.ViewModels
{
    public class DecisionsViewModel : BaseViewModel
    {
        public ObservableCollection<Decision> Decisions { get; set; }
        private readonly DecisionRepository _decisionRepository = new DecisionRepository();

        public async Task LoadDecisionsAsync ()
        {
            var storedDecisions = await _decisionRepository.LoadDecisionsAsync();
            Decisions = new ObservableCollection<Decision>(storedDecisions);
        }

        public async Task<int> SaveDecisionAsync(Decision decision)
        {
            Decisions.Add(decision);
            await _decisionRepository.AddDecisionAsync(decision);
            return decision.ID;
        }

        private ICommand _deleteDecision;
        public ICommand DeleteDecision
        {
            get
            {
                return _deleteDecision ?? (_deleteDecision = new RelayCommand<Decision>(DoDeleteDecision));
            }
        }

        private async void DoDeleteDecision(Decision decision)
        {
            Decisions.Remove(decision);
            await _decisionRepository.DeleteDecisionAsync(decision);
        }
    }
}
