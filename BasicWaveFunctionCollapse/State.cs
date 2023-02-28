namespace BasicWaveFunctionCollapse;

public class State
{
    public static readonly State EmptyState = new(new());
    private List<TileType> SuperposedTiles { get; set; }
    private TileType Collapsed = null;
    
    public State(List<TileType> superposedTiles)
    {
        SuperposedTiles = superposedTiles;
    }

    public State Copy() {
        return new State(new List<TileType>(SuperposedTiles));
    }

    /// collapse l'état en une seule tuile
    /// collapse de manière "forte" : collapse meme si plusieurs états sont supperposés
    /// @return la tuile résultante après collapse
    public TileType Collapse()
    {
        // here we are assuming the superposed tiles were all possible to be according to the adjacent tiles.
        // wrong tiles should have been removed previously by and Update() call
        Collapsed = SuperposedTiles[new Random().Next(0, SuperposedTiles.Count)];
        SuperposedTiles.Clear();

        return Collapsed;
    }
            
    /// update les états supperposés en fonction des tuiles adjacentes
    /// collapse de manière "faible" : collapse seulement si il reste un seul état possible
    /// @return true si l'update a enlevé des tiles superposées
    public bool Update(State up, State down, State right, State left)
    {
        int removed = SuperposedTiles.RemoveAll(tile =>
            (!up.Equals(EmptyState) && !up.SuperposedTiles.Exists(t => t.Down.Contains(tile))) // n'est pas empty (bordure) && il existe dans la liste des superposées en haut une tuile t qui contient la tuile tile en down
            || (!down.Equals(EmptyState) && !down.SuperposedTiles.Exists(t => t.Up.Contains(tile)))
            || (!right.Equals(EmptyState) && !right.SuperposedTiles.Exists(t => t.Left.Contains(tile)))
            || (!left.Equals(EmptyState) && !left.SuperposedTiles.Exists(t => t.Right.Contains(tile))));

        if (SuperposedTiles.Count == 1)
        {
            Collapsed = SuperposedTiles[0];
            SuperposedTiles.Clear();
            return true;
        }

        return removed != 0;
    }

}