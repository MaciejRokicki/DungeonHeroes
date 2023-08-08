using Unity.Entities;
using Unity.Mathematics;

public struct WeaponStatistics
{
    public float Damage;
    public float Speed;
    public float Firerate;
    public BlobArray<float3> ShotDirections;
}