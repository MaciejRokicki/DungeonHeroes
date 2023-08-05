using Unity.Entities;
using UnityEngine;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    public GameObject[] Enemies;
    public Vector3[] SpawnPositions;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        foreach (Vector3 spawnPoint in SpawnPositions)
        {
            Gizmos.DrawCube(spawnPoint, Vector3.one);
        }
    }
#endif
}

public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring>
{
    public override void Bake(EnemySpawnerAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new EnemySpawnerComponent());

        DynamicBuffer<EnemySpawnerEnemiesBuffer> enemiesBuffer = AddBuffer<EnemySpawnerEnemiesBuffer>(entity);
        DynamicBuffer<EnemySpawnerSpawnPositionBuffer> spawnPositionBuffer = AddBuffer<EnemySpawnerSpawnPositionBuffer>(entity);

        foreach (GameObject enemy in authoring.Enemies)
        {
            enemiesBuffer.Add(new EnemySpawnerEnemiesBuffer
            {
                Enemy = GetEntity(enemy, TransformUsageFlags.Dynamic)
            });
        }

        foreach (Vector3 spawnPosition in authoring.SpawnPositions)
        {
            spawnPositionBuffer.Add(new EnemySpawnerSpawnPositionBuffer
            {
                SpawnPosition = spawnPosition
            });
        }
    }
}