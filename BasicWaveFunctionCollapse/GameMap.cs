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
                    states[j] = new Tile(new Position(i, j), initState.Copy());
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
        public Dictionary<Direction, Tile> GetNeighborTiles(Position pos)
        {
            Dictionary<Direction, Tile> neighbors = new();

            neighbors.Add(Direction.Up, pos.Y > 0 ? 
                Grid[pos.Y - 1][pos.X] : 
                new Tile(pos, State.EmptyState));

            neighbors.Add(Direction.Down, pos.Y < MapSize - 1 ? 
                Grid[pos.Y + 1][pos.X] : 
                new Tile(pos, State.EmptyState));

            neighbors.Add(Direction.Left, pos.X > 0 ? 
                Grid[pos.Y][pos.X - 1] : 
                new Tile(pos, State.EmptyState));

            neighbors.Add(Direction.Right, pos.X < MapSize - 1 ? 
                Grid[pos.Y][pos.X + 1] : 
                new Tile(pos, State.EmptyState));

            return neighbors;
        }
    }
}