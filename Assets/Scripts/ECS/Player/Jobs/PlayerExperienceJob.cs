using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct PlayerExperienceJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    public Entity WeaponManagerEntity;
    [ReadOnly]
    public BufferLookup<WeaponBufferElement> WeaponBufferElementLookup;
    [ReadOnly]
    public ComponentLookup<WeaponComponent> WeaponComponentLookup;

    public void Execute(Entity entity, PlayerExperienceComponent playerExperienceComponent)
    {
        if (playerExperienceComponent.Experience >= playerExperienceComponent.ExperienceToNextLevel)
        {
            PlayerExperienceComponent playerExperienceComponentTmp = playerExperienceComponent;
            playerExperienceComponentTmp.Experience = playerExperienceComponent.ExperienceToNextLevel - playerExperienceComponent.Experience;
            playerExperienceComponentTmp.Level++;
            playerExperienceComponentTmp.ExperienceToNextLevel += playerExperienceComponentTmp.Level * 20;

            ECB.SetComponent(entity, playerExperienceComponentTmp);

            CheckAvailableWeapon(playerExperienceComponentTmp.Level, entity);
        }
    }

    private void CheckAvailableWeapon(int level, Entity playerEntity)
    {
        foreach (WeaponBufferElement weapon in WeaponBufferElementLookup[WeaponManagerEntity])
        {
            WeaponComponent w = WeaponComponentLookup[weapon.Weapon];

            if (level == w.WeaponStatisticsBlob.Value.PlayerLevel)
            {
                ECB.AppendToBuffer(playerEntity, new PlayerWeaponBufferElement
                {
                    Weapon = weapon.Weapon,
                    AttackTimer = 0.0f
                });
            }
        }
    }
}