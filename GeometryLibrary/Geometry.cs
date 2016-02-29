using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryLibrary.Geometry
{
    public interface GeometryBase
    {
        GeometryBase Duplicate();
    }

    public class Points
    {
        public class Point : GeometryBase
        {
            #region operator
            public static Point operator +(Point a)
            {
                return (Point)a.Duplicate();
            }

            public static Point operator -(Point a)
            {
                var result = GetZero(a.Dimension);
                for (int i = 0; i < a.Dimension; i++)
                {
                    result[i] = -a[i];
                }
                return result;
            }

            public static Point operator +(Point a, Point b)
            {
                if (a.Dimension != b.Dimension) throw new Exceptions.DimensionMismatchException();

                var result = GetZero(a.Dimension);
                for (int i = 0; i < a.Dimension; i++)
                {
                    result[i] = a[i] + b[i];
                }
                return result;
            }

            public static Point operator -(Point a, Point b)
            {
                if (a.Dimension != b.Dimension) throw new Exceptions.DimensionMismatchException();

                var result = GetZero(a.Dimension);
                for (int i = 0; i < a.Dimension; i++)
                {
                    result[i] = a[i] - b[i];
                }
                return result;
            }

            public static Point operator *(Point a, double b)
            {
                var result = GetZero(a.Dimension);
                for (int i = 0; i < a.Dimension; i++)
                {
                    result[i] = a[i] * b;
                }
                return result;
            }

            public static Point operator *(double b, Point a)
            {
                var result = GetZero(a.Dimension);
                for (int i = 0; i < a.Dimension; i++)
                {
                    result[i] = a[i] * b;
                }
                return result;
            }

            public static Point operator /(Point a, double b)
            {
                var result = GetZero(a.Dimension);
                for (int i = 0; i < a.Dimension; i++)
                {
                    result[i] = a[i] / b;
                }
                return result;
            }
            #endregion

            public Point(params double[] Coordinate)
            {
                this.Coordinate = Coordinate;
            }

            public double[] Coordinate { get; private set; }

            public double this[int index]
            {
                get { return Coordinate[index]; }
                set { Coordinate[index] = value; }
            }

            public int Dimension { get { return Coordinate.Count(); } }

            public GeometryBase Duplicate()
            {
                return new Point(this.Coordinate);
            }

            public static Point GetZero(int Dimension)
            {
                var result = new double[Dimension];
                for (int i = 0; i < Dimension; i++)
                {
                    result[i] = 0;
                }
                return new Point(result);
            }
        }

        public class Point3d : Point
        {
            public Point3d(double X, double Y, double Z) : base(X, Y, Z)
            {
            }

            public double X { get { return this[0]; } set { this[0] = value; } }
            public double Y { get { return this[1]; } set { this[1] = value; } }
            public double Z { get { return this[2]; } set { this[2] = value; } }

            public static Point3d Zero = new Point3d(0, 0, 0);
        }

        public class Point2d : Point
        {
            public Point2d(double X, double Y) : base(X, Y)
            {
            }

            public double X { get { return this[0]; } set { this[0] = value; } }
            public double Y { get { return this[1]; } set { this[1] = value; } }

            public static Point2d Zero = new Point2d(0, 0);
        }

        public class PointList : IList<Point>
        {
            private List<Point> Content = new List<Point>();

            public int Dimension { get { return Content[0].Dimension; } }

            public Point this[int index]
            {
                get
                {
                    return ((IList<Point>)Content)[index];
                }

                set
                {
                    ((IList<Point>)Content)[index] = value;
                }
            }

            public int Count
            {
                get
                {
                    return ((IList<Point>)Content).Count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return ((IList<Point>)Content).IsReadOnly;
                }
            }

            public void Add(Point item)
            {
                if (this.Count > 0 && item.Dimension != this.Dimension) throw new Exceptions.DimensionMismatchException();
                ((IList<Point>)Content).Add(item);
            }

            public void Clear()
            {
                ((IList<Point>)Content).Clear();
            }

            public bool Contains(Point item)
            {
                return ((IList<Point>)Content).Contains(item);
            }

            public void CopyTo(Point[] array, int arrayIndex)
            {
                ((IList<Point>)Content).CopyTo(array, arrayIndex);
            }

            public IEnumerator<Point> GetEnumerator()
            {
                return ((IList<Point>)Content).GetEnumerator();
            }

            public int IndexOf(Point item)
            {
                return ((IList<Point>)Content).IndexOf(item);
            }

            public void Insert(int index, Point item)
            {
                if (this.Count > 0 && item.Dimension != this.Dimension) throw new Exceptions.DimensionMismatchException();
                ((IList<Point>)Content).Insert(index, item);
            }

            public bool Remove(Point item)
            {
                return ((IList<Point>)Content).Remove(item);
            }

            public void RemoveAt(int index)
            {
                ((IList<Point>)Content).RemoveAt(index);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IList<Point>)Content).GetEnumerator();
            }
        }
    }

    public class Domain
    {
        public double From;
        public double To;

        public Domain(double From,double To)
        {
            this.From = From;
            this.To = To;
        }
    }

    public class Curves
    {
        public class BasisSprine
        {
            public Points.PointList ControlPoints = new Points.PointList();
            public List<double> Knot = new List<double>();
            public int Dimension;

            public Domain Domain
            {
                get
                {
                    var knots = GetSortedKnot();
                    return new Domain(knots[Dimension], knots[knots.Count() - Dimension - 1]);
                }
            }

            public double[] GetSortedKnot()
            {
                return Knot.OrderBy(d => d).ToArray();
            }

            public Points.Point PointAt(double t)
            {
                var knots = GetSortedKnot();
                if (t < knots[Dimension] || knots[knots.Count() - Dimension - 1] < t) { return null; }
                var result = Points.Point.GetZero(ControlPoints.Dimension);
                for (int i = 0; i < Knot.Count()-Dimension; i++)
                {
                    result += ControlPoints[i] * DeBoorCoxAlgorithm(i, Dimension, t, knots);
                }
                return result;
            }

            public static double DeBoorCoxAlgorithm(int j,int n,double t,double[] ts)
            {
                if (n == 0)
                {
                    if (0 <= j && j <= ts.Count() - 2)
                    {
                        return (ts[j] <= t && t < ts[j + 1]) ? 1 : 0;
                    }
                    else {
                        throw new IndexOutOfRangeException();
                    }
                }
                else
                {
                    if (0 <= j && j <= ts.Count() - n - 2)
                    {
                        return (t - ts[j]) / (ts[j + n] - ts[j]) * DeBoorCoxAlgorithm(j, n - 1, t, ts) + (ts[j + n + 1] - t) / (ts[j + n + 1] - ts[j + 1]) * DeBoorCoxAlgorithm(j + 1, n - 1, t, ts);
                    }
                    else
                    {
                        throw new IndexOutOfRangeException();
                    }
                }
            }
        }
    }
}

