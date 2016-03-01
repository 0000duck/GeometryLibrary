using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeometryLibrary.Geometry
{
    public class Surfaces
    {
        public interface IUVSurface
        {
            Points.Point PointAt(double u, double v);
            Domain DomainU { get; }
            Domain DomainV { get; }
        }

        public class NurbsSurface:IUVSurface{
            public DimensionList<PointWeightPair> ControlPoints;
            public int uCount;
            public int vCount;
            public List<double> Knot = new List<double>();
            public int degreeU = 3;
            public int degreeV = 3;
            public Domain DomainU { get; set; } = new Domain(0, 1);
            public Domain DomainV { get; set; } = new Domain(0, 1);

            public double[] GetSortedKnot()
            {
                return Knot.OrderBy(d => d).ToArray();
            }

            public PointWeightPair GetControlPoint(int a, int b)
            {
                if (a > uCount || b > vCount || ControlPoints.Count > uCount * vCount) throw new IndexOutOfRangeException();
                return ControlPoints[vCount * b + a];
            }

            public Points.Point PointAt(double u,double v)
            {
                if (!DomainU.Containts(u) || !DomainV.Containts(v)) return null;
                double den = 0;
                var knot = GetSortedKnot();
                var result = Points.Point.GetZero(ControlPoints.Dimension);
                for (int i = 0; i < uCount; i++)
                {
                    for (int j = 0; j < vCount; j++)
                    {
                        var a = Functions.DeBoorCoxAlgorithm(i, degreeU, u, knot) * Functions.DeBoorCoxAlgorithm(j, degreeV, v, knot) * GetControlPoint(i, j).Weight;
                        den += a;
                        result += a * GetControlPoint(i, j).Point;
                    }
                }
                return result / den;
            }
        }
        public class FunctionSurface : IUVSurface
        {
            public Domain DomainU { get; set; } = new Domain(0, 1);
            public Domain DomainV { get; set; } = new Domain(0, 1);

            public Func<double, double, Points.Point> Function;

            public Points.Point PointAt(double u, double v)
            {
                if (!DomainU.Containts(u) || !DomainV.Containts(v)) return null;
                return Function(u, v);
            }
        }
    }
}
