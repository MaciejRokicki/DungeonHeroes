using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct DestroyPlayerJob : IJobEntity
{
    public EntityCommandBuffer ECB;

    public void Execute(Entity entity, PlayerComponent playerComponent)
    {
        if(playerComponent.Health < 0.0f)
        {
            Entity onPlayerDie = ECB.CreateEntity();
            ECB.AddComponent(onPlayerDie, new OnPlayerDieComponent());

            ECB.DestroyEntity(entity);
        }
    }
}