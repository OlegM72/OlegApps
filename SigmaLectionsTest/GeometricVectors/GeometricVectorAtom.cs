using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework8ExampleInterface
{
    internal class GeometricVectorAtom : GeometricVector, IMoveAtom
    {
        public GeometricVectorAtom(int startPoint = 0, int endPoint = 0) : base(startPoint, endPoint)
        {
        }

        public void MoveAtom()
        {
            this.startPoint++;
            this.endPoint++;
        }
    }

    internal class GeometricVectorMoveDerive : GeometricVector, IMoveDerive
    {
        public GeometricVectorMoveDerive(int startPoint = 0, int endPoint = 0) : base(startPoint, endPoint)
        {
        }

        public void Move(int d)
        {
            this.startPoint += d;
            this.endPoint += d;
        }

        public void MoveAtom()
        {
            this.startPoint++;
            this.endPoint++;
        }
    }

    internal class GeometricVectorMove : GeometricVector, IMove
    {
        public GeometricVectorMove(int startPoint = 0, int endPoint = 0) : base(startPoint, endPoint)
        {
        }

        public void Move(int d)
        {
            this.startPoint += d;
            this.endPoint += d;
        }
    }

    internal class GeometricVectorTurn : GeometricVector, ITurn
    {
        public GeometricVectorTurn(int startPoint = 0, int endPoint = 0) : base(startPoint, endPoint)
        {
        }

        public void MoveAtom()
        {
            this.startPoint++;
            this.endPoint++;
        }

        public void Turn()
        {
            this.endPoint *= -1;
        }
    }

    internal class GeometricVectorComplex : GeometricVector, IComplexMove
    {
        public GeometricVectorComplex(int startPoint = 0, int endPoint = 0) : base(startPoint, endPoint)
        {
        }

        public void MoveAtom()
        {
            this.startPoint++;
            this.endPoint++;
        }

        public void Turn()
        {
            this.endPoint *= -1;
        }
    }
}
