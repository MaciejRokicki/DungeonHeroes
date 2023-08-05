using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct EnemyMovementSystem : ISystem
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

        new EnemyMovementJob
        {
            PlayerPosition = playerTransform.Position,
            PlayerHealthBuffer = playerHealthBuffer,
            DeltaTime = SystemAPI.Time.DeltaTime
        }.Schedule();
    }
}