using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobsonROX.Util.Required;

namespace RobsonROX.Util.Tests
{
    [TestClass]
    public class RequiredTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_NullAssignment_ThrowException()
        {
            // ReSharper disable once UnusedVariable.Compiler
            #pragma warning disable 219
            Required<string> required = default(string);
            #pragma warning restore 219
        }

        [TestMethod]
        public void Ctor_ImplicitConversions_SuccessfulConversions()
        {
            const string expected = "Lorem Ipsum";
            Required<string> required = expected;
            string actual = required;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Ctor_EqualityComparisons_SuccessfulComparisons()
        {
            const string expectedString = "Lorem Ipsum";
            Required<string> expectedRequired = expectedString;
            Assert.IsTrue(expectedRequired == expectedString);
            Assert.IsTrue(expectedString == expectedRequired);
        }

    }
}
