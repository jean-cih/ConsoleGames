using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

namespace Pacman
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string nameMap;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Enter the name of file.txt to draw any map: ");
            nameMap = Console.ReadLine();
            char[,] mapGame = ReadMap(nameMap);

            bool isOpen = true;
            bool isClosed;
            int pacManX = 2, pacManY = 3;
            int score = 0;
            int enemyX = 20, enemyY = 5;
            bool speed = true;
            bool dead = true;

            Console.CursorVisible = false;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkBlue;

            ConsoleKeyInfo pressKey = new ConsoleKeyInfo();
            
            Task.Run(() =>
            {
                while (true)
                {
                    if (pacManX == enemyX && pacManY == enemyY)
                    {
                        ConsoleColor loseColor = Console.ForegroundColor;

                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.SetCursorPosition(pacManX, pacManY);
                        Console.WriteLine("x");
                        Console.SetCursorPosition(5, 16);
                        Console.Write("Unfortunately You lose :(");
                        Console.ForegroundColor = loseColor;
                        Console.SetCursorPosition(0, 0);
                        dead = false;
                    }
                    if (enemyX <= 41 && speed)
                    {
                        if(enemyX == 41)
                        {
                            speed = false;
                            continue;
                        }
                        enemyX++;
                    }
                    else if(enemyX >= 2 && !speed)
                    {
                        if(enemyX == 2)
                        {
                            speed = true;
                            continue;
                        }
                        enemyX--;
                    }
                   
                    Thread.Sleep(200);
                }
            });

            Task.Run(() =>
            {
                while (true)
                {
                    pressKey = Console.ReadKey();
                }
            });
               
            while (isOpen && dead)
            {
                Console.Clear();
                
                isClosed = HandleInput(pressKey, ref pacManX, ref pacManY, mapGame, ref score);
                isOpen = isClosed;

                ConsoleColor defaultColor = Console.ForegroundColor;
                DrawMap(mapGame);

                Console.SetCursorPosition(enemyX, enemyY);
                ConsoleColor backColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("&");
                Console.ForegroundColor = backColor;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.SetCursorPosition(pacManX, pacManY);
                Console.Write("@");

                Console.SetCursorPosition(1, 14);
                Console.Write($"Score = {score}");
                    
                Console.ForegroundColor = defaultColor;


                Thread.Sleep(100);
            }
            Console.ReadKey();
        }
        private static char[,] ReadMap(string path)
        {
            string[] file = File.ReadAllLines(path);

            char[,] mapGame = new char[GetMaxLengthOfLine(file), file.Length];

            for (int i = 0; i < mapGame.GetLength(0); i++)
            {
                for (int j = 0; j < mapGame.GetLength(1); j++)
                {
                    mapGame[i, j] = file[j][i];
                }
            }
            return mapGame;
        }
        private static int GetMaxLengthOfLine(string[] lines)
        {
            int MaxLength = lines[0].Length;

            foreach(var line in lines)
                if(line.Length > MaxLength)
                    MaxLength = line.Length;

            return MaxLength;
        }
        private static void DrawMap(char[,] map)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            for (int i = 0; i < map.GetLength(1); i++)
            { 
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    if (map[j, i] == '*')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else
                    {
                        Console.ForegroundColor = defaultColor;
                    }
                    Console.Write(map[j, i]);
                }
                Console.Write("\n");
            }
        }
        private static bool HandleInput(ConsoleKeyInfo pressKey, ref int pacManX, ref int pacManY, char[,] mapGame, ref int score)
        {
            bool isClosed = true;
            int[] direction = GetDirection(pressKey);

            int nextPacmanPositionX = pacManX + direction[0];
            int nextPacmanPositionY = pacManY + direction[1];
            
            if(mapGame[nextPacmanPositionX, nextPacmanPositionY] == '*')
            {
                mapGame[nextPacmanPositionX, nextPacmanPositionY] = ' ';
                score++;
            }
            if ((pacManX == 21 || pacManX == 22) && pacManY == 1)
            {
                Console.SetCursorPosition(5, 16);
                ConsoleColor defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("You have just gone out, Good bye");
                Console.ForegroundColor = defaultColor;
                Console.SetCursorPosition(0, 0);
                isClosed = false;
            }
            if (score == 9)
            {
                ConsoleColor defaultColor = Console.ForegroundColor;
                
                Console.SetCursorPosition(5, 16);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You collected the bunch of Strawberry");
                Console.SetCursorPosition(15, 17);
                Console.WriteLine("You won!!!");
                Console.SetCursorPosition(0, 0);

                Console.ForegroundColor = defaultColor;

                isClosed = false;
            }
            if (mapGame[nextPacmanPositionX, nextPacmanPositionY] != '#')
            {
                pacManX = nextPacmanPositionX;
                pacManY = nextPacmanPositionY;
            }
            return isClosed;
        } 
        private static int[] GetDirection(ConsoleKeyInfo pressKey)
        {
            int[] direction = { 0, 0 };

            if (pressKey.Key == ConsoleKey.UpArrow)
                direction[1] = -1;
            else if (pressKey.Key == ConsoleKey.DownArrow)
                direction[1] = 1;
            else if (pressKey.Key == ConsoleKey.LeftArrow)
                direction[0] = -1;
            else if (pressKey.Key == ConsoleKey.RightArrow)
                direction[0] = 1;

            return direction;
        }
    }
}
