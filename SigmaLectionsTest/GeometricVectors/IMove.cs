using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework8ExampleInterface
{
    internal interface IMoveAtom
    {
        void MoveAtom();
    }

    internal interface IMoveDerive: IMoveAtom
    {
        void Move(int d);
    }

    internal interface ITurn: IMoveAtom
    {
        void Turn();
    }

    internal interface IComplexMove
    {
        void MoveAtom();
        void Turn();
    }

    internal interface IMove
    {
        void Move(int d);
    }
}
