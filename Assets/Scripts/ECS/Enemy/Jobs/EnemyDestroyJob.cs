using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
public partial struct EnemyDestronyJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    [ReadOnly]
    public Entity PlayerEntity;
    [ReadOnly]
    public ComponentLookup<PlayerExperienceComponent> PlayerExperienceComponentLookup;

    public void Execute(Entity entity, EnemyComponent enemyComponent)
    {
        if(enemyComponent.Health < 0.0f)
        {
            ECB.DestroyEntity(entity);

            PlayerExperienceComponent playerExperienceComponentTmp = PlayerExperienceComponentLookup[PlayerEntity];
            playerExperienceComponentTmp.Experience += 10;
            ECB.SetComponent(PlayerEntity, playerExperienceComponentTmp);
        }
    }
}