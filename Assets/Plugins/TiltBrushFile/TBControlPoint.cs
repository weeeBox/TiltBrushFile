using System;
using System.IO;

using UnityEngine;

namespace TiltBrushFile
{
    public class TBControlPoint
    {
        TBControlPoint()
        {
        }

        public TBControlPoint(BinaryReader reader)
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

        public TBControlPoint Clone()
        {
            TBControlPoint clone = new TBControlPoint();
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

        public Vector3 tangent
        {
            get { return pressure * (orientaion * Vector3.up); }
        }

        public Vector3 normal
        {
            get { return orientaion * Vector3.forward; }
        }
    }
}