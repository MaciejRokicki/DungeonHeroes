using Unity.Entities;
using Unity.Mathematics;

public readonly partial struct ObstacleAspect : IAspect
{
    public readonly Entity entity;

    readonly RefRO<ObstacleComponent> obstacleComponent;

    public float3 LeftBotBorder
    {
        get { return obstacleComponent.ValueRO.LeftBotBorder; }
    }

    public float3 RightTopBorder
    {
        get { return obstacleComponent.ValueRO.RightTopBorder; }
    }
}