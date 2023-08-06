using Unity.Burst;
using Unity.Entities;
using Unity.Physics.Stateful;

[UpdateAfter(typeof(WeaponMovementSystem))]
public partial struct WeaponTriggerSystem : ISystem
{
    EntityQuery weaponQuery;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<WeaponComponent>();

        weaponQuery = state.GetEntityQuery(ComponentType.ReadOnly<WeaponComponent>());
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer.ParallelWriter ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();

        state.Dependency = new WeaponEnemyTriggerJob
        {
            ECB = ecb,
            EntityTypeHandle = SystemAPI.GetEntityTypeHandle(),
            StatefulTriggerEventTypeHandle = SystemAPI.GetBufferTypeHandle<StatefulTriggerEvent>(true),
            WeaponComponentTypeHandle = SystemAPI.GetComponentTypeHandle<WeaponComponent>(true),
            EnemyComponentLookup = SystemAPI.GetComponentLookup<EnemyComponent>(true)
        }.ScheduleParallel(weaponQuery, state.Dependency);
    }
}