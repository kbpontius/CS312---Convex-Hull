using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;
using _1_convex_hull;

namespace _2_convex_hull
{
    class ConvexHullSolver
    {
        Graphics g;
        Pen pen = new Pen(Color.Red, 1f);
        List<PointF> points;
        System.Windows.Forms.PictureBox pictureBoxView;

        public ConvexHullSolver(System.Drawing.Graphics g, System.Windows.Forms.PictureBox pictureBoxView)
        {
            this.g = g;
            this.pictureBoxView = pictureBoxView;
        }

        public void Refresh()
        {
            // Use this especially for debugging and whenever you want to see what you have drawn so far
            pictureBoxView.Refresh();
        }

        public void Pause(int milliseconds)
        {
            // Use this especially for debugging and to animate your algorithm slowly
            pictureBoxView.Refresh();
            System.Threading.Thread.Sleep(milliseconds);
        }

        public void Solve(List<PointF> pointList)
        {

            GenerateSolution(pointList);
        }

        private void GenerateSolution(List<PointF> pointList)
        {
            List<PointF> sortedList = pointList.OrderBy(o => o.X).ToList();
            points = sortedList;
            if(sortedList.Count > 1)
            {
                DrawPolygon(Split(sortedList));
            }
        }

        private Polygon Merge(Polygon p1, Polygon p2)
        {
            Tuple<PolyPoint, PolyPoint> top = FindTop(p1, p2);
            Tuple<PolyPoint, PolyPoint> bottom = FindBottom(p1, p2);

            top.Item1.prev = top.Item2;
            top.Item2.next = top.Item1;

            bottom.Item1.next = bottom.Item2;
            bottom.Item2.prev = bottom.Item1;

            return new Polygon(p1.left, p2.right);
        }

        private Tuple<PolyPoint, PolyPoint> FindTop(Polygon leftPolygon, Polygon rightPolygon)
        {
            bool leftPolygonIsFinished = false;
            bool rightPolygonIsFinished = false;
            PolyPoint currentLeft = leftPolygon.right;
            PolyPoint currentRight = rightPolygon.left;
            PolyPoint startingLeft = currentLeft;
            PolyPoint startingRight = currentRight;

            double currentSlope = GetSlope(currentLeft.centerPoint, currentRight.centerPoint);

            while(!leftPolygonIsFinished || !rightPolygonIsFinished)
            {
                while(!leftPolygonIsFinished)
                {
                    currentLeft = currentLeft.next;
                    double newSlope = GetSlope(currentLeft.centerPoint, currentRight.centerPoint);
                    if (newSlope < currentSlope)
                    {
                        rightPolygonIsFinished = false;
                        currentSlope = newSlope;
                    }
                    else
                    {
                        currentLeft = currentLeft.prev;
                        leftPolygonIsFinished = true;
                    }
                }

                while(!rightPolygonIsFinished)
                {
                    currentRight = currentRight.prev;
                    double newSlope = GetSlope(currentLeft.centerPoint, currentRight.centerPoint);
                    if (newSlope > currentSlope)
                    {
                        leftPolygonIsFinished = false;
                        currentSlope = newSlope;
                    }
                    else
                    {
                        currentRight = currentRight.next;
                        rightPolygonIsFinished = true;
                    }
                }
            }

            return Tuple.Create(currentLeft, currentRight);
        }

        private Tuple<PolyPoint, PolyPoint> FindBottom(Polygon leftPolygon, Polygon rightPolygon)
        {
            bool leftPolygonIsFinished = false;
            bool rightPolygonIsFinished = false;
            PolyPoint currentLeft = leftPolygon.right;
            PolyPoint currentRight = rightPolygon.left;
            PolyPoint startingLeft = currentLeft;
            PolyPoint startingRight = currentRight;

            double currentSlope = GetSlope(currentLeft.centerPoint, currentRight.centerPoint);

            while (!leftPolygonIsFinished || !rightPolygonIsFinished)
            {
                while (!leftPolygonIsFinished)
                {
                    currentLeft = currentLeft.prev;
                    double newSlope = GetSlope(currentLeft.centerPoint, currentRight.centerPoint);
                    if (newSlope > currentSlope)
                    {
                        rightPolygonIsFinished = false;
                        currentSlope = newSlope;
                    }
                    else
                    {
                        currentLeft = currentLeft.next;
                        leftPolygonIsFinished = true;
                    }
                }

                while (!rightPolygonIsFinished)
                {
                    currentRight = currentRight.next;
                    double newSlope = GetSlope(currentLeft.centerPoint, currentRight.centerPoint);
                    if (newSlope < currentSlope)
                    {
                        leftPolygonIsFinished = false;
                        currentSlope = newSlope;
                    }
                    else
                    {
                        currentRight = currentRight.prev;
                        rightPolygonIsFinished = true;
                    }
                }
            }

            return Tuple.Create(currentLeft, currentRight);
        }

        private Polygon Split(List<PointF> pointList)
        {
            if (pointList.Count == 2)
            {
                Polygon newPolygon = CreateTwoPointPolygon(pointList);
                return newPolygon;
            } 
            else if (pointList.Count == 3)
            {
                Polygon newPolygon = CreateThreePointPolygon(pointList);
                return newPolygon;
            }
            else
            {
                int centerIndex = (pointList.Count - 1) / 2;
                Polygon p1 = Split(GetLeft(centerIndex, pointList));
                Polygon p2 = Split(GetRight(centerIndex, pointList));

                Polygon newPolygon = Merge(p1, p2);
                return newPolygon;
            }
        }

        private Polygon CreateTwoPointPolygon(List<PointF> pointList)
        {
            PolyPoint p1 = new PolyPoint(pointList[0]);
            PolyPoint p2 = new PolyPoint(pointList[1]);

            p1.next = p2;
            p1.prev = p2;

            p2.next = p1;
            p2.prev = p1;

            return new Polygon(p1, p2);
        }

        private Polygon CreateThreePointPolygon(List<PointF> pointList)
        {
            PolyPoint p0 = new PolyPoint(pointList[0]);
            PolyPoint p1 = new PolyPoint(pointList[1]);
            PolyPoint p2 = new PolyPoint(pointList[2]);

            double zeroToOne = GetSlope(pointList[0], pointList[1]);
            double zeroToTwo = GetSlope(pointList[0], pointList[2]);

            // The slope of oneToOne is more negative
            // than oneToTwo. Thus, to maintain clockwise
            // order, pointList[1] is the PolyPoint.next
            // of pointList[0].
            if (zeroToOne < zeroToTwo)
            {
                p0.next = p1;
                p0.prev = p2;
                p1.next = p2;
                p1.prev = p0;
                p2.next = p0;
                p2.prev = p1;
            }
            else
            {
                p0.next = p2;
                p0.prev = p1;
                p1.next = p0;
                p1.prev = p2;
                p2.next = p1;
                p2.prev = p0;
            }

            return new Polygon(p0, p2);
        }

        // HELPER METHODS
        private double GetSlope(PointF left, PointF right)
        {
            /*
            x2      x1
           y1 |------------------------
              | *
              |  \
              |   \
           y2 |    *             
           
                I've arranged it so that the slope will be 
                negative when it APPEARS to be negative. In the sketch above,
                based on normal x-y coordinate plane, this would be a negative slope.
                Therefore, it will be referred to that way in the code.
            */
            
            double x1 = left.X;
            double y1 = left.Y;
            double x2 = right.X;
            double y2 = right.Y;

            return (y1 - y2) / (x2 - x1);
        }

        private void DrawPolygon(Polygon polygon)
        {
            PolyPoint initialPoint = polygon.left;
            PolyPoint currentPoint = polygon.left;

            DrawLine(currentPoint.centerPoint, currentPoint.next.centerPoint);
            currentPoint = currentPoint.next;

            while(currentPoint != initialPoint)
            {
                DrawLine(currentPoint.centerPoint, currentPoint.next.centerPoint);
                currentPoint = currentPoint.next;
            }
        }

        private void DrawLine(PointF point1, PointF point2)
        {
            g.DrawLine(pen, point1, point2);
            Pause(10);
        }

        // Splits the array and takes the right side, NOT including the centerIndex value.
        private List<PointF> GetLeft(int centerIndex, List<PointF> pointList)
        {
            List<PointF> newList = new List<PointF>();

            for(int i = 0; i <= centerIndex; i++)
            {
                newList.Add(pointList[i]);    
            }

            return newList;
        }

        // Splits the array and takes the right side, including the centerIndex value.
        private List<PointF> GetRight(int centerIndex, List<PointF> pointList)
        {
            List<PointF> newList = new List<PointF>();

            for(int i = centerIndex + 1; i < pointList.Count; i++)
            {
                newList.Add(pointList[i]);
            }

            return newList;
        }
    }
}
