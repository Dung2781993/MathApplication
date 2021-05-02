using PlotApplication.RegexCalculator;
using System;
using System.Collections.Generic;
using System.Text;
using static PlotApplication.Base.ICalculator;

namespace PlotApplication.Base
{
    public abstract class CalculatorBase : ICalculator
    {
        public abstract Number Calculate(string expression);
        public EvaluationStage OnEvaluationStage { get; set; }
    }
}
