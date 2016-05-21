using System;
using System.IO;

namespace TiltBrush
{
    public abstract class TiltFileChunk
    {
        public abstract void Write(BinaryWriter writter);
    }
}

