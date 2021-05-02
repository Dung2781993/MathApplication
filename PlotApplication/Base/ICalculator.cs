using PlotApplication.RegexCalculator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PlotApplication.Base
{
    public delegate void EvaluationStage(Match match, string before, string after);
    public interface ICalculator
    {
        Number Calculate(string expression);
        EvaluationStage OnEvaluationStage { get; set; }
    }
}
