using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct EnemySpawnerProcessJob : IJobEntity
{
    public Random Random;
    public EntityCommandBuffer.ParallelWriter Ecb;
    public float deltaTime;
    public float3 PlayerPosition;

    private void Execute([ChunkIndexInQuery] int chunkIndex,
        EnemySpawnerComponent spawner,
        ref DynamicBuffer<EnemySpawnerEnemiesBuffer> enemySpawnerEnemiesBuffer,
        ref DynamicBuffer<EnemySpawnerSpawnPositionBuffer> enemySpawnerSpawnPositionBuffer)
    {

        foreach (EnemySpawnerSpawnPositionBuffer enemySpawnerSpawnPosition in enemySpawnerSpawnPositionBuffer)
        {
            if (PlayerPosition.x - CameraGameObject.CameraViewSize.x / 2.0f < enemySpawnerSpawnPosition.SpawnPosition.x &&
                PlayerPosition.x + CameraGameObject.CameraViewSize.x / 2.0f > enemySpawnerSpawnPosition.SpawnPosition.x &&
                PlayerPosition.y - CameraGameObject.CameraViewSize.y / 2.0f < enemySpawnerSpawnPosition.SpawnPosition.y &&
                PlayerPosition.y + CameraGameObject.CameraViewSize.y / 2.0f > enemySpawnerSpawnPosition.SpawnPosition.y)
            {
                continue;
            }

            Entity enemyPrefab = enemySpawnerEnemiesBuffer[Random.NextInt(0, enemySpawnerEnemiesBuffer.Length)].Enemy;

            Entity entity = Ecb.Instantiate(chunkIndex, enemyPrefab);
            Ecb.SetComponent(chunkIndex, entity, LocalTransform.FromPosition(enemySpawnerSpawnPosition.SpawnPosition));
        }

    }
}