using System;
using System.IO;

using UnityEngine;

namespace TiltBrush
{
    public class ControlPoint
    {
        private float pressure;
        private uint timestamp;

        public ControlPoint(Vector3 position, Quaternion orientaion, float pressure, uint timestamp)
        {
            this.position = position;
            this.orientaion = orientaion;
            this.pressure = pressure;
            this.timestamp = timestamp;
        }

        public static ControlPoint Read(BinaryReader reader)
        {
            Vector3 position = reader.ReadVector3();
            Quaternion orientaion = reader.ReadQuaternion();
            float pressure = reader.ReadFloat();
            uint timestamp = reader.ReadUInt32(); 
            return new ControlPoint(position, orientaion, pressure, timestamp);
        }

        public Quaternion orientaion { get; set; }
        public Vector3 position { get; set; }
    }
}