using NUnit.Framework;

using System;
using System.IO;

using UnityEngine;

namespace Test
{
    [TestFixture]
    public class TestBinaryReadWriteExtension
    {
        [Test]
        public void TestReadTypes()
        {
            string str = "1234";
            Color color = new Color(0.1f, 0.2f, 0.3f, 0.4f);
            Vector3 vector = new Vector3(1.23f, 4.56f, 7.89f);

            byte[] data;
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writter = new BinaryWriter(stream))
                {
                    writter.Write(str, 4);
                    writter.Write(color);
                    writter.Write(vector);
                }

                data = stream.ToArray();
            }

            using (MemoryStream stream = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    Assert.AreEqual(str, reader.ReadString(4));
                    Assert.AreEqual(color, reader.ReadColor());
                    Assert.AreEqual(vector, reader.ReadVector3());
                }
            }
        }
    }
}

