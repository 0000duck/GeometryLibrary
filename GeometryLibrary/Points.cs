using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryLibrary.Geometry
{
    public class Points
    {
        public class Point : GeometryBase, IDimension
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
}
