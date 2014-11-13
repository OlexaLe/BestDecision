using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestDecision.DataModels
{
    public class Verdict
    {
        public int VariantId { get; set; }
        public string VariantTitle { get; set; }
        public int Value { get; set; }
        //public int Rating { get; set; }
        public int CriteriaWeight { get; set; }
    }
}
