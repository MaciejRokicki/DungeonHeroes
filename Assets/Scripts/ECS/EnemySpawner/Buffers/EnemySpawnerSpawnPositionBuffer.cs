using Unity.Entities;
using Unity.Mathematics;

public struct EnemySpawnerSpawnPositionBuffer : IBufferElementData
{
    public float3 SpawnPosition;
}