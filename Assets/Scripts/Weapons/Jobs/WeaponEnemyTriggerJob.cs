using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics.Stateful;

[BurstCompile]
public partial struct WeaponEnemyTriggerJob : IJobChunk
{
    public EntityCommandBuffer.ParallelWriter ECB;
    public EntityTypeHandle EntityTypeHandle;
    [ReadOnly]
    public BufferTypeHandle<StatefulTriggerEvent> StatefulTriggerEventTypeHandle;
    [ReadOnly]
    public ComponentTypeHandle<WeaponComponent> WeaponComponentTypeHandle;
    [ReadOnly]
    public ComponentLookup<EnemyComponent> EnemyComponentLookup;

    public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
    {
        NativeArray<Entity> entites = chunk.GetNativeArray(EntityTypeHandle);
        BufferAccessor<StatefulTriggerEvent> statefulTriggerEvents = chunk.GetBufferAccessor(ref StatefulTriggerEventTypeHandle);
        NativeArray<WeaponComponent> weaponComponents = chunk.GetNativeArray(ref WeaponComponentTypeHandle);

        for(int i = 0; i < chunk.Count; i++)
        {
            for (int j = 0; j < statefulTriggerEvents[i].Length; j++)
            {
                StatefulTriggerEvent triggerEvent = statefulTriggerEvents[i][j];
                Entity otherEntity = triggerEvent.GetOtherEntity(entites[i]);

                if (triggerEvent.State == StatefulEventState.Enter)
                {
                    EnemyComponent enemyComponent;

                    if (EnemyComponentLookup.TryGetComponent(otherEntity, out enemyComponent))
                    {
                        enemyComponent.Health -= weaponComponents[i].WeaponStatisticsBlob.Value.Damage;

                        ECB.SetComponent(unfilteredChunkIndex, otherEntity, enemyComponent);
                    }
                }
            }
        }
    }
}