using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Zirpl.CalcEngine.Portable.Tests
{
    [TestFixture]
    public class CalculationEngineTests
    {
        [Test]
        public void Test()
        {
            CalculationEngine engine = new CalculationEngine();

            // adjust culture
            var cultureInfo = engine.CultureInfo;
            engine.CultureInfo = System.Globalization.CultureInfo.InvariantCulture;

	        // test internal operators
            engine.Test("0", 0.0);
            engine.Test("+1", 1.0);
            engine.Test("-1", -1.0);
            engine.Test("1+1", 1 + 1.0);
            engine.Test("1*2*3*4*5*6*7*8*9", 1 * 2 * 3 * 4 * 5 * 6 * 7 * 8 * 9.0);
            engine.Test("1/(1+1/(1+1/(1+1/(1+1/(1+1/(1+1/(1+1/(1+1/(1+1/(1+1))))))))))", 1 / (1 + 1 / (1 + 1 / (1 + 1 / (1 + 1 / (1 + 1 / (1 + 1 / (1 + 1 / (1 + 1 / (1 + 1 / (1 + 1.0)))))))))));
            engine.Test("((1+2)*(2+3)/(4+5))^0.123", Math.Pow((1 + 2) * (2 + 3) / (4 + 5.0), 0.123));
            engine.Test("10%", 0.1);
            engine.Test("1e+3", 1000.0);

            // test simple variables
            engine.Variables.Add("one", 1);
            engine.Variables.Add("two", 2);
            engine.Test("one + two", 3);
            engine.Test("(two + two)^2", 16);
            engine.Variables.Clear();

            // test DataContext
            var dc = engine.DataContext;
            var p = TestPerson.CreateTestPerson();
			p.Parent = TestPerson.CreateTestPerson();
			engine.DataContext = p;
            engine.Test("Name", "Test Person");
			engine.Test("Parent.Name", "Test Person");
			engine.Test("Name.Length * 2", p.Name.Length * 2);
            engine.Test("Children.Count", p.Children.Count);
            engine.Test("Children(2).Name", p.Children[2].Name);
            engine.Test("ChildrenDct(\"Test Child 2\").Name", p.ChildrenDct["Test Child 2"].Name);
			engine.Test("ChildrenDct('Test Child 2').Name", p.ChildrenDct["Test Child 2"].Name);
			engine.Test("ChildrenIdDct('16C5888C-6C75-43DD-A372-2A3398DAE038').Name", p.ChildrenDct["Test Child 1"].Name);
			engine.Test("ChildrenDct.Count", p.ChildrenDct.Count);
            engine.DataContext = dc;

            // test functions


            // COMPARE TESTS
            engine.Test("5=5"   , true);
            engine.Test("5==5"  , true);
            engine.Test("6==5"  , false);
            engine.Test("6=5"   , false);
            engine.Test("6<5"   , false);
            engine.Test("6>5"   , true);
            engine.Test("5<=10" , true);
            engine.Test("5>=3"  , true);

            // LOGICAL FUNCTION TESTS

            engine.Test("5<=6.0 && 6>=3"  , true);
            engine.Test("true  && true"   , true);
            engine.Test("true  && false"  , false);
            engine.Test("false && true"   , false);
            engine.Test("false && false"  , false);
            engine.Test("true  || true"   , true);
            engine.Test("true  || false"  , true);
            engine.Test("false || true"   , true);
            engine.Test("false || false"  , false);

            engine.Test("AND(true, true)", true);
            engine.Test("AND(true, false)", false);
            engine.Test("AND(false, true)", false);
            engine.Test("AND(false, false)", false);
            engine.Test("OR(true, true)", true);
            engine.Test("OR(true, false)", true);
            engine.Test("OR(false, true)", true);
            engine.Test("OR(false, false)", false);
            engine.Test("NOT(false)", true);
            engine.Test("NOT(true)", false);
            engine.Test("IF(5 > 4, true, false)", true);
            engine.Test("IF(5 > 14, true, false)", false);
            engine.Test("TRUE()", true);
            engine.Test("FALSE()", false);

            // MATH/TRIG FUNCTION TESTS
            engine.Test("ABS(-12)", 12.0);
            engine.Test("ABS(+12)", 12.0);
            engine.Test("ACOS(.23)", Math.Acos(.23));
            engine.Test("ASIN(.23)", Math.Asin(.23));
            engine.Test("ATAN(.23)", Math.Atan(.23));
            engine.Test("ATAN2(1,2)", Math.Atan2(1, 2));
            engine.Test("CEILING(1.8)", Math.Ceiling(1.8));
            engine.Test("COS(1.23)", Math.Cos(1.23));
            engine.Test("COSH(1.23)", Math.Cosh(1.23));
            engine.Test("EXP(1)", Math.Exp(1));
            engine.Test("FLOOR(1.8)", Math.Floor(1.8));
            engine.Test("INT(1.8)", 1);
            engine.Test("LOG(1.8)", Math.Log(1.8, 10)); // default base is 10
            engine.Test("LOG(1.8, 4)", Math.Log(1.8, 4)); // custom base
            engine.Test("LN(1.8)", Math.Log(1.8)); // real log
            engine.Test("LOG10(1.8)", Math.Log10(1.8)); // same as Log(1.8)
            engine.Test("PI()", Math.PI);
            engine.Test("POWER(2,4)", Math.Pow(2, 4));
            //engine.Test("RAND") <= 1.0);
            //engine.Test("RANDBETWEEN(4,5)") <= 5);
            engine.Test("SIGN(-5)", -1);
            engine.Test("SIGN(+5)", +1);
            engine.Test("SIGN(0)", 0);
            engine.Test("SIN(1.23)", Math.Sin(1.23));
            engine.Test("SINH(1.23)", Math.Sinh(1.23));
            engine.Test("SQRT(144)", Math.Sqrt(144));
            engine.Test("SUM(1, 2, 3, 4)", 1 + 2 + 3 + 4.0);
            engine.Test("TAN(1.23)", Math.Tan(1.23));
            engine.Test("TANH(1.23)", Math.Tanh(1.23));
            engine.Test("TRUNC(1.23)", 1.0);
            engine.Test("PI()", Math.PI);
            engine.Test("PI", Math.PI);
            engine.Test("LN(10)", Math.Log(10));
            engine.Test("LOG(10)", Math.Log10(10));
            engine.Test("EXP(10)", Math.Exp(10));
            engine.Test("SIN(PI()/4)", Math.Sin(Math.PI / 4));
            engine.Test("ASIN(PI()/4)", Math.Asin(Math.PI / 4));
            engine.Test("SINH(PI()/4)", Math.Sinh(Math.PI / 4));
            engine.Test("COS(PI()/4)", Math.Cos(Math.PI / 4));
            engine.Test("ACOS(PI()/4)", Math.Acos(Math.PI / 4));
            engine.Test("COSH(PI()/4)", Math.Cosh(Math.PI / 4));
            engine.Test("TAN(PI()/4)", Math.Tan(Math.PI / 4));
            engine.Test("ATAN(PI()/4)", Math.Atan(Math.PI / 4));
            engine.Test("ATAN2(1,2)", Math.Atan2(1, 2));
            engine.Test("TANH(PI()/4)", Math.Tanh(Math.PI / 4));


            // TEXT FUNCTION TESTS
            engine.Test("CHAR(65)", "A");
            engine.Test("CODE(\"A\")", 65);
            engine.Test("CONCATENATE(\"a\", \"b\")", "ab");
			engine.Test("CONCATENATE('a', 'b')", "ab");
			engine.Test("FIND(\"bra\", \"abracadabra\")", 2);
            engine.Test("FIND(\"BRA\", \"abracadabra\")", -1);
            engine.Test("LEFT(\"abracadabra\", 3)", "abr");
            engine.Test("LEFT(\"abracadabra\")", "a");
            engine.Test("LEN(\"abracadabra\")", 11);
            engine.Test("LOWER(\"ABRACADABRA\")", "abracadabra");
            engine.Test("MID(\"abracadabra\", 1, 3)", "abr");
            engine.Test("PROPER(\"abracadabra\")", "Abracadabra");
            engine.Test("REPLACE(\"abracadabra\", 1, 3, \"XYZ\")", "XYZacadabra");
            engine.Test("REPT(\"abr\", 3)", "abrabrabr");
            engine.Test("RIGHT(\"abracadabra\", 3)", "bra");
            engine.Test("SEARCH(\"bra\", \"abracadabra\")", 2);
            engine.Test("SEARCH(\"BRA\", \"abracadabra\")", 2);
            engine.Test("SUBSTITUTE(\"abracadabra\", \"a\", \"b\")", "bbrbcbdbbrb");
            engine.Test("T(123)", "123");
            engine.Test("TEXT(1234, \"n2\")", "1,234.00");
            engine.Test("TRIM(\"   hello   \")", "hello");
            engine.Test("UPPER(\"abracadabra\")", "ABRACADABRA");
            engine.Test("VALUE(\"1234\")", 1234.0);

            engine.Test("SUBSTITUTE(\"abcabcabc\", \"a\", \"b\")", "bbcbbcbbc");
            engine.Test("SUBSTITUTE(\"abcabcabc\", \"a\", \"b\", 1)", "bbcabcabc");
            engine.Test("SUBSTITUTE(\"abcabcabc\", \"a\", \"b\", 2)", "abcbbcabc");
            engine.Test("SUBSTITUTE(\"abcabcabc\", \"A\", \"b\", 2)", "abcabcabc"); // case-sensitive!

            // test taken from http://www.codeproject.com/KB/vb/FormulaEngine.aspx
            var a1 = "\"http://j-walk.com/ss/books\"";
            var exp = "RIGHT(A1,LEN(A1)-FIND(CHAR(1),SUBSTITUTE(A1,\"/\",CHAR(1),LEN(A1)-LEN(SUBSTITUTE(A1,\"/\",\"\")))))";
            engine.Test(exp.Replace("A1", a1), "books");



            // STATISTICAL FUNCTION TESTS
            engine.Test("Average(1, 3, 3, 1, true, false, \"hello\")", 2.0);
            engine.Test("AverageA(1, 3, 3, 1, true, false, \"hello\")", (1 + 3 + 3 + 1 + 1 + 0 + 0) / 7.0);
            engine.Test("Count(1, 3, 3, 1, true, false, \"hello\")", 4.0);
            engine.Test("CountA(1, 3, 3, 1, true, false, \"hello\")", 7.0);

            // restore culture
            engine.CultureInfo = cultureInfo;
        }
    }
}
