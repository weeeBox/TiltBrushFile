using NUnit.Framework;

using System;
using System.IO;

using TiltBrush;
using UnityEngine;

namespace Test
{
    [TestFixture]
    public class TestTiltIO
    {
        [Test]
        public void TestLoadAndSaveTilt()
        {
            TiltFile file = new TiltFile("test.tilt");

            string path = Path.Combine(Path.GetTempPath(), "output.tilt");
            file.Write(path);
        }
    }
}

