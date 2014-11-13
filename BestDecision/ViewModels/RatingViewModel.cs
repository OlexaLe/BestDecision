using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BestDecision.DataModels;
using BestDecision.Repository;

namespace BestDecision.ViewModels
{
    public class RatingViewModel
        : BaseViewModel
    {
        public Decision Decision { get; set; }

        public ObservableCollection<IGrouping<string, Rating>> Ratings { get; set; } 

        private readonly DecisionRepository _decisionRepository = new DecisionRepository();

        public async Task LoadDecisionAsync(int id)
        {
            var decisionTask = _decisionRepository.LoadDecisionAsync(id);
            var ratingsTask = _decisionRepository.LoadRatingsAsync(id);

            await Task.WhenAll(decisionTask, ratingsTask);

            Decision = decisionTask.Result;
            var ratings = ratingsTask.Result;
            foreach (var rating in ratings)
            {
                rating.RatingValueChanged = RatingValueChanged;
            }
            var ratingsGrouped = from act in ratingsTask.Result group act by act.CriteriaTitle into grp orderby grp.Key select grp;
            Ratings = new ObservableCollection<IGrouping<string, Rating>>(ratingsGrouped);
        }

        private async void RatingValueChanged(Rating rating)
        {
            await _decisionRepository.UpdateRating(rating);
        }
    }
}
