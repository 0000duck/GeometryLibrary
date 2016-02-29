using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryLibrary.Geometry
{
    public class Primitive
    {
        public class Point
        {
            public interface IPoint
            {
            }

            public struct Point2d
            {
                #region operator
                public static Point2d operator +(Point2d a)
                {
                    return new Point2d(a.X, a.Y);
                }

                public static Point2d operator -(Point2d a)
                {
                    return new Point2d(-a.X, -a.Y);
                }

                public static Point2d operator +(Point2d a, Point2d b)
                {
                    return new Point2d(a.X + b.X, a.Y + b.Y);
                }

                public static Point2d operator -(Point2d a, Point2d b)
                {
                    return new Point2d(a.X - b.X, a.Y - b.Y);
                }

                public static Point2d operator *(Point2d a, double b)
                {
                    return new Point2d(a.X * b, a.Y * b);
                }

                public static Point2d operator *(double a, Point2d b)
                {
                    return new Point2d(b.X * a, b.Y * a);
                }

                public static Point2d operator /(Point2d a, double b)
                {
                    return new Point2d(a.X / b, a.Y / b);
                }
                #endregion

                public Point2d(double X, double Y)
                {
                    this.X = X;
                    this.Y = Y;
                }

                double X;
                double Y;

                public static Point2d Zero = new Point2d(0, 0);
            }

            public struct Point3d
            {
                #region operator
                public static Point3d operator +(Point3d a)
                {
                    return new Point3d(a.X, a.Y,a.Z);
                }

                public static Point3d operator -(Point3d a)
                {
                    return new Point3d(-a.X, -a.Y, a.Z);
                }

                public static Point3d operator +(Point3d a, Point3d b)
                {
                    return new Point3d(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
                }

                public static Point3d operator -(Point3d a, Point3d b)
                {
                    return new Point3d(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
                }

                public static Point3d operator *(Point3d a, double b)
                {
                    return new Point3d(a.X * b, a.Y * b, a.Z * b);
                }

                public static Point3d operator *(double a, Point3d b)
                {
                    return new Point3d(b.X * a, b.Y * a, b.Z * a);
                }

                public static Point3d operator /(Point3d a, double b)
                {
                    return new Point3d(a.X / b, a.Y / b, a.Z / b);
                }
                #endregion

                public Point3d(double X,double Y,double Z)
                {
                    this.X = X;
                    this.Y = Y;
                    this.Z = Z;
                }

                double X;
                double Y;
                double Z;

                public static Point3d Zero = new Point3d(0, 0);
            }
        }
    }

    public class Advanced
    {
        public struct PointKnotPair<T> where T :Primitive.Point.IPoint {
            public T Point;
            public double Knot;
        }
    }

    public class Curves
    {
        public class BasisSprine<T> where T : Primitive.Point.IPoint
        {
            public List<Advanced.PointKnotPair<T>> Points = new List<Advanced.PointKnotPair<T>>();
        }
    }
}

