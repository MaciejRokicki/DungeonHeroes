using Unity.Burst;
using Unity.Entities;

[UpdateAfter(typeof(WeaponMovementSystem))]
public partial struct WeaponTriggerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<WeaponComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        state.Dependency = new WeaponEnemyTriggerJob
        {
            ECB = ecb,
            EnemyComponent = SystemAPI.GetComponentLookup<EnemyComponent>()
        }.Schedule(state.Dependency);
    }
}