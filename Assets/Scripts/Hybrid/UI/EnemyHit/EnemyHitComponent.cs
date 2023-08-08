using Unity.Entities;
using Unity.Mathematics;

public struct EnemyHitComponent : IComponentData
{
    public float Damage;
    public float3 Position;
}