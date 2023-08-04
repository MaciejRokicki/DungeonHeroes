using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct EnemySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        LocalTransform playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
        DynamicBuffer<PlayerHealthBufferElement> playerHealthBuffer = SystemAPI.GetSingletonBuffer<PlayerHealthBufferElement>();

        EnemyMovementJob enemyMovementJob = new EnemyMovementJob
        {
            PlayerPosition = playerTransform.Position,
            PlayerHealthBuffer = playerHealthBuffer,
            DeltaTime = SystemAPI.Time.DeltaTime
        };

        enemyMovementJob.Schedule();

        EnemyDestronyJob enemyDestronyJob = new EnemyDestronyJob
        {
            Ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged)
        };

        enemyDestronyJob.Schedule();
    }
}
