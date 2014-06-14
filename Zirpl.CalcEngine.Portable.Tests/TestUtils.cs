using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.CalcEngine.Portable.Tests
{
    public static class TestUtils
    {
        public static void Test(this CalculationEngine engine, string expression, object expectedResult)
        {
                var result = engine.Evaluate(expression);
                if (result is double && expectedResult is int)
                {
                    expectedResult = (double)(int)expectedResult;
                }
                if (!object.Equals(result, expectedResult))
                {
                    var msg = string.Format("error: {0} gives {1}, should give {2}", expression, result, expectedResult);
                    throw new Exception(msg);
                }
        }
    }
}
