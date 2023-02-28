namespace BasicWaveFunctionCollapse
{
    public class Game
    {
        private GameMap _gameMap;
        private readonly List<TileType> _tileTypes;

        public Game()
        {
            TileType oTile = new TileType("O");
            TileType xTile = new TileType("X");
            TileType tTile = new TileType("T");
        
            _tileTypes = new (){ oTile, xTile, tTile };

            oTile.Up = _tileTypes;
            oTile.Down = _tileTypes;
            oTile.Right = _tileTypes;
            oTile.Left = _tileTypes;
        
            xTile.Up = _tileTypes;
            xTile.Down = _tileTypes;
            xTile.Right = _tileTypes;
            xTile.Left = _tileTypes;
        
            tTile.Up = _tileTypes;
            tTile.Down = _tileTypes;
            tTile.Right = _tileTypes;
            tTile.Left = _tileTypes;
        
            State baseState = new(_tileTypes);
            _gameMap = new GameMap(baseState ,16);
        }

        public void Solve()
        {
            int collapsedTiles = 0;
        
            while (collapsedTiles < Math.Pow(_gameMap.MapSize, 2)) {
    
                Tile currentTile = _gameMap.FindTileWithHighestEntropy();

                currentTile.State.Collapse();
                collapsedTiles++;

                Dictionary<Position, bool> updated = new();
                Queue<Tile> queue = new();

                foreach (var neighborTile in _gameMap.GetNeighborTiles(currentTile.Position))
                {
                    queue.Enqueue(neighborTile.Value);
                }

                while (queue.Count > 0) {
                    Tile tile = queue.Dequeue();
                    Dictionary<Direction, Tile> neighbors = _gameMap.GetNeighborTiles(tile.Position);

                    var collapsed = tile.State.Update(neighbors[Direction.Up].State, 
                        neighbors[Direction.Down].State, 
                        neighbors[Direction.Right].State,
                        neighbors[Direction.Left].State);

                    if (collapsed)
                    {
                        collapsedTiles++;
                    }
            
                    foreach (var neighbor in neighbors)
                    {
                        if (!updated.ContainsKey(neighbor.Value.Position))
                        {
                            queue.Enqueue(neighbor.Value);
                        }
                    }

                    updated.Add(tile.Position, true);
                }
            }
        }
    }
}