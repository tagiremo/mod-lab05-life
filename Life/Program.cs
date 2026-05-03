using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        {
            IsAlive = IsAliveNext;
        }

        public Cell(bool isAlive = false)
        {
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
        private Cell[,] cells;
        private readonly int rows;
        private readonly int columns;

        public int Rows => rows;
        public int Columns => columns;

        public Board(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            cells = new Cell[rows, columns];
            
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    cells[row, col] = new Cell();
                }
            }
            
            ConnectNeighbors();
        }
        
        public void SetAlive(int row, int col)
        {
            if (IsInside(row, col))
            {
                cells[row, col].IsAlive = true;
            }
        }
        
        public void SetDead(int row, int col)
        {
            if (IsInside(row, col))
            {
                cells[row, col].IsAlive = false;
            }
        }
        
        public bool IsAlive(int row, int col)
        {
            return IsInside(row, col) && cells[row, col].IsAlive;
        }
        
        public void Randomize(double liveDensity)
        {
            Random rand = new Random();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    cells[row, col].IsAlive = rand.NextDouble() < liveDensity;
                }
            }
        }
        
        public void Reset()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    cells[row, col].IsAlive = false;
                }
            }
        }
        
        private void ConnectNeighbors()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    for (int dr = -1; dr <= 1; dr++)
                    {
                        for (int dc = -1; dc <= 1; dc++)
                        {
                            if (dr == 0 && dc == 0) continue;
                            
                            int nr = row + dr;
                            int nc = col + dc;
                            
                            if (IsInside(nr, nc))
                            {
                                cells[row, col].neighbors.Add(cells[nr, nc]);
                            }
                        }
                    }
                }
            }
        }
        
        public int CountAlive()
        {
            int count = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (cells[row, col].IsAlive)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        
        public int CountNeighbors(int row, int col)
        {
            int count = 0;
            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0) continue;
                    
                    int nr = row + dr;
                    int nc = col + dc;
                    
                    if (IsInside(nr, nc) && cells[nr, nc].IsAlive)
                    {
                        count++;
                    }
                }
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
        
        public void Advance()
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    cells[row, col].DetermineNextLiveState();
                }
            }
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    cells[row, col].Advance();
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
