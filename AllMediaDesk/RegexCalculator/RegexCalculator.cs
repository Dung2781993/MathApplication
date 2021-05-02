using AllMediaDesk.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AllMediaDesk.RegexCalculator
{
    class RegexCalculator : CalculatorBase
    {
        public static IEnumerable<RegexEvaluator> regexEvaluator = new List<RegexEvaluator>
        {
             new RegexEvaluator(@"\s*\(\s*(?<number>-?\d+\.?\d*)\s*\)\s*", RemoveBrackets, 3),
             new RegexEvaluator(@"\s*(?<![\s*/])\s*(?<numberA>-?\d+\.?\d*)\s*\+\s*(?<numberB>-?\d+\.?\d*)\s*(?![\s*/])\s*", Sum, 4),
             new RegexEvaluator(@"\s*(?<![\s*/])\s*(?<numberA>-?\d+\.?\d*)\s*\-\s*(?<numberB>-?\d+\.?\d*)\s*(?![\s*/])\s*", Sub, 4),
             new RegexEvaluator(@"\s*(?<numberA>-?\d+\.?\d*)\s*\*\s*(?<numberB>-?\d+\.?\d*)\s*", Mul, 5),
             new RegexEvaluator(@"\s*(?<numberA>-?\d+\.?\d*)\s*\/\s*(?<numberB>-?\d+\.?\d*)\s*", Div, 5),
             new RegexEvaluator(@"\s*(?<numberA>-?\d+\.?\d*)\s*\+\s*(?<numberB>-?\d+\.?\d*)\s*", Sum, 6),
             new RegexEvaluator(@"\s*(?<numberA>-?\d+\.?\d*)\s*\+\s*(?<numberB>-?\d+\.?\d*)\s*", Sub, 6)
        };

        public override Number Calculate(string expression)
        {
            bool change;
            do
            {
                change = false;
                foreach (var priorityGroup in regexEvaluator.Select(i => i.PriorityGroup).Distinct())
                {
                    var matches = new List<(Match match, RegexEvaluator regex)>();

                    foreach (var evaluator in regexEvaluator.Where(i=> i.PriorityGroup == priorityGroup))
                    {
                        var match = evaluator.Regex.Match(expression);
                        if (!match.Success)
                        {
                            continue;
                        }

                        matches.Add((match, evaluator));
                    }

                    if (matches.Any())
                    {
                        var leftMatch = matches.OrderBy(i => i.match.Index).First();

                        var nextStepExpression = leftMatch.regex.Evaluate(leftMatch.match, expression);
                        change = true;
                        OnEvaluationStage?.Invoke(leftMatch.match, expression, nextStepExpression);
                        expression = nextStepExpression;
                        break;
                    }
                }
            } while (change);
            return Number.Create(expression);
        }

        private static string MakeResult(Match match, string s, string result)
        {
            return s.Substring(0, match.Index) + result + s.Substring(match.Index + match.Length);
        }

        private static string RemoveBrackets(Match match, string s)
        {
            return MakeResult(match, s, match.Groups["number"].Value);
        }

        private static string Sum(Match match, string s)
        {
            var result = Number.Create(match.Groups["numberA"].Value) + Number.Create(match.Groups["numberB"].Value);
            return MakeResult(match, s, result.ToString());
        }

        private static string Sub(Match match, string s)
        {
            var result = Number.Create(match.Groups["numberA"].Value) - Number.Create(match.Groups["numberB"].Value);
            return MakeResult(match, s, result.ToString());
        }

        private static string Mul(Match match, string s)
        {
            var result = Number.Create(match.Groups["numberA"].Value) * Number.Create(match.Groups["numberB"].Value);
            return MakeResult(match, s, result.ToString());
        }

        private static string Div(Match match, string s)
        {
            var result = Number.Create(match.Groups["numberA"].Value) / Number.Create(match.Groups["numberB"].Value);
            return MakeResult(match, s, result.ToString());
        }
    }
}
