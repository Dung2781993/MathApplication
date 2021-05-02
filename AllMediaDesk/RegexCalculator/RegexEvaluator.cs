using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AllMediaDesk.RegexCalculator
{
    public class RegexEvaluator
    {
        public delegate string Evaluator(Match match, string s);

        public RegexEvaluator(string pattern, Evaluator calculator, int priorityGroup)
        {
            Regex = new Regex(pattern, RegexOptions.Compiled);
            Evaluate = calculator;
            PriorityGroup = priorityGroup;
        }

        public int PriorityGroup { get; set; }
        public Regex Regex { get; set; }
        public Evaluator Evaluate { get; set; }
    }
}
