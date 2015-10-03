using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _1_convex_hull
{
    class PolyPoint
    {
        PointF centerPoint;
        PointF prev;
        PointF next;

        /*
            Two-point polygon.
            :parameter: p1 is the centerPoint.
            :parameter: p2 is the next and prev.
        */
        public PolyPoint(PointF p1, PointF p2)
        {
            centerPoint = p1;
            prev = p2;
            next = p2;
        }

        /*
            Three-point polygon.
            :parameter: p1 is the centerPoint.
            :parameter: p2 is the prev.
            :parameter: p3 is the next.
        */
        public PolyPoint(PointF p1, PointF p2, PointF p3)
        {
            centerPoint = p1;
            prev = p2;
            next = p3;
        }
    }
}
