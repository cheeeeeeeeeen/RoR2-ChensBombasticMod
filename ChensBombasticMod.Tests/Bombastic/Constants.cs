﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Chen.BombasticMod.Tests.Bombastic
{
    [TestClass]
    public class Constants
    {
        [TestMethod]
        public void ModVer_Length_ReturnsCorrectFormat()
        {
            string result = BombasticPlugin.ModVer;
            const int ModVersionCount = 3;

            int count = result.Split('.').Length;

            Assert.AreEqual(ModVersionCount, count);
        }

        [TestMethod]
        public void ModName_Value_ReturnsCorrectName()
        {
            string result = BombasticPlugin.ModName;
            const string ModName = "ChensBombasticMod";

            Assert.AreEqual(ModName, result);
        }

        [TestMethod]
        public void ModGuid_Value_ReturnsCorrectGuid()
        {
            string result = BombasticPlugin.ModGuid;
            const string ModGuid = "com.Chen.ChensBombasticMod";

            Assert.AreEqual(ModGuid, result);
        }
    }
}