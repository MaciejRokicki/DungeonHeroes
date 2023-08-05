using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics.Stateful;

[BurstCompile]
public partial struct WeaponEnemyTriggerJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    [ReadOnly]
    public ComponentLookup<EnemyComponent> EnemyComponent;

    public void Execute(Entity entity, ref DynamicBuffer<StatefulTriggerEvent> triggerBuffer, WeaponComponent weaponComponent)
    {
        for (int i = 0; i < triggerBuffer.Length; i++)
        {
            StatefulTriggerEvent triggerEvent = triggerBuffer[i];
            Entity otherEntity = triggerEvent.GetOtherEntity(entity);

            if (triggerEvent.State == StatefulEventState.Enter)
            {
                EnemyComponent enemyComponent;
                
                if(EnemyComponent.TryGetComponent(otherEntity, out enemyComponent))
                {
                    enemyComponent.Health -= weaponComponent.Damage;

                    ECB.SetComponent(otherEntity, enemyComponent);
                }
            }
        }
    }
}