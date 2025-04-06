using System.Collections.Generic;
using Godot;

public partial class Bouncy : CharacterBody2D
{
    private Vector2 mousePosition;
    private List<BouncyData> bouncyDatas = [];
    [Export] private int loopCount = 3;

    public override void _Draw()
    {
        // Vẽ tia ban đầu
        DrawLine(Vector2.Zero, ToLocal(mousePosition), Colors.Red, 5);

        // Vẽ các điểm va chạm và các vector
        foreach (var data in bouncyDatas)
        {
            var localPosition = ToLocal(data.Position);
            
            // Vẽ normal vector (màu xanh)
            DrawLine(localPosition, localPosition + data.Normal * 30, Colors.Blue, 3);
            
            // Vẽ vector phản xạ (màu cam)
            DrawLine(localPosition, localPosition + data.BounceDirection.Normalized() * 30, Colors.Orange, 3);
            
            // Vẽ điểm va chạm
            DrawCircle(localPosition, 5, Colors.Yellow);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        var inputDirection = Input.GetVector("Move Left", "Move Right", "Move Up", "Move Down");
        Velocity = inputDirection * 250;
        MoveAndSlide();
        QueueRedraw();
        
        mousePosition = GetGlobalMousePosition();
        var currentPosition = Position;
        var currentDirection = (mousePosition - Position).Normalized(); // Tạo vector hướng chính xác
        
        bouncyDatas.Clear();

        for (var i = 0; i < loopCount; i++)
        {
            var spaceState = GetWorld2D().DirectSpaceState;
            // Tính điểm đích bằng cách kéo dài vector hướng
            var targetPoint = currentPosition + currentDirection * 1000;
            var query = PhysicsRayQueryParameters2D.Create(currentPosition, targetPoint);
            query.Exclude = [GetRid()];
            var result = spaceState.IntersectRay(query);
            
            if (result.Count > 0)
            {
                var hitPosition = result["position"].As<Vector2>();
                var normal = result["normal"].As<Vector2>();
                
                // Công thức phản xạ: R = I - 2(N·I)N
                var bounceDirection = currentDirection - 2 * normal.Dot(currentDirection) * normal;
                
                bouncyDatas.Add(new BouncyData
                {
                    Position = hitPosition,
                    Normal = normal,
                    BounceDirection = bounceDirection
                });

                // Cập nhật vị trí và hướng cho lần lặp tiếp theo
                currentPosition = hitPosition;
                currentDirection = bounceDirection;
            }
            else
            {
                // Không còn va chạm nào nữa, thoát khỏi vòng lặp
                break;
            }
        }
    }
}

public struct BouncyData
{
    public Vector2 Position;
    public Vector2 Normal;
    public Vector2 BounceDirection;
}