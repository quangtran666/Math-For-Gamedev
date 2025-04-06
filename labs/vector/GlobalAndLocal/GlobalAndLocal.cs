using Godot;

public partial class GlobalAndLocal : Node2D
{
    [Export] private Node2D Node1;
    [Export] private Node2D Node2;

    public override void _Process(double delta)
    {
        GD.Print("Global Position Node1: ", Node1.GlobalPosition);
        GD.Print("Local Position Node1: ", Node1.Position);
        var globalPositionNode1 = Position + Node1.Position;
        GD.Print("Calculated Global Position Node1: ", globalPositionNode1);
        GD.Print("Calculated Local Position Node1: ", MyToLocal(globalPositionNode1));
    }

    private Vector2 MyToLocal(Vector2 globalPosition)
    {
        var direction = globalPosition - Position;
        return new Vector2
        {
            X = Vector2.Right.Dot(direction),
            Y = Vector2.Down.Dot(direction)
        };
    }
}
