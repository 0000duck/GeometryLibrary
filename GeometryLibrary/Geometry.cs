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

    public struct PointWeightPair : IDimension
    {
        public int Dimension { get { return Point.Dimension; } }

        public Points.Point Point;
        public double Weight;
    }
}

