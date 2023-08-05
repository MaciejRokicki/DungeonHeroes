using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class EnemySpawnerSystem : SystemBase
{
    float timer = 0.0f;

    protected override void OnCreate()
    {
        RequireForUpdate<EnemySpawnerComponent>();
        RequireForUpdate<PlayerComponent>();
    }

    protected override void OnUpdate()
    {
        LocalTransform playerTransform = SystemAPI.GetComponent<LocalTransform>(SystemAPI.GetSingletonEntity<PlayerComponent>());

        if (timer > 0.5f)
        {
            EntityCommandBuffer.ParallelWriter ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(World.Unmanaged)
                .AsParallelWriter();

            new EnemySpawnerSpawnEnemyJob
            {
                ECB = ecb,
                Random = new Unity.Mathematics.Random((uint)Random.Range(1, 10000)),
                DeltaTime = SystemAPI.Time.DeltaTime,
                PlayerPosition = playerTransform.Position
            }.ScheduleParallel();

            timer = 0.0f;
        }

        timer += SystemAPI.Time.DeltaTime;
    }
}