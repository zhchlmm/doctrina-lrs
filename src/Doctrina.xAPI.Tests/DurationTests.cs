using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Doctrina.xAPI.Tests
{
    [TestClass]
    public class Duration_Tests
    {
        [TestMethod]
        [ExpectedException(typeof(FormatException), "Allowed dpluc")]
        public void Duplicate_Elements()
        {
            new Duration("P3Y1Y29YT4H35M59.14S");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "A userId of null was inappropriately allowed.")]
        public void Throws_When_Week_Distignator_Is_Combined_With_Others()
        {
            new Duration("P1Y2W");
        }

        [TestMethod]
        public void Week_Designator_Alone_Only()
        {
            string s = "P2W";
            var d = new Duration(s);
            Assert.AreEqual(d.ToString(), s);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Duration was allowed to not start P designator")]
        public void Must_Start_With_P()
        {
            new Duration("1Y2WP");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Duration was allowed to not start P designator")]
        public void Duration_Designator_Cannot_Be_Empty()
        {
            new Duration("P");
        }

        [TestMethod]
        public void Week_Designator()
        {
            string s = "P2W";
            var d = new Duration(s);
            Assert.AreEqual(d.ToString(), s);
        }

        [TestMethod]
        public void Year_Designator()
        {
            string s = "P1Y";
            var d = new Duration(s);
            Assert.AreEqual(d.ToString(), s);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Year designator are allowed without a value!")]
        public void Year_Designator_Without_Value()
        {
            string s = "PY";
            var d = new Duration(s);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Duration designators should not be allowed in this order.")]
        public void Designators_Unordered()
        {
            string s = "P1D1M1Y";
            var d = new Duration(s);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Duration designators should not be allowed in this order.")]
        public void Digits_Must_Be_first()
        {
            string s = "PD1";
            var d = new Duration(s);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Duration designators are allowed without a value!")]
        public void Designators_Most_Have_A_Value()
        {
            var d1 = new Duration("PY");
            var d2 = new Duration("PM");
            var d3 = new Duration("PD");
            var d4 = new Duration("PTH");
            var d5 = new Duration("PTH");
            var d6 = new Duration("PTM");
            var d7 = new Duration("PTS");
        }

        [TestMethod]
        public void Time_With_Hours()
        {
            string s = "PT36H";
            var d = new Duration(s);
            Assert.AreEqual(d.ToString(), s);
        }

        [TestMethod]
        public void One_Day_And_Twele_Hours()
        {
            string s1 = "P1DT12H";
            var d1 = new Duration(s1);
            Assert.AreEqual(d1.ToString(), s1);

            string s2 = "PT36H";
            var d2 = new Duration(s2);
            Assert.AreEqual(d2.ToString(), s2);

            Assert.AreNotEqual(d1, d2);
        }

        [TestMethod]
        public void Allows_Valid_Formats()
        {
            var d1 = new Duration("P23DT23H");
            var d2 = new Duration("P4Y");
            var d3 = new Duration("P4Y");
            var d4 = new Duration("P3Y1M29DT4H35M59.14S");
        }

        [TestMethod]
        public void Allows_Decimal_Fraction_With_FullStop()
        {
            new Duration("P0.5Y");
            new Duration("P0.5M");
            new Duration("P0.5D");
            new Duration("P0.5W");
            new Duration("PT0.5H");
            new Duration("PT0.5M");
            new Duration("PT0.5S");
        }

        [TestMethod]
        public void Allows_Decimal_Fraction_With_Comma()
        {
            new Duration("P0,5Y");
            new Duration("P0,5M");
            new Duration("P0,5D");
            new Duration("P0,5W");
            new Duration("PT0,5H");
            new Duration("PT0,5M");
            new Duration("PT0,5S");
        }
    }
}
