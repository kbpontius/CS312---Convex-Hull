using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _1_convex_hull
{
    class PolyPoint
    {
        public PointF centerPoint;
        public PolyPoint prev;
        public PolyPoint next;

        public PolyPoint(PointF p1)
        {
            centerPoint = p1;
        }
    }
}
