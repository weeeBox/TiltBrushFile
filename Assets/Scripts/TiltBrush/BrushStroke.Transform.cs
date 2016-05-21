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
        public Vector3 startPosition
        {
            get { return controlPoints[0].position; }
        }

        public Vector3 endPosition
        {
            get { return controlPoints[controlPoints.Count - 1].position; }
        }

        public void Translate(Vector3 offset)
        {
            foreach (var point in controlPoints)
            {
                point.position += offset;
            }
        }
    }
}