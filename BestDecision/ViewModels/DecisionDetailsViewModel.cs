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
    public class DecisionDetailsViewModel : BaseViewModel
    {
        public Decision Decision { get; set; }

        public ObservableCollection<Criteria> Criterias { get; set; }

        public ObservableCollection<Variant> Variants { get; set; }

        public bool UserCanProceed
        {
            get { return Criterias.Count > 0 && Variants.Count > 1; }
        }

        private readonly DecisionRepository _decisionRepository = new DecisionRepository();

        public async Task LoadDecisionAsync(int id)
        {
            var decisionTask = _decisionRepository.LoadDecisionAsync(id);
            var criteariasTask = _decisionRepository.LoadCriteriasAsync(id);
            var variantsTask = _decisionRepository.LoadVariantsAsync(id);

            await Task.WhenAll(decisionTask, criteariasTask, variantsTask);

            Decision = decisionTask.Result;
            Criterias = new ObservableCollection<Criteria>(criteariasTask.Result);
            Variants = new ObservableCollection<Variant>(variantsTask.Result);
        }

        public async Task AddVariant (Variant variant)
        {
            var optionId = await _decisionRepository.AddVariantAsync(variant);
            variant.ID = optionId;
            Variants.Add(variant);
            OnPropertyChanged("UserCanProceed");
        }

        public async Task AddCriteria(Criteria criteria)
        {
            var criteriaId = await _decisionRepository.AddCriteriaAsync(criteria);
            criteria.ID = criteriaId;
            Criterias.Add(criteria);
            OnPropertyChanged("UserCanProceed");
        }

        private ICommand removeItem;
        public ICommand RemoveItem
        {
            get
            {
                return removeItem ?? (removeItem = new RelayCommand<object>(DoRemoveItem));
            }
        }

        private async void DoRemoveItem(object item)
        {
            if (item is Criteria)
                await RemoveCriteria(item as Criteria);
            else if (item is Variant)
                await RemoveVariant(item as Variant);
            else
                throw new ArgumentException();
            OnPropertyChanged("UserCanProceed");
        }

        private Task RemoveCriteria(Criteria criteria)
        {
            Criterias.Remove(criteria);
            return _decisionRepository.RemoveCriteriaAsync(criteria);
        }

        private Task RemoveVariant(Variant variant)
        {
            Variants.Remove(variant);
            return _decisionRepository.RemoveVariantAsync(variant);
        }
    }
}
