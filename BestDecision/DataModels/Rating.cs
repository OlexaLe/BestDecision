using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BestDecision.DataModels
{
    [Table("Rating")]
    public class Rating
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int DecisionId { get; set; }
        public int CriteriaId { get; set; }
        public int VariantId { get; set; }

        public int Value
        {
            get { return _value; }
            set { _value = value;
                var handler = RatingValueChanged;
                if (handler != null)
                    handler(this);
            }
        }

        public string VariantTitle { get; set; }
        public string CriteriaTitle { get; set; }

        public Action<Rating> RatingValueChanged;
        private int _value;
    }
}
