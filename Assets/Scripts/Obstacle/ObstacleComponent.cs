using Unity.Entities;
using Unity.Mathematics;

public struct ObstacleComponent : IComponentData
{
    public float3 LeftBotBorder;
    public float3 RightTopBorder;
}
