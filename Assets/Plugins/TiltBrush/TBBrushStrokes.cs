using System;
using System.IO;
using System.Collections.Generic;

namespace TiltBrushFile
{
    public class TBBrushStrokes : IEnumerable<TBBrushStroke>
    {
        const UInt32 SKETCH_SENTINEL = 3312887245u;

        UInt32 m_version;
        UInt32 m_reserved;
        UInt32 m_size;
        byte[] m_payload;

        List<TBBrushStroke> m_brushStrokes;

        TBBrushStrokes()
        {
        }

        public TBBrushStrokes(BinaryReader reader)
        {
            UInt32 sentinel = reader.ReadUInt32();
            if (sentinel != SKETCH_SENTINEL)
            {
                throw new Exception("Wrong sentinel: " + sentinel);
            }

            m_version = reader.ReadUInt32();
            m_reserved = reader.ReadUInt32();
            m_size = reader.ReadUInt32();
            m_payload = new byte[m_size];
            reader.Read(m_payload, 0, m_payload.Length);

            Int32 strokeCount = reader.ReadInt32();
            m_brushStrokes = new List<TBBrushStroke>(strokeCount);
            for (int strokeIndex = 0; strokeIndex < strokeCount; ++strokeIndex)
            {
                m_brushStrokes.Add(new TBBrushStroke(reader));
            }
        }

        public void Write(BinaryWriter writter)
        {
            writter.Write(SKETCH_SENTINEL);
            writter.Write(m_version);
            writter.Write(m_reserved);
            writter.Write(m_size);
            writter.Write(m_payload);
            writter.Write((Int32) m_brushStrokes.Count);

            foreach (var brushStroke in m_brushStrokes)
            {
                brushStroke.Write(writter);
            }
        }

        public void Clear()
        {
            m_brushStrokes.Clear();
        }

        public void AddAll(IEnumerable<TBBrushStroke> brushStrokes)
        {
            m_brushStrokes.AddRange(brushStrokes);
        }

        public void Add(TBBrushStroke brushStroke)
        {
            m_brushStrokes.Add(brushStroke);
        }

        #region IEnumerable implementation

        public IEnumerator<TBBrushStroke> GetEnumerator()
        {
            return m_brushStrokes.GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return m_brushStrokes.GetEnumerator();
        }

        #endregion

        #region Clonable

        public TBBrushStrokes Clone()
        {
            TBBrushStrokes clone = new TBBrushStrokes();
            clone.m_version = m_version;
            clone.m_reserved = m_reserved;
            clone.m_size = m_size;
            clone.m_payload = m_payload;
            List<TBBrushStroke> brushStrokes = new List<TBBrushStroke>(m_brushStrokes.Count);
            foreach (var brushStroke in m_brushStrokes)
            {
                brushStrokes.Add(brushStroke.Clone());
            }
            clone.m_brushStrokes = brushStrokes;
            return clone;
        }

        #endregion

        public List<TBBrushStroke> brushStrokes
        {
            get { return m_brushStrokes; }
        }
    }
}

