using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct EnemySpawnerSpawnEnemyJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ECB;
    public Random Random;
    public float DeltaTime;
    public float3 PlayerPosition;

    private void Execute([ChunkIndexInQuery] int chunkIndex,
        EnemySpawnerComponent _,
        ref DynamicBuffer<EnemySpawnerEnemiesBuffer> enemySpawnerEnemiesBuffer,
        ref DynamicBuffer<EnemySpawnerSpawnPositionBuffer> enemySpawnerSpawnPositionBuffer)
    {

        foreach (EnemySpawnerSpawnPositionBuffer enemySpawnerSpawnPosition in enemySpawnerSpawnPositionBuffer)
        {
            if (PlayerPosition.x - CameraGameObject.Instance.CameraViewSize.x / 2.0f < enemySpawnerSpawnPosition.SpawnPosition.x &&
                PlayerPosition.x + CameraGameObject.Instance.CameraViewSize.x / 2.0f > enemySpawnerSpawnPosition.SpawnPosition.x &&
                PlayerPosition.y - CameraGameObject.Instance.CameraViewSize.y / 2.0f < enemySpawnerSpawnPosition.SpawnPosition.y &&
                PlayerPosition.y + CameraGameObject.Instance.CameraViewSize.y / 2.0f > enemySpawnerSpawnPosition.SpawnPosition.y)
            {
                continue;
            }

            Entity enemyPrefab = enemySpawnerEnemiesBuffer[Random.NextInt(0, enemySpawnerEnemiesBuffer.Length)].Enemy;

            Entity entity = ECB.Instantiate(chunkIndex, enemyPrefab);
            ECB.SetComponent(chunkIndex, entity, LocalTransform.FromPosition(enemySpawnerSpawnPosition.SpawnPosition));
        }

    }
}