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
        public class Point : GeometryBase,IDimension
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

    }

    public class Domain
    {
        public double A = 0;
        public double B = 1;
        public bool AEqual = true;
        public bool BEqual = true;

        public double Minimum { get { return Math.Min(A, B); } }
        public double Maximum { get { return Math.Max(A, B); } }

        public Domain(double A,double B,bool AEqual=true,bool BEqual=true)
        {
            this.A = A;
            this.B = B;
            this.AEqual = AEqual;
            this.BEqual = BEqual;
        }

        public bool Containts(double value)
        {
            return (Minimum < value && Maximum < value) || (AEqual && A == value) || (BEqual && B == value);
        }
    }

    public interface IDimension
    {
        int Dimension { get; }
    }

    public class DimensionList<T> : IList<T>, IDimension where T : IDimension
    {
        private List<T> Content = new List<T>();

        public int Dimension { get { return Content.Count == 0 ? 0 : Content[0].Dimension; } }

        public T this[int index]
        {
            get
            {
                return ((IList<T>)Content)[index];
            }

            set
            {
                ((IList<T>)Content)[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return ((IList<T>)Content).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IList<T>)Content).IsReadOnly;
            }
        }

        public void Add(T item)
        {
            if (this.Count > 0 && item.Dimension != this.Dimension) throw new Exceptions.DimensionMismatchException();
            ((IList<T>)Content).Add(item);
        }

        public void Clear()
        {
            ((IList<T>)Content).Clear();
        }

        public bool Contains(T item)
        {
            return ((IList<T>)Content).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ((IList<T>)Content).CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IList<T>)Content).GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return ((IList<T>)Content).IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            if (this.Count > 0 && item.Dimension != this.Dimension) throw new Exceptions.DimensionMismatchException();
            ((IList<T>)Content).Insert(index, item);
        }

        public bool Remove(T item)
        {
            return ((IList<T>)Content).Remove(item);
        }

        public void RemoveAt(int index)
        {
            ((IList<T>)Content).RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<T>)Content).GetEnumerator();
        }
    }


    public class Curves
    {
        public interface ICurve
        {
            Points.Point PointAt(double t);
            Domain Domain { get; }
        }

        public class BasisSprine:ICurve
        {
            public DimensionList<Points.Point> ControlPoints = new DimensionList<Points.Point>();
            public List<double> Knot = new List<double>();
            public int Degree = 1;

            public Domain Domain
            {
                get
                {
                    var knots = GetSortedKnot();
                    return new Domain(knots[Degree], knots[knots.Count() - Degree - 1]);
                }
            }

            public double[] GetSortedKnot()
            {
                return Knot.OrderBy(d => d).ToArray();
            }

            public Points.Point PointAt(double t)
            {
                var knots = GetSortedKnot();
                if (t < knots[Degree] || knots[knots.Count() - Degree - 1] < t) { return null; }
                var result = Points.Point.GetZero(ControlPoints.Dimension);
                for (int i = 0; i < Knot.Count()-Degree; i++)
                {
                    result += ControlPoints[i] * Functions.DeBoorCoxAlgorithm(i, Degree, t, knots);
                }
                return result;
            }
        }

        public class NurbsCurve:ICurve
        {
            public DimensionList<PointWeightPair> ControlPoints = new DimensionList<PointWeightPair>();
            public int Degree = 1;
            public List<double> Knot = new List<double>();

            public double[] GetSortedKnot()
            {
                return Knot.OrderBy(d => d).ToArray();
            }

            public Domain Domain
            {
                get; set;
            } = new Domain(0, 1);

            public Points.Point PointAt(double t)
            {
                if (!Domain.Containts(t)) return null;
                if (!IsValid()) return null;
                var a = Points.Point.GetZero(ControlPoints.Dimension);
                double b = 0;
                var knot = GetSortedKnot();
                for(int i = 0; i < ControlPoints.Count; i++)
                {
                    double c = Functions.DeBoorCoxAlgorithm(i, ControlPoints.Count, t, knot) * ControlPoints[i].Weight;
                    a += c * ControlPoints[i].Point;
                    b += c;
                }
                return a / b;
            }

            public bool IsValid()
            {
                if (Knot.Count != Degree + ControlPoints.Count) return false;
                return true;
            }
        }

        public struct PointWeightPair:IDimension
        {
            public int Dimension { get { return Point.Dimension; } }

            public Points.Point Point;
            public double Weight;
        }


        public class BezierCurve : ICurve
        {
            public DimensionList<Points.Point> ControlPoints = new DimensionList<Points.Point>();

            public Domain Domain { get; set; } = new Domain(0, 1);

            public Points.Point PointAt(double t)
            {
                var result = Points.Point.GetZero(ControlPoints.Dimension);
                for(int i = 0; i < ControlPoints.Count; i++)
                {
                    result += ControlPoints[i] * Functions.BernsteinBasisPolynomials(ControlPoints.Count - 1, i, t);
                }
                return result;
            }
        }
    }
}

