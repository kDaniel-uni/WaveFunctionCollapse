using System;
using System.Collections.Generic;
using System.Data.Common;

namespace BasicWaveFunctionCollapse
{
    public class Tile
    {
            public Position Position { get; set; }
            public State State { get; set; }
    
            public Tile(Position position, State state)
            {
                Position = position;
                State = state;
            }
    }
    public class State
    {
        private List<TileType> SuperposedTiles { get; set; }
        public TileType? Collapsed { get; private set; } = null;
        public State(List<TileType> superposedTiles)
        {
            SuperposedTiles = superposedTiles;
        }

        public State Copy() {
            return new State(SuperposedTiles);
        }

        /// collapse l'état en une seule tuile
        /// collapse de manière "forte" : collapse meme si plusieurs états sont supperposés
        /// @return la tuile résultante après collapse
        public TileType Collapse()
        {
            // here we are assuming the superposed tiles were all possible to be according to the adjacent tiles.
            // wrong tiles should have been removed previously by and Update() call
            Collapsed = SuperposedTiles[new Random().Next(0, SuperposedTiles.Count)];
            SuperposedTiles = new List<TileType>();

            return Collapsed;
        }
            
        /// update les états supperposés en fonction des tuiles adjacentes
        /// collapse de manière "faible" : collapse seulement si il reste un seul état possible
        /// @return true si l'update à collapse
        public bool Update(State up, State down, State right, State left)
        {
            SuperposedTiles.RemoveAll(tile =>
                !up.SuperposedTiles.Exists(t => t.Down.Contains(tile)) // il existe dans la liste des superposées en haut une tuile t qui contient la tuile tile en down
                  || !down.SuperposedTiles.Exists(t => t.Up.Contains(tile))
                  || !right.SuperposedTiles.Exists(t => t.Left.Contains(tile))
                  || !left.SuperposedTiles.Exists(t => t.Right.Contains(tile)));

            if (SuperposedTiles.Count <= 1)
            {
                Collapse();
                return true;
            }

            return false;
        }

        // Update the current tile using only one direction
        public void UpdateDir(DirectionType dir, State state)
        {
            if (Collapsed != null)
            {
                return;
            }

            SuperposedTiles.RemoveAll(tile => 
                !state.SuperposedTiles.Exists(t => 
                    t.OpposingTileTypes(dir).Contains(tile)));

            if (SuperposedTiles.Count <= 1)
            {
                Collapse();
            }
        }

        public bool IsCollapsed() {
            return Collapsed != null;
        }

    }

}