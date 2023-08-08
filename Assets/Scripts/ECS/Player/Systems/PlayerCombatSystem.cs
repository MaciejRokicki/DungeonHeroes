using Unity.Burst;
using Unity.Entities;

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

        state.Dependency = new PlayerWeaponSpawnJob
        {
            ECB = ecb,
            DeltaTime = SystemAPI.Time.DeltaTime,
            WeaponCompnentLookup = SystemAPI.GetComponentLookup<WeaponComponent>(),
        }.Schedule(state.Dependency);

        state.Dependency.Complete();
    }
}