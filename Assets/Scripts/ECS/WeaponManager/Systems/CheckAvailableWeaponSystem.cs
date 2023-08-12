using Unity.Burst;
using Unity.Entities;

[UpdateAfter(typeof(PlayerExperienceSystem))]
public partial struct CheckAvailableWeaponSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<OnPlayerLevelUpComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        new CheckAvailableWeaponJob
        {
            ECB = ecb,
            WeaponManagerEntity = SystemAPI.GetSingletonEntity<WeaponManagerComponent>(),
            PlayerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>(),
            WeaponBufferElementLookup = SystemAPI.GetBufferLookup<WeaponBufferElement>(true),
            WeaponComponentLookup = SystemAPI.GetComponentLookup<WeaponComponent>(true),
        }.Schedule();
    }
}