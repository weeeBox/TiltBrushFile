using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using UnityEngine;

namespace TiltBrushFile
{
    public partial class TBBrushStroke
    {
        const float kMinValue = -10000;
        const float kMaxValue = 10000;

        public Bounds bounds
        {
            get
            {
                Vector3 min = new Vector3(kMaxValue, kMaxValue, kMaxValue);
                Vector3 max = new Vector3(kMinValue, kMinValue, kMinValue);

                foreach (var point in controlPoints)
                {
                    min.x = Mathf.Min(min.x, point.position.x);
                    min.y = Mathf.Min(min.y, point.position.y);
                    min.z = Mathf.Min(min.z, point.position.z);

                    max.x = Mathf.Min(max.x, point.position.x);
                    max.y = Mathf.Min(max.y, point.position.y);
                    max.z = Mathf.Min(max.z, point.position.z);
                }

                Vector3 center = 0.5f * (min + max);
                Vector3 size = max - min;

                return new Bounds(center, size);

            }
        }
    }
}