using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    class Program
    {
        static void Main(string[] args)
        {
            Pathing Game = new Pathing();

            Game.GameCreator();
            Console.ReadKey();
        }
    }

    class Point
    {

        public int PointValue { get; set; }
        public int XAxis { get; set; }
        public int YAxis { get; set; }
        public enum Type { Empty, Mouse, Cheese, Cat, Wall}
        public Type PointType { get; set; }

        public Point(int X, int Y)
        {
            this.XAxis = X;
            this.YAxis = Y;
            this.PointType = Type.Empty;
            this.PointValue = 10000000;
        }
    }

    class Pathing
    {
        public Point[,] Grid { get; set; }
        public int GridSize { get; set; }
        public Random rng { get; set; }

        public Pathing()
        {
            this.GridSize = 12;
            this.rng = new Random();

            //populate grid with empty points
            this.Grid = new Point[GridSize, GridSize];
            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {

                    Grid[x, y] = new Point(x,y);
                }
            }
            //Place a mouse
            //int MouseXRNG = rng.Next(0, GridSize);
            //int MouseYRNG = rng.Next(0, GridSize);
            //Grid[MouseXRNG, MouseYRNG].PointType = Point.Type.Mouse;
            //Grid[MouseXRNG, MouseYRNG].PointValue = 0;
            Grid[5, 5].PointType = Point.Type.Mouse;
            Grid[5, 5].PointValue = 0;
            
            //Place cheese
            while (true)
            {
                int CheeseXRNG = rng.Next(0, GridSize);
                int CheeseYRNG = rng.Next(0, GridSize);
                if (Grid[CheeseXRNG, CheeseYRNG].PointType == Point.Type.Empty)
                {
                    Grid[CheeseXRNG, CheeseYRNG].PointType = Point.Type.Cheese;
                    break;
                }
                
            }
            //Place Cat

            Grid[0, 0].PointType = Point.Type.Cat;
        }

        public void DrawGrid()
        {
            for (int y = 0; y < GridSize; y++)
            {
                for (int x = 0; x < GridSize; x++)
                {
                    if (Grid[x, y].PointType == Point.Type.Empty)
                    {
                        Console.Write("[ ]");
                    }
                    else if (Grid[x, y].PointType == Point.Type.Mouse)
                    {
                        Console.Write("[M]");
                    }
                    else if (Grid[x, y].PointType == Point.Type.Cat)
                    {
                        Console.Write("[X]");
                    }
                    else if (Grid[x, y].PointType == Point.Type.Cheese)
                    {
                        Console.Write("[C]");
                    }
                    else
                    {
                        Console.Write("[!]");
                    }
                    //if (Grid[x, y].PointValue > 1000)
                    //{
                    //    Console.Write("[ ]");
                    //}
                    //else
                    //{
                    //    Console.Write("[" + Grid[x, y].PointValue + "]");
                    //}
                }
                Console.WriteLine();
            }
        }

        public void PlaceNumbers()
        {
            int numberIncrementor = 0;
            int NumberUsed = numberIncrementor + 1;

            // loop threw all items within the grid adding numbers to surrounding squares
            for (int i = 0; i < GridSize; i++)
            {
                foreach (Point GridPoint in Grid)
                {
                    if (GridPoint.PointValue == numberIncrementor)
                    {
                        foreach (Point item in SurroundingPoints(GridPoint, true))
                        {
                            Grid[item.XAxis, item.YAxis].PointValue = NumberUsed;
                        }
                    }
                }
                numberIncrementor++;
                NumberUsed++;
            }
        }

        public int GetUserMove()
        {
            //returning an integer for move, if up then 1, if 
            //down then 3, if right then 2, if right then 4.

            //[ ][ ][1][ ][ ]
            //[ ][4][0][2][ ]  
            //[ ][ ][3][ ][ ]

            ConsoleKey input = new ConsoleKey();
            bool gettingInput = true;
            while (gettingInput)
            {
                input = Console.ReadKey().Key;

                if (input == ConsoleKey.UpArrow)
                {

                    gettingInput = false;
                    return 1;
                }
                else if (input == ConsoleKey.DownArrow)
                {

                    gettingInput = false;
                    return 3;
                }
                else if (input == ConsoleKey.RightArrow)
                {

                    gettingInput = false;
                    return 2;
                }
                else if (input == ConsoleKey.LeftArrow)
                {

                    gettingInput = false;
                    return 4;
                }
                else
                {
                    Console.WriteLine("Press up arrow, down arrow, left arrow, right arrow.");

                }
            }
            return 0;
        }



        public void MoveCat()
        {
            foreach (Point item in Grid)
            {
                if (item.PointType == Point.Type.Cat)
                {
                    foreach (Point ListItem in SurroundingPoints(item, false))
                    {
                        if (item.PointValue - 1 == ListItem.PointValue)
                        {
                            item.PointType = Point.Type.Empty;
                            this.Grid[ListItem.XAxis, ListItem.YAxis].PointType = Point.Type.Cat;
                            break;
                        }
                    }
                    break;
                }
            }
        }

        public List<Point> SurroundingPoints(Point inputPoint, bool UsingForPopulationOfNumbers)
        {
            List<Point> List = new List<Point>();
            List<Point> ReturnList = new List<Point>();
            Point SeedPoint = Grid[inputPoint.XAxis, inputPoint.YAxis];
            if (SeedPoint.YAxis - 1 >= 0)
            {
                Point UpPoint = Grid[SeedPoint.XAxis, SeedPoint.YAxis - 1]; 
                List.Add(UpPoint);
            }
            if (SeedPoint.YAxis + 1 < GridSize)
            {
                Point DownPoint = Grid[SeedPoint.XAxis, SeedPoint.YAxis + 1]; 
                List.Add(DownPoint);
            }
            if (SeedPoint.XAxis + 1 < GridSize)
            {
                Point RightPoint = Grid[SeedPoint.XAxis + 1, SeedPoint.YAxis]; 
                List.Add(RightPoint);
            }
            if (SeedPoint.XAxis - 1 >= 0)
            {
                Point LeftPoint = Grid[SeedPoint.XAxis - 1, SeedPoint.YAxis]; 
                List.Add(LeftPoint);
            }

            //make sure the points dont have numbers set to them already.
            if (UsingForPopulationOfNumbers)
            {
                foreach (Point item in List)
                {
                    if (Grid[item.XAxis, item.YAxis].PointValue < 100)
                    {

                    }
                    else
                    {
                        ReturnList.Add(item);
                    }
                }
                return ReturnList;
            }

            return List;

        }

        public void GameCreator()
        {
            int x = 0;
            this.PlaceNumbers();
            while (true)
            {
                this.DrawGrid();
                this.MoveCat();
                Console.WriteLine(x);
                Console.ReadKey();
                Console.Clear();
                x++;
            }
            
        }
    }



}
