using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

public partial struct EnemyMovementSystem : ISystem
{
    EntityQuery enemyQuery;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerComponent>();
        enemyQuery = state.GetEntityQuery(ComponentType.ReadOnly<EnemyComponent>());
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer.ParallelWriter ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter();
        EntityTypeHandle enemyEntityTypeHandle = SystemAPI.GetEntityTypeHandle();
        ComponentTypeHandle<LocalTransform> transformTypeHandle = SystemAPI.GetComponentTypeHandle<LocalTransform>();
        ComponentTypeHandle<PhysicsVelocity> physicsVelocityTypeHandle = SystemAPI.GetComponentTypeHandle<PhysicsVelocity>();
        ComponentTypeHandle<PhysicsMass> physicsMassTypeHandle = SystemAPI.GetComponentTypeHandle<PhysicsMass>();
        ComponentTypeHandle<EnemyComponent> enemyComponentTypeHandle = SystemAPI.GetComponentTypeHandle<EnemyComponent>();
        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        LocalTransform playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);

        state.Dependency = new EnemyMovementJob
        {
            ECB = ecb,
            EntityTypeHandle = enemyEntityTypeHandle,
            TransformTypeHandle = transformTypeHandle,
            PhysicsVelocityTypeHandle = physicsVelocityTypeHandle,
            PhysicsMassTypeHandle = physicsMassTypeHandle,
            EnemyComponentTypeHandle = enemyComponentTypeHandle,
            PlayerPosition = playerTransform.Position,
            PlayerEntity = playerEntity,
            DeltaTime = SystemAPI.Time.DeltaTime
        }.ScheduleParallel(enemyQuery, state.Dependency);
    }
}