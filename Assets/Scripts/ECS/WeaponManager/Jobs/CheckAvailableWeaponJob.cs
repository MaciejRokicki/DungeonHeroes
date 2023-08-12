using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct CheckAvailableWeaponJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    public Entity WeaponManagerEntity;
    public Entity PlayerEntity;
    [ReadOnly]
    public BufferLookup<WeaponBufferElement> WeaponBufferElementLookup;
    [ReadOnly]
    public ComponentLookup<WeaponComponent> WeaponComponentLookup;

    public void Execute(Entity entity, OnPlayerLevelUpComponent onPlayerLevelUpComponent)
    {
        for (int i = 0; i < WeaponBufferElementLookup[WeaponManagerEntity].Length; i++)
        {
            Entity weaponEntity = WeaponBufferElementLookup[WeaponManagerEntity][i].Weapon;
            WeaponComponent w = WeaponComponentLookup[weaponEntity];

            if (onPlayerLevelUpComponent.Level == w.WeaponStatisticsBlob.Value.PlayerLevel)
            {
                ECB.AppendToBuffer(PlayerEntity, new PlayerWeaponBufferElement
                {
                    Weapon = weaponEntity,
                    AttackTimer = 0.0f
                });

                Entity onNewWeaponAvailableEntity = ECB.CreateEntity();
                ECB.AddComponent(onNewWeaponAvailableEntity, new OnNewWeaponAvailableComponent
                {
                    WeaponId = i
                });
            }
        }

        ECB.DestroyEntity(entity);
    }
}