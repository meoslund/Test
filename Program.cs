using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TickTackToe
{
    class MyClass
    {

        public struct CellProbability
        {
            public int x;
            public int y;
            public int WinCount;
            public int LossCount;
        }

        /********************** ResetGrid Method **********************/
        public void ResetGrid(int x, int y, char[,] Matrix)
        {
            // Console.WriteLine("I'm in the ResetGrid method");
            for (x = 0; x < 3; x++)
                for (y = 0; y < 3; y++)
                {
                    Matrix[x, y] = ' ';
                }
            // Console.WriteLine("I'm leaving the ResetGrid method");
        }

        /********************** PrintGrid Method **********************/
        public void PrintGrid(int x, int y, char[,] Matrix)
        {
            // Console.WriteLine("I'm in the PrintGrid method");
            Console.WriteLine("   A    B    C");
            for (y = 0; y < 3; y++)
            {
                for (x = 0; x < 3; x++)
                {
                    if (x < 1) Console.Write("{0} ", y + 1);
                    Console.Write("{0} ", Matrix[x, y]);
                    if (x < 2) Console.Write(" | ");
                }
                Console.WriteLine();
                if (y < 2) Console.WriteLine("  --------------");
            }
            Console.WriteLine();
            // Console.WriteLine("I'm leaving the PrintGrid method");
        }

        /********************** GridInput Method **********************/
        public void GridInput(int x, int y, char[,] Matrix)
        {
            // Console.WriteLine("I'm in the GridInput method");

        }

        /********************** Flip a Coin Method **********************/
        public char FlipCoin()
        {
            // Console.WriteLine("I'm in the FlipCoin method");
            string coin;
            char WhoseTurn;
            Random rnd = new Random();
            int flip = rnd.Next(1, 2);  // creates a number between 1 and 2
            if (flip == 1)
                coin = "Heads";
            else
                coin = "Tails";

            Console.WriteLine("Do you want Heads or Tails?");
            string line;
            line = Console.ReadLine();
            Console.WriteLine("Coin is {0}", coin);
            if (line == coin)
            {
                Console.WriteLine("You go first.");
                WhoseTurn = 'P';
            }
            else
            {
                Console.WriteLine("Computer goes first.");
                WhoseTurn = 'C';
            }
            return WhoseTurn;
        }

        /********************** Check if there is a win **********************/
        public char CheckForWin(char[,] Matrix)
        {
            int x, y;
            char win = ' ';
            /* X pass */
            for (x = 0; x < 3 & win == ' '; x++)
            {
                if (Matrix[x, 0] == 'X' & Matrix[x, 1] == 'X' & Matrix[x, 2] == 'X') win = 'X';
            }
            if (Matrix[0, 0] == 'X' & Matrix[1, 1] == 'X' & Matrix[2, 2] == 'X') win = 'X';
            if (Matrix[2, 2] == 'X' & Matrix[1, 1] == 'X' & Matrix[0, 0] == 'X') win = 'X';

            /* O pass */
            for (y = 0; y < 3 & win == ' '; y++)
            {
                if (Matrix[0, y] == 'O' & Matrix[1, y] == 'O' & Matrix[2, y] == 'O') win = 'O';
            }
            if (Matrix[0, 0] == 'O' & Matrix[1, 1] == 'O' & Matrix[2, 2] == 'O') win = 'O';
            if (Matrix[2, 2] == 'O' & Matrix[1, 1] == 'O' & Matrix[0, 0] == 'O') win = 'O';
            return win;
        }

        /********************** Computer Random Move **********************/
        public char[,] RandomMove(char[,] Matrix)
        {
            bool GoodMove = false;
            Random rnd = new Random();
            do
            {
                int x = rnd.Next(0, 3);  // creates a number between 0 and 2 
                int y = rnd.Next(0, 3);  // creates a number between 0 and 2
                if (Matrix[x, y] == ' ')
                {
                    GoodMove = true;
                    Matrix[x, y] = 'O';
                    Console.WriteLine("Computer's turn - {0} {1}", x, y);
                }
            } while (GoodMove == false);
            return Matrix;
        }

        /******************* Computer Intelligent Move ***************/
        public CellProbability RegressionMove(char[,] Matrix)
        {
            CellProbability SuggestedMove = new CellProbability();
            CellProbability BestMove = new CellProbability();
            int xc, yc, xp, yp;
            SuggestedMove.WinCount = 0;
            SuggestedMove.LossCount = 0;
            BestMove.WinCount = 0;
            BestMove.LossCount = 0;
            for (xc=0;xc<3;xc++)
            {
                for (yc = 0; yc < 3; yc++)
                {
                    if (Matrix[xc, yc] == ' ')
                    {
                        Matrix[xc, yc] = 'O';
                        PrintGrid(3, 3, Matrix);
                        if (CheckForWin(Matrix) == 'O')
                        {
                            SuggestedMove.x = xc;
                            SuggestedMove.y = yc;
                            SuggestedMove.WinCount++;
                        }
                        else for (xp = 0; xp < 3; xp++)
                            {
                                for (yp = 0; yp < 3; yp++)
                                {
                                    if (Matrix[xp, yp] == ' ')
                                    {
                                        Matrix[xp, yp] = 'X';
                                        PrintGrid(3, 3, Matrix);
                                        if (CheckForWin(Matrix) == 'X')
                                            SuggestedMove.LossCount++;
                                        else SuggestedMove = RegressionMove(Matrix);
                                    }
                                }
                            }

                    }
                }
            }
            /* 
             * For each open cell
             *      If cell will give me a win, 
             *          then note a 1 win to 0 loss on cell and return cell x,y & ratio (ie. 1).
             *          else (how many winning paths & how many losing paths will I have below me?)
             *              for each possible player move after I take the cell, 
             *                  if player gets a win, then note 0 win and 1 loss on cell. Return cell x,y & ratio (i.e. 0)
             *                  else call this method and return . 
             *                  Accumulate the win & loss counts
             *          As looping through for, Remember which cell has the hightest win to loss ratio.
             * Return cell x,y & ratio           
             */
            return SuggestedMove;
        }

        /********************** Computer Move **********************/
        public char[,] ComputerMove(char[,] Matrix, bool PlayHard)
        {
            CellProbability SuggestedMove = new CellProbability();
            if (!PlayHard)
                Matrix = RandomMove(Matrix);
            else
            {
               SuggestedMove = RegressionMove(Matrix);
               Matrix[SuggestedMove.x, SuggestedMove.y] = 'O';

            }
            PrintGrid(3, 3, Matrix);
            return Matrix;
        }

        /********************** Player Move **********************/
        public char[,] PlayerMove(char[,] Matrix)
        {
            string row, column;
            int x = 0, y = 0;
            bool GoodMove = false;
            do
            {
                Console.WriteLine("\nWhat is your move? ");
                Console.WriteLine("Column:");
                column = Console.ReadLine();
                Console.WriteLine("Row:");
                row = Console.ReadLine();
                switch (column)
                {
                    case "A":
                        x = 0;
                        break;
                    case "B":
                        x = 1;
                        break;
                    case "C":
                        x = 2;
                        break;
                    default: break;
                }
                switch (row)
                {
                    case "1":
                        y = 0;
                        break;
                    case "2":
                        y = 1;
                        break;
                    case "3":
                        y = 2;
                        break;
                    default: break;
                }
                if (Matrix[x, y] == ' ')
                    GoodMove = true;
                else
                    Console.WriteLine("That cell already used. Please pick another cell. \n");
            } while (GoodMove == false);
            Matrix[x, y] = 'X';
            PrintGrid(3, 3, Matrix);
            return Matrix;
        }

        /********************** Check if there are any open cells for additional moves ********************/
        public bool CheckforMoves (char[,] Matrix)
        {
            bool MoreMoves = false;
            int x, y;
            for (x = 0; x < 3; x++)
                for (y = 0; y < 3; y++)
                    if (Matrix[x, y] == ' ') MoreMoves = true;
            return MoreMoves; 
        }

    }



    class Program
    {

        /********************** Main Method **********************/
        static void Main()
        {
            var Grid = new char[3, 3];
            char WhoseTurn;
            char win = ' ';
            bool MoreMoves = true; 
            bool PlayHard = false;  // This determines whether to go with Regression algorithm (i.e. True) or random move selection (i.e. False).
            MyClass meth = new MyClass();
            Console.WriteLine("-------------- Welcome to Tick Tack Toe! --------------\n");
            meth.ResetGrid(3, 3, Grid);
            meth.PrintGrid(3, 3, Grid);
            WhoseTurn = meth.FlipCoin();
            while (win == ' ' & MoreMoves)
            {
                if (WhoseTurn == 'P')
                {
                    Grid = meth.PlayerMove(Grid);
                    win = meth.CheckForWin(Grid);
                    if (win == 'X')
                        Console.WriteLine("\nYou Wins!");
                    else
                        WhoseTurn = 'C';
                }
                else
                {
                    Grid = meth.ComputerMove(Grid, PlayHard);
                    win = meth.CheckForWin(Grid);
                    if (win == 'O')
                        Console.WriteLine("\nComputer Wins! You suck human!");
                    else
                        WhoseTurn = 'P';
                }
                MoreMoves = meth.CheckforMoves(Grid);
                if (!MoreMoves) Console.WriteLine("\nCats game. Better luck next time.");
            }
            Console.WriteLine("\nThanks for playing!");
            Console.Read();
        }

    }

}
