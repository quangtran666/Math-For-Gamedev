using Godot;

public partial class CharacterMovement : CharacterBody2D
{
    [Export] private int speed = 250;
    [Export] private int bombRadius = 100;
    private Dummy dummy;
    
    private bool isInBombRadius;

    public override void _Ready()
    {
        dummy = GetNode<Dummy>("../Dummy");
        GD.Print(dummy.Name);
    }

    public override void _Draw()
    {
        var color = isInBombRadius ? Colors.Green : Colors.Red;
        
        DrawCircle(Vector2.Zero, bombRadius, color, false);
        DrawLine(Vector2.Zero, dummy.Position - Position, Colors.White, 10);
    }

    public override void _Process(double delta)
    {
        isInBombRadius = dummy.Position.DistanceTo(Position) < bombRadius;
        QueueRedraw();
    }

    public override void _PhysicsProcess(double delta)
    {
        var inputdirection = Input.GetVector("Move Left", "Move Right", "Move Up", "Move Down");
        Velocity = inputdirection * speed;
        MoveAndSlide();
    }
}
