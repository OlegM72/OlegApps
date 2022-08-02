using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework8ExampleInterface
{
    internal class GeometricVector
    {
        protected int startPoint;
        protected int endPoint;
        public GeometricVector(int startPoint = 0, int endPoint = 0)
        {
            this.startPoint = startPoint;
            this.endPoint = endPoint;
        }

        public int StartPoint { get => startPoint; set => startPoint = value; }
        public int EndPoint { get => endPoint; set => endPoint = value; }

        public override string? ToString()
        {
            return $"start point = {startPoint} || end point = {endPoint}";
        }
    }
}
