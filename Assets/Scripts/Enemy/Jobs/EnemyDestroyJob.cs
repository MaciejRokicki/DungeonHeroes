using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct EnemyDestronyJob : IJobEntity
{
    public EntityCommandBuffer ECB;

    public void Execute(Entity entity, EnemyComponent enemyComponent)
    {
        if(enemyComponent.Health < 0.0f)
        {
            ECB.DestroyEntity(entity);
        }
    }
}