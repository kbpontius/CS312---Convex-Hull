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

            Polygon currentPolygon = polygonList[0];

            for(int i = 1; i < polygonList.Count; i++)
            {
                currentPolygon = Merge(currentPolygon, polygonList[i]);
            }
        }

        private Polygon Merge(Polygon currentPolygon, Polygon mergingPolygon)
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

        private void Split(List<PointF> pointList)
        {
            if (pointList.Count == 3)
            {
                Polygon polygon = CreateThreePointPolygon(pointList);
                DrawPolygon(polygon);
            }
            else if (pointList.Count == 2)
            {
                Polygon polygon = CreateTwoPointPolygon(pointList);
            }
            else
            {
                int centerIndex = (pointList.Count - 1) / 2;
                Split(GetLeft(centerIndex, pointList));
                Split(GetRight(centerIndex, pointList));
            }
        }

        private Polygon CreateTwoPointPolygon(List<PointF> pointList)
        {
            PolyPoint p1 = new PolyPoint(pointList[0], pointList[1]);
            PolyPoint p2 = new PolyPoint(pointList[1], pointList[0]);

            return new Polygon(p1, p2);
        }

        private Polygon CreateThreePointPolygon(List<PointF> pointList)
        {
            PolyPoint p1;
            PolyPoint p2;
            PolyPoint p3;

            double oneToOne = GetSlope(pointList[0], pointList[1]);
            double oneToTwo = GetSlope(pointList[0], pointList[2]);

            // The slope of oneToOne is more negative
            // than oneToTwo. Thus, to maintain clockwise
            // order, pointList[1] is the PolyPoint.next
            // of pointList[0].
            if (oneToOne < oneToTwo)
            {
                p1 = new PolyPoint(pointList[0], pointList[2], pointList[1]);
                p2 = new PolyPoint(pointList[1], pointList[0], pointList[2]);
                p3 = new PolyPoint(pointList[2], pointList[1], pointList[0]);
            }
            else
            {
                p1 = new PolyPoint(pointList[0], pointList[1], pointList[2]);
                p2 = new PolyPoint(pointList[1], pointList[2], pointList[1]);
                p3 = new PolyPoint(pointList[2], pointList[0], pointList[1]);
            }

            return new Polygon(p1, p3);
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
