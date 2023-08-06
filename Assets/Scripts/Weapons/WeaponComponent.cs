using Unity.Entities;
using Unity.Mathematics;

public struct WeaponComponent : IComponentData
{
    public BlobAssetReference<WeaponStatistics> WeaponStatisticsBlob;
    public float3 Direction;
    public float3 SpawnPosition;
}