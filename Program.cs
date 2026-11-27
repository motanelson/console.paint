using System;
using System.IO;
class paint
{ 
const int GRID = 16;

static byte[,] pixels = new byte[GRID, GRID];

static int cursorX = 0;
static int cursorY = 0;
static int currentColor = 15;


static string fileName = "0:\\art.csv";
    static ConsoleColor[] palette =
        {
        ConsoleColor.Black,
        ConsoleColor.DarkBlue,
        ConsoleColor.DarkGreen,
        ConsoleColor.DarkCyan,
        ConsoleColor.DarkRed,
        ConsoleColor.DarkMagenta,
        ConsoleColor.DarkYellow,
        ConsoleColor.Gray,

        ConsoleColor.DarkGray,
        ConsoleColor.Blue,
        ConsoleColor.Green,
        ConsoleColor.Cyan,
        ConsoleColor.Red,
        ConsoleColor.Magenta,
        ConsoleColor.Yellow,
        ConsoleColor.White
    };



    public static void Main()
{
        Console.WindowHeight = 80;
        Console.WindowWidth = 81;
        Console.SetWindowSize(80,80);
        Console.BackgroundColor = ConsoleColor.White;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Clear();
    Console.WriteLine("give me the input file ? ");
    fileName = Console.ReadLine();
    DrawInterface();
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.W:
                    if (cursorY > 0) cursorY--;
                    break;

                case ConsoleKey.S:
                    if (cursorY < GRID - 1) cursorY++;
                    break;

                case ConsoleKey.A:
                    if (cursorX > 0) cursorX--;
                    break;

                case ConsoleKey.D:
                    if (cursorX < GRID - 1) cursorX++;
                    break;

                case ConsoleKey.Spacebar:
                    pixels[cursorX, cursorY] = (byte)currentColor;
                    break;

                case ConsoleKey.C:
                    ClearPixels();
                    break;

                case ConsoleKey.G:
                    SaveFile();
                    break;

                case ConsoleKey.L:
                    LoadFile();
                    break;

                case ConsoleKey.Escape:
                    Console.Clear();
                    Console.WriteLine("Painter terminated.");
                    return;
                    break;

                default:
                    SelectColor(key);
                    break;
            }

            DrawInterface();
        }
}

static void SelectColor(ConsoleKeyInfo key)
{
    char c = key.KeyChar;

    if (c >= '0' && c <= '9')
    {
        currentColor = c - '0';
    }
    else if (c >= 'a' && c <= 'f')
    {
        currentColor = 10 + c - 'a';
    }
    else if (c >= 'A' && c <= 'F')
    {
        currentColor = 10 + c - 'A';
    }

    if (currentColor > 15)
        currentColor = 15;
}

static void ClearPixels()
{
    for (int y = 0; y < GRID; y++)
    {
        for (int x = 0; x < GRID; x++)
        {
            pixels[x, y] = 0;
        }
    }
}

static void DrawInterface()
{
    

    Console.BackgroundColor = ConsoleColor.White;
    Console.ForegroundColor = ConsoleColor.Black;
    Console.Clear();

    Console.SetCursorPosition(0, 0);
    Console.WriteLine("VGA PIXEL PAINTER 32x32");
    Console.WriteLine();

    Console.WriteLine("Palette (0-F):");

    for (int i = 0; i < 16; i++)
    {
        Console.BackgroundColor = palette[i];
        Console.ForegroundColor = ConsoleColor.Black;

        Console.Write(" " + i.ToString("X") + " ");
    }

    Console.BackgroundColor = ConsoleColor.White;
    Console.ForegroundColor = ConsoleColor.Black;

    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine("Drawing area:");

    for (int y = 0; y < GRID; y++)
    {
        Console.SetCursorPosition(2, 4 + y);

        for (int x = 0; x < GRID; x++)
        {
            int color = pixels[x, y];

            if (x == cursorX && y == cursorY)
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("[]");
            }
            else
            {
                Console.BackgroundColor = palette[color];
                Console.ForegroundColor = palette[color];
                Console.Write("  ");
            }
        }

        Console.BackgroundColor = ConsoleColor.White;
        Console.ForegroundColor = ConsoleColor.Black;
    }

    Console.SetCursorPosition(0, 18);

    Console.WriteLine("WASD: Move  SPACE: Paint  0-F: Color");
    Console.WriteLine("C: Clear  G: Save  L: Load  ESC: Exit");

    Console.WriteLine();
    Console.Write("Selected color: ");

    Console.BackgroundColor = palette[currentColor];
    Console.ForegroundColor = palette[currentColor];

    Console.Write("  ");

    Console.BackgroundColor = ConsoleColor.White;
    Console.ForegroundColor = ConsoleColor.Black;

    Console.WriteLine(" " + currentColor.ToString("X"));
}

 static void SaveFile()
{
    Console.BackgroundColor = ConsoleColor.White;
    Console.ForegroundColor = ConsoleColor.Black;
        Console.Clear();
    //Console.SetCursorPosition(0, 20);
    Console.Write("Filename: ");

    string name = Console.ReadLine();

    if (name == null || name == "")
        name = fileName;

    fileName = name;

    try
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            for (int y = 0; y < GRID; y++)
            {
                for (int x = 0; x < GRID; x++)
                {
                    writer.Write(pixels[x, y]);

                    if (x < GRID - 1)
                        writer.Write(",");
                }

                writer.WriteLine();
            }
        }

        //Console.SetCursorPosition(0, 23);
        Console.WriteLine("Saved successfully.");
            return;
    }
    catch
    {
        //Console.SetCursorPosition(0, 43);
        Console.WriteLine("Error saving file.");
    }
}

static void LoadFile()
{
    Console.BackgroundColor = ConsoleColor.White;
    Console.ForegroundColor = ConsoleColor.Black;

    Console.SetCursorPosition(0,23);
    Console.Write("Filename: ");

    string name = Console.ReadLine();

    if (name == null || name == "")
        name = fileName;

    fileName = name;

    try
    {
        string[] rows = File.ReadAllLines(fileName);

        if (rows.Length != GRID)
            throw new Exception();

        for (int y = 0; y < GRID; y++)
        {
            string[] values = rows[y].Split(',');

            if (values.Length != GRID)
                throw new Exception();

            for (int x = 0; x < GRID; x++)
            {
                pixels[x, y] = byte.Parse(values[x]);
            }
        }

        Console.SetCursorPosition(0, 23);
        Console.WriteLine("Loaded successfully.");
    }
    catch
    {
        Console.SetCursorPosition(0, 23);
        Console.WriteLine("Error loading file.");
    }
}
}