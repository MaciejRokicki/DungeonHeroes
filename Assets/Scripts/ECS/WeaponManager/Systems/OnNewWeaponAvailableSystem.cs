using Unity.Entities;

public partial class OnNewWeaponAvailableSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<OnNewWeaponAvailableComponent>();
    }

    protected override void OnUpdate()
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);

        foreach((OnNewWeaponAvailableComponent onNewWeaponAvailableComponent, Entity entity) in SystemAPI.Query<OnNewWeaponAvailableComponent>().WithEntityAccess())
        {
            WeaponUiManagerGameObject.Instance.RevealWeapon(onNewWeaponAvailableComponent.WeaponId);
            ecb.DestroyEntity(entity);
        }
    }
}