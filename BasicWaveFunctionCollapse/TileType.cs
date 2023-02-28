using System;
using System.Collections.Generic;

namespace BasicWaveFunctionCollapse
{
    public class TileType
    {
        public string Value { get; set; }
        public List<TileType> Up { get; set; }
        public List<TileType> Down { get; set; }
        public List<TileType> Right { get; set; }
        public List<TileType> Left { get; set; }

        public TileType(string value)
        {
            Value = value;
        }
        
        private TileType(string value, List<TileType> up, List<TileType> down, List<TileType> right, List<TileType> left)
        {
            Value = value;
            Up = up;
            Down = down;
            Right = right;
            Left = left;
        }
        public TileType Copy() {
            return new TileType(Value, Up, Down, Right, Left);
        }

    }

}