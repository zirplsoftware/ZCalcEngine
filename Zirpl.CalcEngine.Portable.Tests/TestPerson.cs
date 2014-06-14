using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zirpl.CalcEngine.Portable.Tests
{
    public class TestPerson
    {
        public TestPerson()
        {
            Children = new List<TestPerson>();
            ChildrenDct = new Dictionary<string, TestPerson>();
        }
        public string Name { get; set; }
        public bool Male { get; set; }
        public DateTime Birth { get; set; }
        public List<TestPerson> Children { get; private set; }
        public Dictionary<string, TestPerson> ChildrenDct { get; private set; }
        public int Age { get { return DateTime.Today.Year - Birth.Year; } }

        public static TestPerson CreateTestPerson()
        {
            var p = new TestPerson();
            p.Name = "Test Person";
            p.Birth = DateTime.Today.AddYears(-30);
            p.Male = true;
            for (int i = 0; i < 5; i++)
            {
                var c = new TestPerson();
                c.Name = "Test Child " + i.ToString();
                c.Birth = DateTime.Today.AddYears(-i);
                c.Male = i % 2 == 0;
                p.Children.Add(c);
                p.ChildrenDct.Add(c.Name, c);
            }
            return p;
        }
    }
}
