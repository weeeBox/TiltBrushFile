using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEngine;

namespace TiltBrush
{
    public class BrushStroke
    {
        public BrushStroke(int brushIndex, Color brushColor, float brushSize, uint strokeFlags, List<ControlPoint> controlPoints)
        {
            this.brushIndex = brushIndex;
            this.brushColor = brushColor;
            this.brushSize = brushSize;
            this.strokeFlags = strokeFlags;
            this.controlPoints = controlPoints;
        }

        public static BrushStroke Read(BinaryReader reader)
        {
            Int32 brushIndex = reader.ReadInt32();
            Color brushColor = reader.ReadColor();
            float brushSize = reader.ReadFloat();
            reader.Skip(4); // 1u
            reader.Skip(4); // 3u
            UInt32 strokeFlags = reader.ReadUInt32();
            Int32 controlPointCount = reader.ReadInt32();
            List<ControlPoint> controlPoints = new List<ControlPoint>();
            for (int pointIndex = 0; pointIndex < controlPointCount; ++pointIndex)
            {
                controlPoints.Add(ControlPoint.Read(reader));
            }

            return new BrushStroke(brushIndex, brushColor, brushSize, strokeFlags, controlPoints);
        }

        public int brushIndex { get; set; }
        public Color brushColor { get; set; }
        public float brushSize { get; set; }
        public uint strokeFlags { get; set; }
        public List<ControlPoint> controlPoints { get; set; }
    }
}
