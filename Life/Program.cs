using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.IO;
using System.Text.Json;

namespace cli_life
{
    public class Cell
    {
        public bool IsAlive;
        public readonly List<Cell> neighbors = new List<Cell>();
        private bool IsAliveNext;
        public void DetermineNextLiveState()
        {
            int liveNeighbors = neighbors.Where(x => x.IsAlive).Count();
            if (IsAlive)
                IsAliveNext = liveNeighbors == 2 || liveNeighbors == 3;
            else
                IsAliveNext = liveNeighbors == 3;
        }
        public void Advance()
        public bool IsAlive { get; set; }

        public Cell(bool isAlive = false)
        {
            IsAlive = IsAliveNext;
            IsAlive = isAlive;
        }
    }

    public class Config
    {
        public int Rows { get; set; } = 10;
        public int Columns { get; set; } = 10;
        public int Steps { get; set; } = 20;
        public double LiveDensity { get; set; } = 0.3;
        public string InputFile { get; set; } = "Data/figure.txt";
        public string OutputFile { get; set; } = "Data/data.txt";
    }

    public class Board
    {
        public readonly Cell[,] Cells;
        public readonly int CellSize;
        private readonly int rows;
        private readonly int columns;
        private Cell[,] cells;

        public int Columns { get { return Cells.GetLength(0); } }
        public int Rows { get { return Cells.GetLength(1); } }
        public int Width { get { return Columns * CellSize; } }
        public int Height { get { return Rows * CellSize; } }
        public int Rows => rows;
        public int Columns => columns;

        public Board(int width, int height, int cellSize, double liveDensity = .1)
        public Board(int rows, int columns)
        {
            CellSize = cellSize;
            this.rows = rows;
            this.columns = columns;
            cells = new Cell[rows, columns];

            Cells = new Cell[width / cellSize, height / cellSize];
            for (int x = 0; x < Columns; x++)
                for (int y = 0; y < Rows; y++)
                    Cells[x, y] = new Cell();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    cells[row, col] = new Cell();
                }
            }
        }

            ConnectNeighbors();
            Randomize(liveDensity);
        public void SetAlive(int row, int col)
        {
            if (IsInside(row, col))
            {
                cells[row, col].IsAlive = true;
            }
        }

        readonly Random rand = new Random();
        public void Randomize(double liveDensity)
        public void SetDead(int row, int col)
        {
            foreach (var cell in Cells)
                cell.IsAlive = rand.NextDouble() < liveDensity;
            if (IsInside(row, col))
            {
                cells[row, col].IsAlive = false;
            }
        }

        public void Advance()
        public bool IsAlive(int row, int col)
        {
            foreach (var cell in Cells)
                cell.DetermineNextLiveState();
            foreach (var cell in Cells)
                cell.Advance();
            return IsInside(row, col) && cells[row, col].IsAlive;
        }
        private void ConnectNeighbors()

        public int CountAlive()
        {
            for (int x = 0; x < Columns; x++)
            int count = 0;

            for (int row = 0; row < rows; row++)
            {
                for (int y = 0; y < Rows; y++)
                for (int col = 0; col < columns; col++)
                {
                    int xL = (x > 0) ? x - 1 : Columns - 1;
                    int xR = (x < Columns - 1) ? x + 1 : 0;

                    int yT = (y > 0) ? y - 1 : Rows - 1;
                    int yB = (y < Rows - 1) ? y + 1 : 0;

                    Cells[x, y].neighbors.Add(Cells[xL, yT]);
                    Cells[x, y].neighbors.Add(Cells[x, yT]);
                    Cells[x, y].neighbors.Add(Cells[xR, yT]);
                    Cells[x, y].neighbors.Add(Cells[xL, y]);
                    Cells[x, y].neighbors.Add(Cells[xR, y]);
                    Cells[x, y].neighbors.Add(Cells[xL, yB]);
                    Cells[x, y].neighbors.Add(Cells[x, yB]);
                    Cells[x, y].neighbors.Add(Cells[xR, yB]);
                    if (cells[row, col].IsAlive)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
    class Program
    {
        static Board board;
        static private void Reset()
        {
            board = new Board(
                width: 50,
                height: 20,
                cellSize: 1,
                liveDensity: 0.5);
        }
        static void Render()

        public int CountNeighbors(int row, int col)
        {
            for (int row = 0; row < board.Rows; row++)
            int count = 0;

            for (int dr = -1; dr <= 1; dr++)
            {
                for (int col = 0; col < board.Columns; col++)   
                for (int dc = -1; dc <= 1; dc++)
                {
                    var cell = board.Cells[col, row];
                    if (cell.IsAlive)
                    if (dr == 0 && dc == 0)
                    {
                        Console.Write('*');
                        continue;
                    }
                    else

                    int nr = row + dr;
                    int nc = col + dc;

                    if (IsInside(nr, nc) && cells[nr, nc].IsAlive)
                    {
                        Console.Write(' ');
                        count++;
                    }
                }
                Console.Write('\n');
            }

            return count;
        }

        public void NextGeneration()
        {
            Cell[,] next = new Cell[rows, columns];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    int neighbors = CountNeighbors(row, col);
                    bool alive = cells[row, col].IsAlive;

                    bool nextState = (alive && (neighbors == 2 || neighbors == 3)) ||
                                     (!alive && neighbors == 3);

                    next[row, col] = new Cell(nextState);
                }
            }

            cells = next;
        }
        static void Main(string[] args)

        public void Randomize(double liveDensity)
        {
            Reset();
            while(true)
            Random random = new Random(1);

            for (int row = 0; row < rows; row++)
            {
                Console.Clear();
                Render();
                board.Advance();
                Thread.Sleep(1000);
                for (int col = 0; col < columns; col++)
                {
                    cells[row, col].IsAlive = random.NextDouble() < liveDensity;
                }
            }
        }

        public void Save(string path)
        {
            using StreamWriter writer = new StreamWriter(path);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    writer.Write(cells[row, col].IsAlive ? '1' : '0');
                }

                writer.WriteLine();
            }
        }

        public void Load(string path)
        {
            string[] lines = File.ReadAllLines(path);

            for (int row = 0; row < rows && row < lines.Length; row++)
            {
                for (int col = 0; col < columns && col < lines[row].Length; col++)
                {
                    cells[row, col].IsAlive = lines[row][col] == '1';
                }
            }
        }

        public string ToText()
        {
            string result = "";

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    result += cells[row, col].IsAlive ? "*" : ".";
                }

                result += Environment.NewLine;
            }

            return result;
        }

        private bool IsInside(int row, int col)
        {
            return row >= 0 && row < rows && col >= 0 && col < columns;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Directory.CreateDirectory("Data");

            Config config = LoadConfig("Data/config.json");
            Board board = new Board(config.Rows, config.Columns);

            if (File.Exists(config.InputFile))
            {
                board.Load(config.InputFile);
            }
            else
            {
                board.Randomize(config.LiveDensity);
            }

            using StreamWriter writer = new StreamWriter(config.OutputFile);

            for (int step = 0; step <= config.Steps; step++)
            {
                int alive = board.CountAlive();
                writer.WriteLine($"{step};{alive}");

                Console.WriteLine($"Generation {step}");
                Console.WriteLine(board.ToText());
                Console.WriteLine($"Alive cells: {alive}");
                Console.WriteLine();

                board.NextGeneration();
            }

            File.WriteAllText("Data/plot.png", "Plot placeholder");
            Console.WriteLine("Simulation finished.");
        }

        private static Config LoadConfig(string path)
        {
            if (!File.Exists(path))
            {
                return new Config();
            }

            string json = File.ReadAllText(path);
            Config? config = JsonSerializer.Deserialize<Config>(json);
             return config ?? new Config();
        }
    }
}
}
