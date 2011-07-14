using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ObjectFormatterConsole
{
    public static class TestObject
    {
        public static Dto Patrick
        {
            get
            {
                return new Dto
                {
                    DateCreated = DateTime.Now,
                    Array = new[] { "a", "b", "c" },
                    NestedCollection = new List<List<string>>
                        {
                            new List<string> { "My", "Very", "Educated", "Mother", "Just", "Sat", "Upon", "Nine", "Pies" },
                            new List<string> { "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune", "Pluto" }
                        },
                    Employee = new EmployeeDto
                    {
                        FirstName = "Douglas",
                        MiddleName = "Patrick",
                        LastName = "Caldwell",
                        Age = 28.9,
                        NickNames = new List<string> { "Stud Muffin", "Patty Cakes" },
                        Pals = new Dictionary<string, EmployeeDto>
                            {
                                { "Ryan", new EmployeeDto { FirstName = "Ryan" } },
                                { "Chris", new EmployeeDto { FirstName = "Chris" } }
                            },
                        BodyParts = new BodyDto
                            {
                                { "Eyes", "Two Eyes" },
                                { "Ears", "Two Ears" },
                                { "Nose", "Very Much So" }
                            }
                    }
                };
            }
        }
    }
    
    public struct Dto
    {
        public EmployeeDto Employee { get; set; }
        public DateTime DateCreated { get; set; }
        public List<List<string>> NestedCollection { get; set; }
        public string[] Array { get; set; }
    }

    public struct EmployeeDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public double Age { get; set; }
        public List<string> NickNames { get; set; }
        public Dictionary<string, EmployeeDto> Pals { get; set; }
        public BodyDto BodyParts { get; set; }
    }

    public class BodyDto : Dictionary<string, string>
    {
        public string this[int position] { get { return this.ElementAt(position).Value; } set { this[this.ElementAt(position).Key] = value; } }
        public string this[int position, string checkValue]
        {
            get
            {
                var value = this[position];
                return value == checkValue ? "A Good Match" : "Not A Match";
            }
        }
    }
}
