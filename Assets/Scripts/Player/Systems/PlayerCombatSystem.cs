using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[UpdateAfter(typeof(PlayerMovementSystem))]
public partial struct PlayerCombatSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerComponent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        NativeArray<float3> directions = new NativeArray<float3>(new float3[8]
        {
                new float3(0.0f, 1.0f, 0.0f),
                new float3(1.0f, 1.0f, 0.0f),
                new float3(1.0f, 0.0f, 0.0f),
                new float3(1.0f, -1.0f, 0.0f),
                new float3(0.0f, -1.0f, 0.0f),
                new float3(-1.0f, -1.0f, 0.0f),
                new float3(-1.0f, 0.0f, 0.0f),
                new float3(-1.0f, 1.0f, 0.0f)
        }, Allocator.TempJob);
        RefRW<PlayerComponent> playerComponent = SystemAPI.GetSingletonRW<PlayerComponent>();
        DynamicBuffer<PlayerAttackTimerBufferElement> playerAttackTimerBuffer = SystemAPI.GetSingletonBuffer<PlayerAttackTimerBufferElement>();

        state.Dependency = new PlayerWeaponSpawnJob
        {
            ECB = ecb,
            DeltaTime = SystemAPI.Time.DeltaTime,
            WeaponCompnent = SystemAPI.GetComponent<WeaponComponent>(SystemAPI.GetComponent<PlayerComponent>(playerEntity).Weapon),
            AttackDirections = directions,
            WeaponEntity = SystemAPI.GetComponent<PlayerComponent>(playerEntity).Weapon,
        }.Schedule(state.Dependency);

        state.Dependency.Complete();

        foreach (PlayerAttackTimerBufferElement playerAttackTimerBufferElement in playerAttackTimerBuffer)
        {
            playerComponent.ValueRW.AttackTimer = playerAttackTimerBufferElement.Value;
        }

        playerAttackTimerBuffer.Clear();
    }
}