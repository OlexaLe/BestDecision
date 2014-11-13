using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace BestDecision.Converters
{
    class DecisionTitle : IValueConverter
    {
        private readonly string unnamedDecisionString = ResourceLoader.GetForCurrentView("Resources").GetString("UnnamedDecision");

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var title = (string)value;
            return string.IsNullOrWhiteSpace(title) ? unnamedDecisionString : title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
