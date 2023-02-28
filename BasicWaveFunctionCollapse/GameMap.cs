using System;
using System.Collections.Generic;

namespace BasicWaveFunctionCollapse
{
    public class GameMap
    {
        public Tile[][] Grid;
        public int MapSize;

        public GameMap(State initState, int mapSize)
        {
            MapSize = mapSize;
            Grid = new Tile[MapSize][];
            InitGrid(initState);
        }
    
        private void InitGrid(State initState)
        {
            for (int i = 0; i < MapSize; i++)
            {
                Tile[] states = new Tile[MapSize];
                for (int j = 0; j < MapSize; j++)
                {
                    states[j] = new Tile(new Position(i,j), initState);
                }
                Grid[i] = states;
            }
        }
    
    
        public Tile FindTileWithHighestEntropy()
        {
            Position pos = new Position(new Random().Next(0, MapSize), new Random().Next(0, MapSize));
            return Grid[pos.X][pos.Y];
        }
    
    
        // Get the neighbors that are inside the map
        // Usage of Dictionary to allow sparse storage of tiles and access to iterator
        // Fill the empty directions with empty states
        public Dictionary<DirectionType, Tile> GetNeighborTiles(Position pos)
        {
            Dictionary<DirectionType, Tile> neighbors = new();

            neighbors.Add(DirectionType.Up, pos.Y > 0 ? 
                Grid[pos.Y - 1][pos.X] : 
                new Tile(pos, State.NullState));

            neighbors.Add(DirectionType.Down, pos.Y < MapSize ? 
                Grid[pos.Y + 1][pos.X] : 
                new Tile(pos, State.NullState));

            neighbors.Add(DirectionType.Left, pos.X > 0 ? 
                Grid[pos.Y][pos.X - 1] : 
                new Tile(pos, State.NullState));

            neighbors.Add(DirectionType.Right, pos.X < MapSize ? 
                Grid[pos.Y][pos.X + 1] : 
                new Tile(pos, State.NullState));

            return neighbors;
        }
    }
}