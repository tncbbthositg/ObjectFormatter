using System;
using System.Collections.Generic;
using System.Linq;
using ObjectFormatting;

namespace ObjectFormatterConsole
{
    class FormatterDemo
    {
        static void Main()
        {
            var patrick = TestObject.Patrick;

            Console.WriteLine(ObjectFormatter.TokenFormat(
@"Mr. {Employee.FirstName}: is {{{{{Employee.Age:0.00}}}}} in {{years}}.  
{Employee.FirstName} has {Employee.NickNames.Count} nicknames and the first one is {Employee.NickNames[0]}.
He even has {Employee.Pals.Count} friends and his favorite is {Employee.Pals[""Ryan""].FirstName}.
Patrick has {Employee.BodyParts[""Ears""]} and {Employee.BodyParts[0]}.
Patrick has Two Eyes: {Employee.BodyParts[0, ""Two Eyes""]}.
Patrick has A Small Nose: {Employee.BodyParts[0, ""A Small Nose""]}.
The planet {{{NestedCollection[1][2]}}} is represented by {{{NestedCollection[0][2]}}}.
Date Created: {DateCreated:d/M/yyyy HH:mm:ss}
Date Created: {DateCreated:MM/dd/yyyy}
Date Created: {DateCreated:dddd, MMMM d, yyyy}
{Array[1]}
My Initials Are: {Employee.FirstName[0]}. {Employee.MiddleName[0]}. {Employee.LastName[0]}.
", patrick));

            Console.ReadLine();
        }
    }
}
