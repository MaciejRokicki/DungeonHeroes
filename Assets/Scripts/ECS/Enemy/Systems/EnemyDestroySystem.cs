using Unity.Burst;
using Unity.Entities;

public partial struct EnemyDestroySystem : ISystem
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

        new EnemyDestronyJob
        {
            ECB = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged),
            PlayerEntity = playerEntity,
            PlayerExperienceComponentLookup = SystemAPI.GetComponentLookup<PlayerExperienceComponent>(true)
        }.Schedule();
    }
}
