using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEngine;

namespace TiltBrushFile
{
    public partial class BrushStroke
    {
        Int32 m_brushIndex;
        Color m_brushColor;
        float m_brushSize;
        UInt32 m_reserver1;
        UInt32 m_reserver2;
        UInt32 m_strokeFlags;
        List<ControlPoint> m_controlPoints;

        BrushStroke()
        {
        }

        public BrushStroke(BinaryReader reader)
        {
            m_brushIndex = reader.ReadInt32();
            m_brushColor = reader.ReadColor();
            m_brushSize = reader.ReadFloat();
            m_reserver1 = reader.ReadUInt32();
            m_reserver2 = reader.ReadUInt32();
            m_strokeFlags = reader.ReadUInt32();
            int controlPointCount = reader.ReadInt32();
            m_controlPoints = new List<ControlPoint>();
            for (int pointIndex = 0; pointIndex < controlPointCount; ++pointIndex)
            {
                m_controlPoints.Add(new ControlPoint(reader));
            }
        }

        public void Write(BinaryWriter writter)
        {
            writter.Write(m_brushIndex);
            writter.Write(m_brushColor);
            writter.Write(m_brushSize);
            writter.Write(m_reserver1);
            writter.Write(m_reserver2);
            writter.Write(m_strokeFlags);
            writter.Write((Int32) m_controlPoints.Count);
            foreach (var controlPoint in m_controlPoints)
            {
                controlPoint.Write(writter);
            }
        }

        public BrushStroke Clone()
        {
            BrushStroke clone = new BrushStroke();
            clone.m_brushIndex = m_brushIndex;
            clone.m_brushColor = m_brushColor;
            clone.m_brushSize = m_brushSize;
            clone.m_reserver1 = m_reserver1;
            clone.m_reserver2 = m_reserver2;
            clone.m_strokeFlags = m_strokeFlags;
            List<ControlPoint> controlPoints = new List<ControlPoint>(m_controlPoints.Count);
            foreach (var controlPoint in m_controlPoints)
            {
                controlPoints.Add(controlPoint.Clone());
            }
            clone.m_controlPoints = controlPoints;
            return clone;
        }

        public Int32 brushIndex
        {
            get { return m_brushIndex; }
        }

        public Color brushColor
        {
            get { return m_brushColor; }
            set { m_brushColor = value; }
        }

        public float brushSize
        {
            get { return m_brushSize; }
            set { m_brushSize = value; }
        }

        public List<ControlPoint> controlPoints
        {
            get { return m_controlPoints; }
        }
    }
}
