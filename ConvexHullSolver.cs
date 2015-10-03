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
            Split(sortedList);
        }

        private Polygon Merge(Polygon p1, Polygon p2)
        {
            // FindTop(currentPolygon, mergingPolygon);

            return currentPolygon;
        }

        /*
        private void FindTop(Polygon leftPolygon, Polygon rightPolygon)
        {
            bool leftPolygonIsFinished = false;
            bool rightPolygonIsFinished = false;
            PointF currentLeft = leftPolygon.GetRight();
            PointF currentRight = rightPolygon.GetLeft();

            double currentSlope = GetSlope(currentLeft, currentRight);

            while(!leftPolygonIsFinished || !rightPolygonIsFinished)
            {
                while(!leftPolygonIsFinished && leftPolygon.hasPrev())
                {
                    double newSlope = GetSlope(leftPolygon.GetPrev(), currentRight);
                    
                    // TODO: REMOVE ME
                    // DrawLine(currentLeft, currentRight);
                    if (newSlope > currentSlope)
                    {
                        rightPolygonIsFinished = false;
                        currentLeft = leftPolygon.GetPrev();
                        currentSlope = newSlope;
                    }
                    else
                    {
                        leftPolygonIsFinished = true;
                    }
                }

                while(!rightPolygonIsFinished && rightPolygon.hasNext())
                {
                    double newSlope = GetSlope(currentLeft, rightPolygon.GetNext());

                    // TODO: REMOVE ME
                    // DrawLine(currentLeft, currentRight);
                    if (newSlope < currentSlope)
                    {
                        leftPolygonIsFinished = false;
                        currentRight = rightPolygon.GetNext();
                        currentSlope = newSlope;
                    }
                    else
                    {
                        rightPolygonIsFinished = true;
                    }
                }
            }

            DrawLine(currentLeft, currentRight);
        }
        */

        private Polygon Split(List<PointF> pointList)
        {
            if (pointList.Count == 2)
            {
                return CreateTwoPointPolygon(pointList);
            } 
            else if (pointList.Count == 3)
            {
                return CreateThreePointPolygon(pointList);
            }
            else
            {
                int centerIndex = (pointList.Count - 1) / 2;
                Polygon p1 = Split(GetLeft(centerIndex, pointList));
                Polygon p2 = Split(GetRight(centerIndex, pointList));

                return Merge(p1, p2);
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

            double oneToOne = GetSlope(pointList[0], pointList[1]);
            double oneToTwo = GetSlope(pointList[0], pointList[2]);

            // The slope of oneToOne is more negative
            // than oneToTwo. Thus, to maintain clockwise
            // order, pointList[1] is the PolyPoint.next
            // of pointList[0].
            if (oneToOne < oneToTwo)
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
            x2                      x1
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
            
            double x2 = left.X;
            double y1 = left.Y;
            double x1 = right.X;
            double y2 = right.Y;

            return (x1 - x2) / (y1 - y2);
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
