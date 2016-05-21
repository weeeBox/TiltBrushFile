using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEngine;

namespace TiltBrush
{
    public partial class BrushStroke
    {
        public void Translate(Vector3 offset)
        {
            foreach (var point in controlPoints)
            {
                point.position += offset;
            }
        }
    }
}