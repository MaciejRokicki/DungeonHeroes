using Unity.Burst;
using Unity.Entities;

public partial struct WeaponSystem : ISystem
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

        WeaponMovementJob weaponMovementJob = new WeaponMovementJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            Ecb = ecb
        };

        state.Dependency = weaponMovementJob.Schedule(state.Dependency);

        WeaponEnemyTriggerJob weaponEnemyTriggerJob = new WeaponEnemyTriggerJob
        {
            Ecb = ecb,
            EnemyComponent = SystemAPI.GetComponentLookup<EnemyComponent>()
        };

        state.Dependency = weaponEnemyTriggerJob.Schedule(state.Dependency);
        state.Dependency.Complete();
    }
}