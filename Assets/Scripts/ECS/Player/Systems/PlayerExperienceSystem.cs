using Unity.Burst;
using Unity.Entities;

public partial struct PlayerExperienceSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerExperienceComponent>();
        state.RequireForUpdate<WeaponManagerComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        new PlayerExperienceJob
        {
            ECB = ecb,
            WeaponManagerEntity = SystemAPI.GetSingletonEntity<WeaponManagerComponent>(),
            WeaponBufferElementLookup = SystemAPI.GetBufferLookup<WeaponBufferElement>(true),
            WeaponComponentLookup = SystemAPI.GetComponentLookup<WeaponComponent>(true),
        }.Schedule();
    }
}