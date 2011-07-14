using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectFormatting;

namespace ObjectFormatterTests
{
    [TestClass]
    public class ObjectFormatterUnitTest
    {
        [TestMethod]
        public void CanOutputConstant()
        {
            Assert.AreEqual("Test Formatting", ObjectFormatter.TokenFormat("Test Formatting", null));
        }

        [TestMethod]
        public void CanOutputBasicFormat()
        {
            var helloWorld = "Hello World";
            var date = new DateTime(1979, 4, 6);

            Assert.AreEqual(string.Format("{0}", helloWorld), ObjectFormatter.TokenFormat("{Value}", new { Value = helloWorld }));
            Assert.AreEqual(string.Format("{0}", date), ObjectFormatter.TokenFormat("{Value}", new { Value = date }));
            Assert.AreEqual(string.Format("{0} as of {1}", helloWorld, date), ObjectFormatter.TokenFormat("{text} as of {date}", new { text = helloWorld, date = date }));
        }

        [TestMethod]
        public void CanUseStringFormatSpecifiers()
        {
            var date = new DateTime(1979, 4, 6);
            var guid = Guid.NewGuid();

            var testObject = new { Date = date, Guid = guid };

            Assert.AreEqual(string.Format("{0:d/M/yyyy HH:mm:ss}", date), ObjectFormatter.TokenFormat("{Date:d/M/yyyy HH:mm:ss}", testObject));
            Assert.AreEqual(string.Format("{0:MM/dd/yyyy}", date), ObjectFormatter.TokenFormat("{Date:MM/dd/yyyy}", testObject));
            Assert.AreEqual(string.Format("{0:dddd, MMMM d, yyyy}", date), ObjectFormatter.TokenFormat("{Date:dddd, MMMM d, yyyy}", testObject));

            Assert.AreEqual(string.Format("{0:N}", guid), ObjectFormatter.TokenFormat("{Guid:N}", testObject));
            Assert.AreEqual(string.Format("{0:D}", guid), ObjectFormatter.TokenFormat("{Guid:D}", testObject));
            Assert.AreEqual(string.Format("{0:B}", guid), ObjectFormatter.TokenFormat("{Guid:B}", testObject));
        }

        [TestMethod]
        public void CanEscapeSquirlyBrackets()
        {
            var hello = "Hello Lulu!";
            var test = new { hello = hello };

            Assert.AreEqual(string.Format("{0}", hello), ObjectFormatter.TokenFormat("{hello}", test));
            Assert.AreEqual(string.Format("{{0}}", hello), ObjectFormatter.TokenFormat("{{0}}", test));
            Assert.AreEqual(string.Format("{{{0}}}", hello), ObjectFormatter.TokenFormat("{{{hello}}}", test));
        }

        [TestMethod]
        public void CanAccessChildProperties()
        {
            var buddy = new
            {
                Ryan = new { FirstName = "Ryan", LastName = "McGarty" },
                Chris = new { FirstName = "Chris", LastName = "McGarty" }
            };

            Assert.AreEqual(buddy.Ryan.FirstName, ObjectFormatter.TokenFormat("{Ryan.FirstName}", buddy));
            Assert.AreEqual(buddy.Chris.FirstName, ObjectFormatter.TokenFormat("{Chris.FirstName}", buddy));
        }

        [TestMethod]
        public void CanUseIndexers()
        {
            var array = new[] { "Goodbye", "Cruel", "World" };
            var test = new { array = array, indexers = new Indexers() };

            Assert.AreEqual(array[0], ObjectFormatter.TokenFormat("{array[0]}", test));
            Assert.AreEqual(array[1], ObjectFormatter.TokenFormat("{array[1]}", test));
            Assert.AreEqual(array[2], ObjectFormatter.TokenFormat("{array[2]}", test));

            Assert.AreEqual(array[0][0].ToString(), ObjectFormatter.TokenFormat("{array[0][0]}", test));
            Assert.AreEqual(array[0][1].ToString(), ObjectFormatter.TokenFormat("{array[0][1]}", test));
            Assert.AreEqual(array[0][2].ToString(), ObjectFormatter.TokenFormat("{array[0][2]}", test));

            Assert.AreEqual(test.indexers[0], ObjectFormatter.TokenFormat("{indexers[0]}", test));
            Assert.AreEqual(test.indexers[1], ObjectFormatter.TokenFormat("{indexers[1]}", test));
            Assert.AreEqual(test.indexers[2], ObjectFormatter.TokenFormat("{indexers[2]}", test));
            
            Assert.AreEqual(test.indexers["Hello"], ObjectFormatter.TokenFormat("{indexers[\"Hello\"]}", test));
            Assert.AreEqual(test.indexers["Nurse"], ObjectFormatter.TokenFormat("{indexers[\"Nurse\"]}", test));
            Assert.AreEqual(test.indexers["!"], ObjectFormatter.TokenFormat("{indexers[\"!\"]}", test));
            
            Assert.AreEqual(test.indexers[0, "Hello"], ObjectFormatter.TokenFormat("{indexers[0, \"Hello\"]}", test));
            Assert.AreEqual(test.indexers[1, "Nurse"], ObjectFormatter.TokenFormat("{indexers[1, \"Nurse\"]}", test));
            Assert.AreEqual(test.indexers[3, "!"], ObjectFormatter.TokenFormat("{indexers[3, \"!\"]}", test));
        }

        [TestMethod]
        public void CanUseDictionaryForValues()
        {
            var tokens = new Dictionary<string, object>
            {
                {"int", 406},
                {"string", "Lulu"}
            };

            var test = new { property = "Whatevers" };

            Assert.AreEqual(tokens["int"].ToString(), ObjectFormatter.TokenFormat("{int}", tokens));
            Assert.AreEqual(tokens["string"].ToString(), ObjectFormatter.TokenFormat("{string}", tokens));
            Assert.AreEqual(string.Format("{0}:{1}", tokens["int"], test.property), ObjectFormatter.TokenFormat("{int}:{property}", test, tokens));
        }
    }
}
