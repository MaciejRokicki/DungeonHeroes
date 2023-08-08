using Unity.Entities;

public partial class EnemyHitSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<EnemyHitComponent>();
    }

    protected override void OnUpdate()
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);

        foreach((RefRW<EnemyHitComponent> enemyHitComponent, Entity entity) in SystemAPI.Query<RefRW<EnemyHitComponent>>().WithEntityAccess())
        {
            ecb.RemoveComponent<EnemyHitComponent>(entity);
            EnemyHitManagerGameObject.DisplayEnemyHit(enemyHitComponent.ValueRO.Position, enemyHitComponent.ValueRO.Damage);
        }
    }
}