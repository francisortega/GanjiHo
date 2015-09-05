// AUTHOR:  Francis Lorenz Ortega
// ID:      1295578
// AUTHOR:  David Tang
// ID:      6600689
// VERSION: Project Deliverable I
// DATE:    02-26-15

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
namespace GanjiHo
{
    class Program
    {
        private static int boardSize;
        private static String[,] board;
        private enum Players { White, Black };
        private static bool WhiteIsComputer = true, BlackIsComputer = true;
        private static bool isWhite = true;
        private static ArrayList level1 = new ArrayList();
        private static ArrayList level2 = new ArrayList();
        private static int[] bestConfigAtLevel1 = new int[3], bestConfigAtRoot = new int[3];
        private static char[] Alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'X', 'Y', 'Z' };
        static void Main(string[] args)
        {
            Intro();
            InitializeBoard();
            DrawBoard();
            Console.WriteLine("");
            bestConfigAtLevel1[0] = (isWhite) ? Int32.MinValue : Int32.MaxValue;
            bestConfigAtRoot[0] = (isWhite) ? Int32.MaxValue : Int32.MinValue;
            // Keep playing
            while (!IsGameOver()) TakeTurn();
            if (IsGameOver())
            {
                if (isWhite)
                    Console.WriteLine("No more possible moves for White player, game Over! Black Player Won!");
                else
                    Console.WriteLine("No more possible moves for Black player, game Over! White Player Won!");
            }
            Console.ReadLine();
        }

        // METHOD:  Intro
        // PURPOSE: Basic introduction
        static void Intro()
        {
            string side = "";
            Console.Title = "GanJi-Ho";
            Console.WriteLine("Welcome to Game of GanJi-Ho!");
            // Should auto-start at 8; then increase until there is a clear winner
            boardSize = 8;
            board = new String[8, 8];
            Console.WriteLine("Please choose your side, Enter 'W' for White or 'B' for Black");
            side = Console.ReadLine().ToUpper();
            while (!side.Equals("W") && !side.Equals("B"))
            {
                Console.WriteLine("Invalid choice, please reenter: ");
                side = Console.ReadLine().ToUpper();
            }

            if (side.Equals("W"))
            {
                Console.WriteLine("You will be playing as White Player, good luck! ");
                WhiteIsComputer = false;
                BlackIsComputer = true;
            }
            else
            {
                Console.WriteLine("You will be playing as Black Player, good luck! ");
                BlackIsComputer = false;
                WhiteIsComputer = true;
            }
            Console.WriteLine("Board size is set to 8x8\nPress any key to continue...");
            Console.ReadLine();
            Console.Clear();
        }

        // METHOD:  InitializeBoard
        // PURPOSE: Initialize an empty board
        static void InitializeBoard()
        {
            for (var i = 0; i < boardSize; i++)
            {
                for (var j = 0; j < boardSize; j++)
                    board[i, j] = " ";
            }
        }

        // METHOD:  DrawBoard
        // PURPOSE: Draw proper board design
        static void DrawBoard()
        {
            // Number columns
            Console.Write("\n  ");
            for (var x = 1; x <= Convert.ToInt16(boardSize); x++)
            {
                Console.Write(x + " ");
            }
            Console.WriteLine(" ");

            // Alphabet rows and board
            for (var i = 0; i < Convert.ToInt16(boardSize); i++)
            {
                Console.Write(Alphabet[i] + "|");
                for (var j = 0; j < Convert.ToInt16(boardSize); j++)
                {
                    Console.Write(board[i, j] + "|");
                }
                Console.WriteLine(" ");
            }
            Console.WriteLine(" ");
        }

        // METHOD:  TakeTurn
        // PURPOSE: Take turns for White and Black players
        static void TakeTurn()
        {
            bool isValid = true;

            // White player's turn
            if (isWhite)
            {
                if (WhiteIsComputer)
                {
                    Console.WriteLine("Player: " + Players.White);
                    int[] CurrentBestMove = Minimax(2);
                    bestConfigAtLevel1[0] = (isWhite) ? Int32.MinValue : Int32.MaxValue;
                    bestConfigAtRoot[0] = (isWhite) ? Int32.MaxValue : Int32.MinValue;
                    isValid = false;
                    isWhite = false;
                    board[CurrentBestMove[1], CurrentBestMove[2]] = "O";
                    board[CurrentBestMove[1] + 1, CurrentBestMove[2]] = "O";
                    DrawBoard();
                }
                else
                {
                    Console.WriteLine("Player: " + Players.White);
                    Console.WriteLine("Please enter your coordinate: ");
                    String whiteCoordinate = Console.ReadLine().ToUpper();

                    while (isValid)
                    {
                        if (whiteCoordinate.Length == 2 && Char.IsDigit(whiteCoordinate[1]))
                        {
                            int row = Array.IndexOf(Alphabet, whiteCoordinate.First()); // Convert x-coordinate from alpha to number
                            int col = int.Parse(whiteCoordinate[1].ToString()) - 1; // Convert y-coordinate - 1 due to index

                            // Check first character to find row and within bounds
                            // Check second character for column is within bounds
                            if ((Alphabet.Contains(whiteCoordinate[0]) && (row >= 0 && row < boardSize - 1))
                                && (col >= 0 && col < boardSize))
                            {
                                Console.WriteLine("heuristicn now: " + EvalHeuristic());
                                // Check if both cells are available
                                if (board[row, col] == " " && board[row + 1, col] == " ")
                                {
                                    board[row, col] = "O";
                                    board[row + 1, col] = "O";
                                    isValid = false;
                                    isWhite = false;
                                }
                                else // Selected cell is blocked
                                {
                                    Console.WriteLine("Warning! You cannot enter this cell");
                                    whiteCoordinate = Console.ReadLine().ToUpper();
                                    isValid = true;
                                }
                            }
                            // Invalid coordinate
                            else
                            {
                                Console.WriteLine("Invalid coordinate, please re-enter: ");
                                whiteCoordinate = Console.ReadLine().ToUpper();
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please enter two characters");
                            whiteCoordinate = Console.ReadLine().ToUpper();
                            continue;
                        }
                        DrawBoard();
                    } // End while 
                }//end of else
            }//end turn =isWhite

            // Black Player's turn
            else
            {
                if (BlackIsComputer)
                {
                    Console.WriteLine("Player: " + Players.Black);
                    int[] CurrentBestMove = Minimax(2);
                    bestConfigAtLevel1[0] = (isWhite) ? Int32.MinValue : Int32.MaxValue;
                    bestConfigAtRoot[0] = (isWhite) ? Int32.MaxValue : Int32.MinValue;
                    isValid = false;
                    isWhite = true;
                    board[CurrentBestMove[1], CurrentBestMove[2]] = "X";
                    board[CurrentBestMove[1], CurrentBestMove[2] + 1] = "X";
                    DrawBoard();
                }
                else
                {
                    Console.WriteLine("Player: " + Players.Black);
                    Console.WriteLine("Please enter your coordinate: ");
                    String blackCoordinate = Console.ReadLine().ToUpper();

                    //Black Player cannot enter the rightmost column
                    while (isValid)
                    {
                        if (blackCoordinate.Length == 2 && Char.IsDigit(blackCoordinate[1]))
                        {
                            int row = Array.IndexOf(Alphabet, blackCoordinate.First()); // Convert x-coordinate from alpha to number
                            int col = int.Parse(blackCoordinate[1].ToString()) - 1; // Convert y-coordinate - 1 due to index

                            // Check first character to find row
                            // Check second character for column is within bounds
                            if ((Alphabet.Contains(blackCoordinate[0]) && (row >= 0 && row < boardSize))
                                && (col >= 0 && col < boardSize - 1))
                            {
                                Console.WriteLine("heuristicn now: " + EvalHeuristic());
                                // Check if both cells are available
                                if (board[row, col] == " " && board[row, col + 1] == " ")
                                {
                                    board[row, col] = "X";
                                    board[row, col + 1] = "X";
                                    isValid = false;
                                    isWhite = true;
                                }
                                else // Selected cell is blocked
                                {
                                    Console.WriteLine("Warning! You cannot enter this cell");
                                    blackCoordinate = Console.ReadLine().ToUpper();
                                    isValid = true;
                                }
                            }
                            //invalid coordinate
                            else
                            {
                                Console.WriteLine("Invalid coordinate, please re-enter: ");
                                blackCoordinate = Console.ReadLine().ToUpper();
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Please enter three characters");
                            blackCoordinate = Console.ReadLine().ToUpper();
                            continue;
                        }
                        DrawBoard();
                    }
                }//end of inner else            
            }//end of outer else          

        }//end TakeTurn()

        // METHOD:  IsGameOver
        // PURPOSE: Checks whether the game is over.
        static bool IsGameOver()
        {
            if (isWhite)
            {
                for (var i = 0; i < boardSize - 1; i++)
                {
                    for (var j = 0; j < boardSize; j++)
                    {
                        // Find spot for White player.
                        if (board[i, j] == " " && board[i + 1, j] == " ")
                            return false;
                        else continue;
                    }
                }

                return true;
            }
            else
            {
                for (var i = 0; i < boardSize; i++)
                {
                    for (var j = 0; j < boardSize - 1; j++)
                    {
                        // Find spot for Black player.
                        if (board[i, j] == " " && board[i, j + 1] == " ")
                            return false;
                        else continue;
                    }
                }
                return true;
            }

        }//end isGameOver()

        //heuristic function for each configuration
        static int EvalHeuristic()
        {
            int totalScore = 0, whiteScore = 0, blackScore = 0;

            if (isWhite)
            {
                for (var i = 0; i < boardSize - 1; i++)
                {
                    for (var j = 0; j < boardSize; j++)
                    {
                        if ((board[i, j] == " " && board[i + 1, j] == " "))
                            whiteScore++;
                    }
                }
            }
            for (var i = 0; i < boardSize; i++)
            {
                for (var j = 0; j < boardSize - 1; j++)
                {
                    if ((board[i, j] == " " && board[i, j + 1] == " "))
                        blackScore--;
                }
            }
            totalScore = whiteScore + blackScore;
            return totalScore;
        }


        static int[] Minimax(int depth)
        {
            int currentScore = 0;

            /*   if (depth == 0 || IsGameOver())
               {
                   bestScore = EvalHeuristic();
                   Console.WriteLine("depth is now 0, and score = " + bestScore);
               }*/
            // else
            // {
            if (isWhite)
            {
                for (int i = 0; i < boardSize - 1; i++)
                {
                    for (int j = 0; j < boardSize; j++)
                    {
                        // try this move
                        if (board[i, j] == " " && board[i + 1, j] == " ")
                        {
                            board[i, j] = "O"; board[i + 1, j] = "O"; //DrawBoard(); 
                            currentScore = EvalHeuristic();
                            if (depth == 2)
                            {
                                level1.Add(new int[] { currentScore, i, j });//store all configurations at level1
                                isWhite = false;
                                Minimax(depth - 1);
                                if (bestConfigAtLevel1[0] > bestConfigAtRoot[0])
                                {
                                    bestConfigAtRoot[0] = bestConfigAtLevel1[0];
                                    bestConfigAtRoot[1] = bestConfigAtLevel1[1];
                                    bestConfigAtRoot[2] = bestConfigAtLevel1[2];
                                }
                                board[i, j] = " "; board[i + 1, j] = " ";//undo
                                                                         //    DrawBoard();
                            }
                            else if (depth == 1)
                            {
                                if (currentScore > bestConfigAtLevel1[0])
                                {
                                    bestConfigAtLevel1[0] = currentScore;
                                    bestConfigAtLevel1[1] = i;
                                    bestConfigAtLevel1[2] = j;
                                }
                                board[i, j] = " "; board[i + 1, j] = " ";//undo
                                                                         //    DrawBoard();
                            }
                        }
                    }
                }
                return bestConfigAtRoot;
            }
            else
            {
                //try all possible moves 
                for (int i = 0; i < boardSize; i++)
                {
                    for (int j = 0; j < boardSize - 1; j++)
                    {
                        // try this move
                        if (board[i, j] == " " && board[i, j + 1] == " ")
                        {
                            board[i, j] = "X"; board[i, j + 1] = "X";// DrawBoard();                        
                            currentScore = EvalHeuristic();
                            if (depth == 2)
                            {
                                isWhite = true;
                                Minimax(depth - 1);
                                if (bestConfigAtLevel1[0] < bestConfigAtRoot[0])
                                {
                                    bestConfigAtRoot[0] = bestConfigAtLevel1[0];
                                    bestConfigAtRoot[1] = bestConfigAtLevel1[1];
                                    bestConfigAtRoot[2] = bestConfigAtLevel1[2];
                                }
                                board[i, j] = " "; board[i, j + 1] = " ";//undo
                                                                         //  DrawBoard();
                            }
                            else if (depth == 1)
                            {
                                if (currentScore < bestConfigAtLevel1[0])
                                {
                                    bestConfigAtLevel1[0] = currentScore;
                                    bestConfigAtLevel1[1] = i;
                                    bestConfigAtLevel1[2] = j;
                                }
                                board[i, j] = " "; board[i, j + 1] = " ";//undo
                                                                         //   DrawBoard();
                            }
                        }
                    }
                }
                return bestConfigAtRoot;
            }

        }
    }
}
