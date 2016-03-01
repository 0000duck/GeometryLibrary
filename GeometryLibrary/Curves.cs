using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryLibrary.Geometry
{
    public class Curves
    {
        public interface ICurve
        {
            Points.Point PointAt(double t);
            Domain Domain { get; }
        }

        public class BasisSprine : ICurve
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
                for (int i = 0; i < Knot.Count() - Degree; i++)
                {
                    result += ControlPoints[i] * Functions.DeBoorCoxAlgorithm(i, Degree, t, knots);
                }
                return result;
            }
        }

        public class NurbsCurve : ICurve
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
                for (int i = 1; i <= ControlPoints.Count; i++)
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

        public class BezierCurve : ICurve
        {
            public DimensionList<Points.Point> ControlPoints = new DimensionList<Points.Point>();

            public Domain Domain { get; set; } = new Domain(0, 1);

            public Points.Point PointAt(double t)
            {
                var result = Points.Point.GetZero(ControlPoints.Dimension);
                for (int i = 0; i < ControlPoints.Count; i++)
                {
                    result += ControlPoints[i] * Functions.BernsteinBasisPolynomials(ControlPoints.Count - 1, i, t);
                }
                return result;
            }
        }

        public class FunctionCurve : ICurve
        {
            public Domain Domain { get; set; } = new Domain(0, 1);

            public Func<double, Points.Point> Function;

            public Points.Point PointAt(double t)
            {
                if (!Domain.Containts(t)) return null;
                return Function(t);
            }
        }
    }
}
