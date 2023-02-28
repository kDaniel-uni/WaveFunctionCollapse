// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Linq;
using BasicWaveFunctionCollapse;

//Console.WriteLine($"{TileType.O.Value}");

const int MapSize = 16;

TileType OTile = new TileType("O");
TileType XTile = new TileType("X");
TileType TTile = new TileType("T");

TileType[] AllTilesArray = { OTile, XTile, TTile };
List<TileType> AllTiles = AllTilesArray.ToList();
//TileType[] OTileDown = { OTile, XTile, TTile };
//TileType[] OTileRight = { OTile, XTile, TTile };
//TileType[] OTileLeft = { OTile, XTile, TTile };

OTile.Up = AllTiles;
OTile.Down = AllTiles;
OTile.Right = AllTiles;
OTile.Left = AllTiles.ToList();

XTile = OTile.Copy();
TTile = OTile.Copy();

State baseState = new(AllTiles.ToList());


// init tile types
// TODO: -^

// init 2D map with all superposed states
Tile[][] map = CreateNewMap(baseState);

//bool allCollapsed = false;
int collapsedTiles = 0;

// while map not stable
while (collapsedTiles < Math.Pow(MapSize, 2)) {
    
    Position pos = FindStateWithHighestEntropy();
    State currentState = map[pos.Y][pos.X].State;

    currentState.Collapse();
    collapsedTiles++;

    Dictionary<Position, bool> updated = new();
    Queue<Tile> queue = new();

    foreach (var neighborTile in GetNeighborTiles(map, pos))
    {
        queue.Enqueue(neighborTile.Value);
    }

    while (queue.Count > 0) {
            Tile tile = queue.Dequeue();
            Dictionary<DirectionType, Tile> neighbors = GetNeighborTiles(map, tile.Position);

            var collapsed = tile.State.Update(neighbors[DirectionType.Up].State, 
                neighbors[DirectionType.Down].State, 
                neighbors[DirectionType.Right].State,
                neighbors[DirectionType.Left].State);

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

    // High time complexity especially for 3D or big maps
    /*allCollapsed = true;
    for (int y = 0; y < MapSize; y++)
    {
        for (int x = 0; x < MapSize; x++)
        {
            if (!map[y][x].State.IsCollapsed()) {
                allCollapsed = false;
                goto here;
            }
        }
    }
    here:;*/
}

Tile[][] CreateNewMap(State initState)
{
    Tile[][] map = new Tile[MapSize][];
    for (int i = 0; i < MapSize; i++)
    {
        Tile[] states = new Tile[MapSize];
        for (int j = 0; j < MapSize; j++)
        {
            states[j] = new Tile(new Position(i,j), initState);
        }
        map[i] = states;
    }

    return map;
}

Position FindStateWithHighestEntropy()
{
    return new Position(new Random().Next(0, MapSize), new Random().Next(0, MapSize));
}

// Get the neighbors that are inside the map
// Usage of Dictionary to allow sparse storage of tiles and access to iterator
// Fill the empty directions with empty states
Dictionary<DirectionType, Tile> GetNeighborTiles(Tile[][] map, Position pos)
{
    Dictionary<DirectionType, Tile> neighbors = new();

    neighbors.Add(DirectionType.Up, pos.Y > 0 ? 
        map[pos.Y - 1][pos.X] : 
        new Tile(pos, State.NullState));

    neighbors.Add(DirectionType.Down, pos.Y < MapSize ? 
        map[pos.Y + 1][pos.X] : 
        new Tile(pos, State.NullState));

    neighbors.Add(DirectionType.Left, pos.X > 0 ? 
        map[pos.Y][pos.X - 1] : 
        new Tile(pos, State.NullState));

    neighbors.Add(DirectionType.Right, pos.X < MapSize ? 
        map[pos.Y][pos.X + 1] : 
        new Tile(pos, State.NullState));

    return neighbors;
}