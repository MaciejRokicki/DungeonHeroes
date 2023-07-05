using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
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

        if(timer > 0.5f)
        {
            EntityCommandBuffer.ParallelWriter ecb = GetEntityCommandBuffer();

            EnemySpawnerProcessJob job = new EnemySpawnerProcessJob
            {
                Random = new Unity.Mathematics.Random((uint)Random.Range(1, 10000)),
                Ecb = ecb,
                deltaTime = SystemAPI.Time.DeltaTime,
                PlayerPosition = playerTransform.Position
            };

            job.ScheduleParallel();

            timer = 0.0f;
        }

        timer += SystemAPI.Time.DeltaTime;
    }

    [BurstCompile]
    private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer()
    {
        BeginSimulationEntityCommandBufferSystem.Singleton ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        EntityCommandBuffer ecb = ecbSingleton.CreateCommandBuffer(World.Unmanaged);

        return ecb.AsParallelWriter();
    }
}