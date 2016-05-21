using System;
using System.IO;

namespace TiltBrush
{
    public class TiltHeader
    {
        static readonly string SKETCH_SENTINEL = "tilT";

        string m_sentinel;

        UInt16 m_headerSize;
        UInt16 m_headerVersion;
        UInt32 m_reserved1;
        UInt32 m_reserved2;

        public TiltHeader(BinaryReader reader)
        {
            m_sentinel = reader.ReadString(4);
            if (m_sentinel != SKETCH_SENTINEL)
            {
                throw new Exception("Wrong sentinel: " + m_sentinel);
            }

            m_headerSize = reader.ReadUInt16();
            m_headerVersion = reader.ReadUInt16();
            m_reserved1 = reader.ReadUInt32();
            m_reserved2 = reader.ReadUInt32();
        }

        public void Write(BinaryWriter writter)
        {
            writter.Write(m_sentinel, 4);
            writter.Write(m_headerSize);
            writter.Write(m_headerVersion);
            writter.Write(m_reserved1);
            writter.Write(m_reserved2);
        }
    }
}

