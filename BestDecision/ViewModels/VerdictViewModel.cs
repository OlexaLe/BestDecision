using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BestDecision.DataModels;
using BestDecision.Repository;

namespace BestDecision.ViewModels
{
    public class VerdictViewModel
        : BaseViewModel
    {
        public Decision Decision { get; set; }

        private readonly DecisionRepository _decisionRepository = new DecisionRepository();

        private string _bestDecision;
        public string BestDecision
        {
            get { return _bestDecision; }
            set { SetProperty(ref _bestDecision, value); }
        }

        public int BestDecisionPercentage { get; set; }

        public ObservableCollection<Verdict> Verdicts { get; set; }
        
        public async Task LoadDecisionAsync(int id)
        {
            var decisionTask = _decisionRepository.LoadDecisionAsync(id);
            var criteariasTask = _decisionRepository.LoadCriteriasAsync(id);
            var ratingsTask = _decisionRepository.LoadRatingsAsync(id);

            await Task.WhenAll(decisionTask, ratingsTask, criteariasTask);

            Decision = decisionTask.Result;
            var criterias = criteariasTask.Result;

            var verdicts = new List<Verdict>();
            foreach (var rating in ratingsTask.Result)
            {
                var isNewVerdict = false;
                var verdict = verdicts.FirstOrDefault(v => v.VariantId == rating.VariantId);
                if (verdict == null)
                {
                    isNewVerdict = true;
                    verdict = new Verdict
                    {
                        VariantId = rating.VariantId,
                        Value = 0,
                        VariantTitle = rating.VariantTitle
                    };
                }
                var criteriaWeight = criterias.First(c => c.ID == rating.CriteriaId).Weight;
                verdict.Value += rating.Value * criteriaWeight;

                if (isNewVerdict)
                    verdicts.Add(verdict);
            }

            verdicts = verdicts.OrderByDescending(v => v.Value).ToList();
            var maximumValue = criterias.Sum(criteria => criteria.Weight * 100);
            foreach (var verdict in verdicts)
            {
                verdict.Value = (int)((double)verdict.Value / maximumValue * 100);
            }

            BestDecision = verdicts[0].VariantTitle;
            BestDecisionPercentage = verdicts[0].Value;

            Verdicts = new ObservableCollection<Verdict>(verdicts.GetRange(1, verdicts.Count - 1));
        }
    }
}
