using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
public partial struct PlayerMovementJob : IJobEntity
{
    public float3 movementDirection;
    [ReadOnly]
    public NativeArray<ObstacleComponent> obstacleAspects;

    [BurstCompile]
    private void Execute(PlayerAspect playerAspect)
    {
        foreach (ObstacleComponent component in obstacleAspects)
        {
            float3 position = playerAspect.Position + movementDirection;

            if (position.x > component.LeftBotBorder.x && position.x < component.RightTopBorder.x &&
                position.y > component.LeftBotBorder.y && position.y < component.RightTopBorder.y)
            {
                movementDirection = float3.zero;

                break;
            }
        }

        playerAspect.Move(movementDirection);
    }
}