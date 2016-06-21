using System;
using FpcFramework.Converter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class ConverterTest
    {
        [TestMethod]
        public void ConvertInt()
        {
            int res = Converter.ConvertTo<int>(1);
            Assert.IsTrue(res == 1);           
        }


        [TestMethod]
        public void ConvertString()
        {
            string res = Converter.ConvertTo<string>("1");
            Assert.IsTrue(res == "1");
        }



        [TestMethod]
        public void ConvertBool()
        {
            bool res = Converter.ConvertTo<bool>(true);
            Assert.IsTrue(res);

            res = Converter.ConvertTo<bool>(false);
            Assert.IsTrue(!res);

            res = Converter.ConvertTo<bool>(1);
            Assert.IsTrue(res);

            res = Converter.ConvertTo<bool>(0);
            Assert.IsTrue(!res);
        }
    }
}
