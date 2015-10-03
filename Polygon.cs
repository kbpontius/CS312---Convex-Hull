using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _1_convex_hull
{
    class Polygon
    {
        public PolyPoint left;
        public PolyPoint right;

        /*
            Creates a Polygon with a left and right point.
            :parameter: P
        */
        public Polygon(PolyPoint left, PolyPoint right)
        {
            this.left = left;
            this.right = right;
        }       
    }
}
