using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

public partial struct WeaponMovementSystem : ISystem
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

        state.Dependency = new WeaponMovementJob
        {
            ECB = ecb,
            DeltaTime = SystemAPI.Time.DeltaTime,
            EntityTypeHandle = SystemAPI.GetEntityTypeHandle(),
            TransformTypeHandle = SystemAPI.GetComponentTypeHandle<LocalTransform>(),
            WeaponComponentTypeHandle = SystemAPI.GetComponentTypeHandle<WeaponComponent>(true)
        }.ScheduleParallel(weaponQuery, state.Dependency);
    }
}