using Unity.Burst;
using Unity.Entities;

public partial struct PlayerDestroySystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        state.Dependency = new DestroyPlayerJob
        {
            ECB = ecb
        }.Schedule(state.Dependency);

        state.Dependency.Complete();
    }
}