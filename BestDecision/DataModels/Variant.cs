using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestDecision.DataModels
{
    [Table("Variant")]
    public class Variant
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int DecisionId { get; set; }
        public string Title { get; set; }
    }
}
