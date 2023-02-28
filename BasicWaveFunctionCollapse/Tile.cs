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
}