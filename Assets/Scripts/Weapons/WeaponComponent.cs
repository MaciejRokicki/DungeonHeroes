using Unity.Entities;
using Unity.Mathematics;

public struct WeaponComponent : IComponentData
{
    public float Damage;
    public float Speed;
    public float Firerate;
    public float3 Direction;
    public float3 SpawnPosition;
}