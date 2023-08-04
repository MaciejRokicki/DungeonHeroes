using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public partial struct DestroyPlayerJob : IJobEntity
{
    public EntityCommandBuffer Ecb;

    public void Execute(Entity entity, PlayerComponent playerComponent)
    {
        if(playerComponent.Health < 0.0f)
        {
            Ecb.DestroyEntity(entity);
        }
    }
}