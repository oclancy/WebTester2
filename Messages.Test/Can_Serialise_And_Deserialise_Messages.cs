using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Messages.Test
{
    [TestClass]
    public class Can_Serialise_And_Deserialise_Messages
    {
        [TestMethod]
        public void Serialise_And_Deserialize()
        {
            var msg = new DataMessage() { Data = "Test" };

            var bytes = msg.Serialise();

            var msg2 = bytes.Deserialize();

            Assert.IsInstanceOfType(msg2, typeof(DataMessage));
        }
    }
}
