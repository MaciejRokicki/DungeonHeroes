using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct PlayerExperienceJob : IJobEntity
{
    public EntityCommandBuffer ECB;

    public void Execute(Entity entity, PlayerExperienceComponent playerExperienceComponent)
    {
        if (playerExperienceComponent.Experience >= playerExperienceComponent.ExperienceToNextLevel)
        {
            PlayerExperienceComponent playerExperienceComponentTmp = playerExperienceComponent;
            playerExperienceComponentTmp.Experience = playerExperienceComponent.ExperienceToNextLevel - playerExperienceComponent.Experience;
            playerExperienceComponentTmp.Level++;
            playerExperienceComponentTmp.ExperienceToNextLevel += playerExperienceComponentTmp.Level * 20;

            ECB.SetComponent(entity, playerExperienceComponentTmp);

            Entity onPlayerLevelUp = ECB.CreateEntity();
            ECB.AddComponent(onPlayerLevelUp, new OnPlayerLevelUpComponent
            {
                Level = playerExperienceComponentTmp.Level
            });
        }
    }
}