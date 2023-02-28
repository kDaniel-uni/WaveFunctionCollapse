using System;
using System.Collections.Generic;

namespace BasicWaveFunctionCollapse
{
    public class GameMap
    {
        public Tile[][] Grid;
        public int MapSize;
        public List<Position> UncollapsedPositions;
        public State InitState;

        public GameMap(State initState, int mapSize)
        {
            InitState = initState;
            MapSize = mapSize;
            Grid = new Tile[MapSize][];
            UncollapsedPositions = new();
            for (int i = 0; i < MapSize; i++)
            {
                for (int j = 0; j < MapSize; j++)
                {
                    UncollapsedPositions.Add(new Position(i, j));
                }
            }
            InitGrid();
        }
    
        private void InitGrid()
        {
            for (int i = 0; i < MapSize; i++)
            {
                Tile[] states = new Tile[MapSize];
                for (int j = 0; j < MapSize; j++)
                {
                    states[j] = new Tile(new Position(i,j), InitState);
                }
                Grid[i] = states;
            }
        }
    
    
        public Position CollapseTileWithHighestEntropy()
        {
            int index = new Random().Next(0, UncollapsedPositions.Count);
            Position pos = UncollapsedPositions[index];
            UncollapsedPositions.RemoveAt(index);
            Grid[pos.X][pos.Y].State.Collapse();

            return pos;
        }
    
    
        // Get the neighbors that are inside the map
        // Usage of Dictionary to allow sparse storage of tiles and access to iterator
        public Dictionary<DirectionType, Tile> GetNeighborTiles(Position pos)
        {
            Dictionary<DirectionType, Tile> neighbors = new();

            if (pos.Y > 0)
            {
                neighbors.Add(DirectionType.Up, Grid[pos.Y - 1][pos.X]);
            }
            
            if (pos.Y < MapSize - 1)
            {
                neighbors.Add(DirectionType.Down, Grid[pos.Y + 1][pos.X]);
            }
            
            if (pos.X > 0)
            {
                neighbors.Add(DirectionType.Left, Grid[pos.Y][pos.X - 1]);
            }
            
            if (pos.X < MapSize - 1)
            {
                neighbors.Add(DirectionType.Right, Grid[pos.Y][pos.X + 1]);
            }
            
            return neighbors;
        }

        public void PrintGrid()
        {
            String boundLine = "-";
                
            for (int i = 0; i < MapSize; i++)
            {
                boundLine += "-";
            }

            boundLine += "-";
            
            Console.WriteLine(boundLine);
            
            for (int j = 0; j < MapSize; j++)
            {
                String line = "|";
                
                for (int i = 0; i < MapSize; i++)
                {
                    if (Grid[i][j].State.Collapsed == null)
                    {
                        line += "?";
                        continue;
                    }
                    
                    line += Grid[i][j].State.Collapsed.Value;
                }

                line += "|";
                
                Console.WriteLine(line);
            }
            
            Console.WriteLine(boundLine);
        }
    }
}