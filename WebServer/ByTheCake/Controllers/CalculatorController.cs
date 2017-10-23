namespace WebServer.ByTheCake.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure;
    using WebServer.Server.HTTP.Contracts;

    public class CalculatorController : BaseController
    {
        private string[] allowedmathematicalSigns = new string[] { "+", "-", "*", "/" };
        private const string FirstValue = "first";
        private const string SecondValue = "second";
        private const string MathematicalSign = "mathematicalSign";

        public IHttpResponse Calculate(IDictionary<string, string> queryParameters)
        {
            string result = string.Empty;
            double calc = 0;
            if (queryParameters.ContainsKey(FirstValue) 
                && queryParameters.ContainsKey(SecondValue) 
                && queryParameters.ContainsKey(MathematicalSign))
            {
                var mathSign = queryParameters.FirstOrDefault(k => k.Key == MathematicalSign).Value;
                
                if (this.allowedmathematicalSigns.Contains(mathSign) && queryParameters.Values.Count == 3)
                {
                    var firstValue = double.Parse(queryParameters.FirstOrDefault(k => k.Key == FirstValue).Value);
                    var secondValue = double.Parse(queryParameters.FirstOrDefault(k => k.Key == SecondValue).Value);
                    if (mathSign == "+")
                    {
                        calc = firstValue + secondValue;
                    }

                    else if (mathSign == "-")
                    {
                        calc = firstValue - secondValue;
                    }

                    else if (mathSign == "/")
                    {
                        calc = firstValue / secondValue;
                    }

                    else
                    {
                        calc = firstValue * secondValue;
                    }
                    result = $"<div>Result: {calc}</div>";
                }
                else
                {
                    result = @"<strong>Invalid Sign</strong>";
                }
            }

            this.ViewData["calculate"] = result;

            return this.FileViewResponse(@"Calculator\calculator");
        }
    }
}