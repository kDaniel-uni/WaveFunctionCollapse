using System;
using System.Collections.Generic;

namespace BasicWaveFunctionCollapse
{
    public class Game
    {
        private GameMap _gameMap;
        private readonly List<TileType> _tileTypes;

        public Game()
        {
            TileType oTile = new ("O");
            TileType xTile = new ("X");
            TileType tTile = new ("T");
        
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
            _gameMap = new GameMap(baseState ,2);
        }

        public void Solve()
        {
            while (_gameMap.UncollapsedPositions.Count != 0) {
                
                Dictionary<Position, bool> updated = new();
                Queue<Tile> queue = new();
                
                Position currentPosition = _gameMap.CollapseTileWithHighestEntropy();
                updated.Add(currentPosition, true);

                foreach (var neighbor in _gameMap.GetNeighborTiles(currentPosition))
                {
                    queue.Enqueue(neighbor.Value);
                }

                while (queue.Count > 0) {
                    Tile tile = queue.Dequeue();
                    Dictionary<DirectionType, Tile> neighbors = _gameMap.GetNeighborTiles(tile.Position);

                    foreach (var neighbor in neighbors)
                    {
                        tile.State.UpdateDir(neighbor.Key, neighbor.Value.State);
                        
                        if (!updated.ContainsKey(neighbor.Value.Position) && !queue.Contains(neighbor.Value))
                        {
                            queue.Enqueue(neighbor.Value);
                        }
                    }

                    if (tile.State.IsCollapsed())
                    {
                        _gameMap.UncollapsedPositions.Remove(tile.Position);
                    }

                    updated.Add(tile.Position, true);
                }
                
                Console.Out.WriteLine($"UnCollapsed Tiles : {_gameMap.UncollapsedPositions.Count}");
                _gameMap.PrintGrid();
            }
        }
    }
}