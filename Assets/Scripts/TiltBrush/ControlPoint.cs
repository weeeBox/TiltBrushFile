using System;
using System.IO;

using UnityEngine;

namespace TiltBrush
{
    public class ControlPoint
    {
        ControlPoint()
        {
        }

        public ControlPoint(BinaryReader reader)
        {
            this.position = reader.ReadVector3();
            this.orientaion = reader.ReadQuaternion();
            this.pressure = reader.ReadFloat();
            this.timestamp = reader.ReadUInt32(); 
        }

        public void Write(BinaryWriter writter)
        {
            writter.Write(this.position);
            writter.Write(this.orientaion);
            writter.Write(this.pressure);
            writter.Write(this.timestamp);
        }

        public ControlPoint Clone()
        {
            ControlPoint clone = new ControlPoint();
            clone.orientaion = orientaion;
            clone.position = position;
            clone.pressure = pressure;
            clone.timestamp = timestamp;
            return clone;
        }

        public override string ToString()
        {
            return string.Format("Position={0} Orientaion={1} Pressure={2} Timestamp={3}", position, orientaion.eulerAngles, pressure, timestamp);
        }

        public Quaternion orientaion { get; set; }
        public Vector3 position { get; set; }
        public float pressure { get; set; }
        public UInt32 timestamp { get; set; }
    }
}