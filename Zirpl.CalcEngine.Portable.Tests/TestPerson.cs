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
        }
        public string Name { get; set; }
        public bool Male { get; set; }
        public DateTime Birth { get; set; }
	    public Address Address { get; set; }
	    public TestPerson Parent { get; set; }
        public List<TestPerson> Children { get; set; }

	    public Dictionary<string, TestPerson> ChildrenDct
	    {
		    get
		    {
			    return Children.ToDictionary(person => person.Name, person => person);
			}
	    }

        public Dictionary<string, TestPerson> ChildrenIdDct
		{
			get
			{
				return Children.ToDictionary(person => person.Id.ToString().ToUpperInvariant(), person => person);
			}
		}

		public int Age { get { return DateTime.Today.Year - Birth.Year; } }

        public static TestPerson CreateTestPerson()
        {
            var p = new TestPerson();
            p.Name = "Test Person";
			p.Code = "Code";
			p.Id = Guid.Parse("96C5888C-6C75-43DD-A372-2A3398DAE038");
			p.Birth = DateTime.Today.AddYears(-30);
            p.Male = true;
			for (int i = 0; i < 5; i++)
			{
				var c = new TestPerson();
				c.Name = "Test Child " + i.ToString();
				c.Id = Guid.Parse(i + "6C5888C-6C75-43DD-A372-2A3398DAE038");
				c.Birth = DateTime.Today.AddYears(-i);
				c.Male = i % 2 == 0;
				p.Children.Add(c);
			}
			return p;
        }

	    public string Code { get; set; }

	    public Guid Id { get; set; }
    }

	public class Address
	{
		public string Street { get; set; }
	}
}
