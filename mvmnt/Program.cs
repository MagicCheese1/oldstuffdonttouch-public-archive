using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace mvmnt {
    class Program {

        //Console.SetCursorPosition(10*2-1,3);
        // Console.Write("s");

        static int[, ] grid, dgrid;
        static int px = 3, py = 4;
        static int currentlevel = 0;
        static bool finished = false;
        static void Main (string[] args) {
            Start : Console.Clear ();
            List<string> levelList = new List<string> ();
            string[] Levels = Directory.GetFiles (System.AppDomain.CurrentDomain.BaseDirectory + "\\LEVELS", "*.MLevel", SearchOption.TopDirectoryOnly);
            foreach (var level in Levels) {
                levelList.Add (File.ReadAllText (level));
            }
            //bool canswr = false;
            //while (canswr == false) {
            //    Console.WriteLine ("Found {0} levels!", levelList.Count);
            //    Console.WriteLine ("which level do you want to load?");
            //    string Input = Console.ReadLine ();
            //    int inputint;
            //    if (Int32.TryParse (Input, out inputint)) {
            //        inputint--;
            //        if (levelList.Count > inputint) {
            //            canswr = true;
            //            currentlevel = inputint;
            //        } else {
            //            Console.WriteLine ("This answer is not correct because such level doesn't exist!");
            //        }
            //
            //    } else {
            //        Console.WriteLine ("This answer is not correct because it's not a number!");
            //    }
            //}
            if (currentlevel >= levelList.Count) {
                Console.WriteLine ("you finished lol");
                Console.ReadLine ();
                return;
            }
            Initlevel (levelList.ElementAt (currentlevel));
            ConsoleKeyInfo keyinfo;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;

            do {
                keyinfo = Console.ReadKey (true);
                if (keyinfo.Key == ConsoleKey.UpArrow) {
                    CheckForCollision (1);

                }
                if (keyinfo.Key == ConsoleKey.DownArrow) {
                    CheckForCollision (2);

                }
                if (keyinfo.Key == ConsoleKey.RightArrow) {
                    CheckForCollision (3);
                }
                if (keyinfo.Key == ConsoleKey.LeftArrow) {
                    CheckForCollision (4);
                }
                if (keyinfo.Key == ConsoleKey.Escape) {
                    Console.SetCursorPosition (0, 12);
                    Console.Write ("press R if you want to reload\npress X if you want to Exit \npress S if you want to go back");
                    var inpt = Console.ReadKey ();
                    if (inpt.Key == ConsoleKey.S) {
                        Console.SetCursorPosition (0, 12);
                        Console.Write ("                                    \n                                           \n                                              ");
                    } else if (inpt.Key == ConsoleKey.R) {
                        goto Start;
                    } else if (inpt.Key == ConsoleKey.X) {
                        Console.Clear ();
                        Console.ReadKey ();
                        return;
                    }
                }
                if (finished) {
                    finished = false;
                    goto Start;
                }

            } while (true);
        }

        static void TileDrawer (int x, int y, int[, ] gridb) {
            Console.SetCursorPosition (y * 2, x);
            if (gridb[x, y] == 0 || gridb[x, y] == 10)
                Console.Write ("  ");
            else if (gridb[x, y] == 1)
                Console.Write ("# ");
            else if (gridb[x, y] == 11) {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write ("@ ");
                Console.ForegroundColor = ConsoleColor.White;
            } else if (gridb[x, y] == 12) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write ('>');
                Console.ForegroundColor = ConsoleColor.White;
            }

        }

        static void Initlevel (string Leveldata) {
            grid = new int[15, 22];
            dgrid = new int[15, 22];
            int i = 0, j = 0, g = 0;
            Console.Clear ();
            foreach (var row in Leveldata.Split ('\n')) {
                j = 0;
                g++;
                foreach (var col in row.Trim ().Split (' ')) {
                    dgrid[i, j] = int.Parse (col.Trim ());
                    grid[i, j] = dgrid[i, j];
                    if (dgrid[i, j] == 10) {
                        px = i;
                        py = j;
                        grid[i, j] = 11;
                    }

                    TileDrawer (i, j, grid);
                    j++;
                }
                Console.SetCursorPosition (0, g);
                i++;
            }
        }

        static void CheckForCollision (int way) {
            switch (way) {
                case 1:
                    if (grid[py - 1, px] != 12) {
                        if (grid[py - 1, px] == 0 || grid[py - 1, px] > 9 && grid[py - 1, px] != 12) {
                            TileDrawer (py, px, dgrid);
                            py--;
                            goto Here;
                        }
                    } else {
                        Finished ();
                    }
                    break;
                case 2:
                    if (grid[py + 1, px] != 12) {
                        if (grid[py + 1, px] == 0 || grid[py + 1, px] > 9 && grid[py + 1, px] != 12) {
                            TileDrawer (py, px, dgrid);
                            py++;
                            goto Here;
                        }
                    } else {
                        Finished ();
                    }
                    break;
                case 3:
                    if (grid[py, px + 1] != 12) {
                        if (grid[py, px + 1] == 0 || grid[py, px + 1] > 9 && grid[py, px + 1] != 12) {
                            TileDrawer (py, px, dgrid);
                            px++;
                            goto Here;
                        }
                    } else {
                        Finished ();
                    }
                    break;
                case 4:
                    if (dgrid[py, px - 1] != 12) {
                        if (grid[py, px - 1] == 0 || grid[py, px - 1] > 9 && grid[py, px - 1] != 12) {
                            TileDrawer (py, px, dgrid);
                            px--;
                            goto Here;
                        }
                    } else {
                        Finished ();
                    }
                    break;
            }
            return;
            Here:
                grid[py, px] = 11;
            TileDrawer (py, px, grid);
        }
        static void Finished () {
            currentlevel++;
            finished = true;
        }
    }
}

/* using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;

namespace mvmnt
{
    class Program
    {
        static byte MapHeight = 10;
        static byte MapWidth = 30;
        static byte[,] grid;

    //    static void Main(string[] args)
    //    {
    //    Console.OutputEncoding = Encoding.UTF8;
    //    Console.WriteLine("■■■■■■■■■■");
    //    Console.WriteLine("█ Hello  █");
    //    Console.WriteLine("█ world  █");
    //    Console.WriteLine("■■■■■■■■■■");
    //    Console.ReadLine();
    //    }
    //    static byte redo = 0;
    //    static byte playerHorizontal = 20;
    //    static byte playerVertical = 10;



        [DllImport("kernel32.dll", ExactSpelling = true)]

    private static extern IntPtr GetConsoleWindow();
    private static IntPtr ThisConsole = GetConsoleWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]

    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    private const byte HIDE = 0;
    private const byte MAXIMIZE = 3;
    private const byte MINIMIZE = 6;
    private const byte RESTORE = 9;


        static void Main(string[] args)
        {
             
             ShowWindow(ThisConsole, MAXIMIZE);
            Console.OutputEncoding = System.Text.Encoding.UTF8;
              Console.Clear();
                Render();
            ConsoleKeyInfo keyinfo;
            do
            {
            keyinfo = Console.ReadKey(true);
            
            switch(keyinfo.Key)
            {
                case ConsoleKey.LeftArrow:
                    playerHorizontal--;
                    break;
                case ConsoleKey.RightArrow:
                    playerHorizontal++;
                    break;
                case ConsoleKey.UpArrow:
                    playerVertical--;
                    break;
                case ConsoleKey.DownArrow:
                    playerVertical++;
                    break;
            }
            Render();
            }while(redo == 0);
            Console.ReadLine();
        }

        static void DrawMap()
        {
            byte g= 5;
            grid = new byte[MapWidth,MapHeight];
            
            Console.SetCursorPosition(10,g);
            for(byte x = 0; x < MapHeight; x++)
            {
                g++;
                for(byte y= 0; y < MapWidth; y++)
                {
                    if(grid[y,x] == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write('•');
                    }
                }
                Console.SetCursorPosition(10,g);
            }
        }
        static void Render()
        {
            DrawMap();
            Console.SetCursorPosition(playerHorizontal, playerVertical);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write('@');
        }
        static void DrawHorizontalLine(byte x, byte y, byte Length)
        {
            Console.SetCursorPosition(x, y);
            for(byte i = 1; i <= Length; i++)
            {
                 Console.Write("═");
            }
        }

        static void DrawVerticalLine(byte x, byte y, byte Length)
        {
            Length  /= 2;
            Console.SetCursorPosition(x, y);
            for(byte i = 1; i <= Length; i++)
            {
                 Console.Write("│");
                 Console.SetCursorPosition(x,y + i);

            }
        }

    }
}
*/