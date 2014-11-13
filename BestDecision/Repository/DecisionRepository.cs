using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using BestDecision.DataModels;
using SQLite;

namespace BestDecision.Repository
{
    public class DecisionRepository
    {
        private SQLiteAsyncConnection _connection
        {
            get
            { return App.Connection; }
        }


        public Task<List<Decision>> LoadDecisionsAsync()
        {
            return _connection.Table<Decision>().ToListAsync();
        }

        public Task<Decision> LoadDecisionAsync(int id)
        {
            return _connection.GetAsync<Decision>(id);
        }

        public Task AddDecisionAsync(Decision decision)
        {
            return _connection.InsertAsync(decision);
        }

        public async Task DeleteDecisionAsync(Decision decision)
        {
            var criteriasTask = LoadCriteriasAsync(decision.ID);
            var variantsTask = LoadVariantsAsync(decision.ID);
            var ratingsTask = LoadRatingsAsync(decision.ID);

            await Task.WhenAll(criteriasTask, variantsTask, ratingsTask);

            var criteriaRemoveTask = DeleteCollection(criteriasTask.Result);
            var variantsRemoveTask = DeleteCollection(variantsTask.Result);
            var ratingsRemoveTask = DeleteCollection(ratingsTask.Result);
            await Task.WhenAll(criteriaRemoveTask, variantsRemoveTask, ratingsRemoveTask);

            await _connection.DeleteAsync(decision);
        }

        Task DeleteCollection<T>(IEnumerable<T> collection)
        {
            var tasks = collection.Select(item => _connection.DeleteAsync(item));
            return Task.WhenAll(tasks);
        }

        public Task<List<Criteria>> LoadCriteriasAsync(int decisionId)
        {
            return _connection.Table<Criteria>().Where(c => c.DecisionId == decisionId).ToListAsync();
        }

        public Task<List<Variant>> LoadVariantsAsync(int decisionId)
        {
            return _connection.Table<Variant>().Where(c => c.DecisionId == decisionId).ToListAsync();
        }

        public Task<List<Rating>> LoadRatingsAsync(int decisionId)
        {
            return _connection.Table<Rating>().Where(c => c.DecisionId == decisionId).ToListAsync();
        }

        public async Task<int> AddVariantAsync(Variant variant)
        {
            var variantId = await _connection.InsertAsync(variant);
            await MakeSureThatRatingPairExistForVariant(variant);
            return variantId;
        }

        public async Task RemoveVariantAsync(Variant variant)
        {
            await RemoveRatingsForVariant(variant);
            await _connection.DeleteAsync(variant);
        }

        public async Task<int> AddCriteriaAsync(Criteria criteria)
        {
            var criteriaId = await _connection.InsertAsync(criteria);
            await MakeSureThatRatingPairExistForCriteria(criteria);
            return criteriaId;
        }

        public async Task RemoveCriteriaAsync(Criteria criteria)
        {
            await RemoveRatingsForCriteria(criteria);
            await _connection.DeleteAsync(criteria);
        }

        public Task UpdateRating(Rating rating)
        {
            return _connection.UpdateAsync(rating);
        }

        async Task MakeSureThatRatingPairExistForCriteria(Criteria criteria)
        {
            var variants = await LoadVariantsAsync(criteria.DecisionId);
            var ratings = await LoadRatingsAsync(criteria.DecisionId);
            foreach (var variant in variants)
            {
                if (ratings.Any(r => r.CriteriaId == criteria.ID && r.VariantId == variant.ID)) continue;

                var rating = new Rating
                {
                    CriteriaId = criteria.ID,
                    VariantId = variant.ID,
                    DecisionId = criteria.DecisionId,
                    Value = 50,
                    CriteriaTitle = criteria.Title,
                    VariantTitle = variant.Title
                };
                await _connection.InsertAsync(rating);
            }
        }

        async Task MakeSureThatRatingPairExistForVariant(Variant variant)
        {
            var criterias = await LoadCriteriasAsync(variant.DecisionId);
            var ratings = await LoadRatingsAsync(variant.DecisionId);

            foreach (var criteria in criterias)
            {
                if (ratings.Any(r => r.CriteriaId == criteria.ID && r.VariantId == variant.ID)) continue;

                var rating = new Rating
                {
                    CriteriaId = criteria.ID,
                    VariantId = variant.ID,
                    DecisionId = criteria.DecisionId,
                    Value = 50,
                    CriteriaTitle = criteria.Title,
                    VariantTitle = variant.Title
                };
                await _connection.InsertAsync(rating); 
            }
        }

        async Task RemoveRatingsForCriteria(Criteria criteria)
        {
            var ratings = await LoadRatingsAsync(criteria.DecisionId);
            foreach (var rating in ratings.Where(rating => rating.CriteriaId == criteria.ID))
            {
                await _connection.DeleteAsync(rating);
            }
        }

        async Task RemoveRatingsForVariant(Variant varitant)
        {
            var ratings = await LoadRatingsAsync(varitant.DecisionId);
            foreach (var rating in ratings.Where(rating => rating.VariantId == varitant.ID))
            {
                await _connection.DeleteAsync(rating);
            }
        }

        
    }
}
