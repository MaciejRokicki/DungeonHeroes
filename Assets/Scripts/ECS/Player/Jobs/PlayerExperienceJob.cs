using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct PlayerExperienceJob : IJobEntity
{
    public EntityCommandBuffer ECB;

    public void Execute(Entity entity, PlayerExperienceComponent playerExperienceComponent)
    {
        if(playerExperienceComponent.Experience >= playerExperienceComponent.ExperienceToNextLevel)
        {
            PlayerExperienceComponent playerExperienceComponentTmp = playerExperienceComponent;
            playerExperienceComponentTmp.Experience = playerExperienceComponent.ExperienceToNextLevel - playerExperienceComponent.Experience;
            playerExperienceComponentTmp.Level++;
            playerExperienceComponentTmp.ExperienceToNextLevel += playerExperienceComponentTmp.Level * 20; 

            ECB.SetComponent(entity, playerExperienceComponentTmp);
        }
    }
}