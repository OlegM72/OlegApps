// See https://aka.ms/new-console-template for more information
using Homework8ExampleInterface;
using System;

Console.WriteLine("Hello, World!");


IMoveAtom moveAtom = new GeometricVectorAtom(1, 10);
Console.WriteLine($"moveAtom = {moveAtom}"); // moveAtom = start point = 1 || end point = 10

moveAtom.MoveAtom();
Console.WriteLine(moveAtom); // start point = 2 || end point = 11

// moveAtom = new GeometricVectorMove(1, 10); // так робити не можна - не вдається перетворити тип

Console.WriteLine();
moveAtom = new GeometricVectorMoveDerive(1, 10);
moveAtom.MoveAtom();
Console.WriteLine(moveAtom); // start point = 2 || end point = 11

(moveAtom as IMoveDerive).Move(3);
Console.WriteLine(moveAtom); // start point = 5 || end point = 14

Console.WriteLine();
ITurn turn = new GeometricVectorTurn(1, 10);
Console.WriteLine(turn); // start point = 1 || end point = 10
turn.MoveAtom();
Console.WriteLine(turn); // start point = 2 || end point = 11
turn.Turn();
Console.WriteLine(turn); // start point = 2 || end point = -11

Console.WriteLine();
IComplexMove complexMove = new GeometricVectorComplex(1, 10);
Console.WriteLine(complexMove); // start point = 1 || end point = 10
complexMove.MoveAtom();
Console.WriteLine(complexMove); // start point = 2 || end point = 11
complexMove.Turn();
Console.WriteLine(complexMove); // start point = 2 || end point = -11

// turn = complexMove; // так не можна - не вдається перетворити тип
