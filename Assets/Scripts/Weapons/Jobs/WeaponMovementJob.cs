using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct WeaponMovementJob : IJobChunk
{
    public EntityCommandBuffer.ParallelWriter ECB;
    [ReadOnly]
    public float DeltaTime;
    public EntityTypeHandle EntityTypeHandle;
    public ComponentTypeHandle<LocalTransform> TransformTypeHandle;
    [ReadOnly]
    public ComponentTypeHandle<WeaponComponent> WeaponComponentTypeHandle;

    public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
    {
        NativeArray<Entity> entities = chunk.GetNativeArray(EntityTypeHandle);
        NativeArray<LocalTransform> transforms = chunk.GetNativeArray(ref TransformTypeHandle);
        NativeArray<WeaponComponent> weaponComponents = chunk.GetNativeArray(ref WeaponComponentTypeHandle);

        for(int i = 0; i < chunk.Count; i++)
        {
            LocalTransform transform = transforms[i];
            transform.Position = transform.Position + weaponComponents[i].Direction * weaponComponents[i].Speed * DeltaTime;

            ECB.SetComponent(unfilteredChunkIndex, entities[i], transform);

            if (math.distance(transforms[i].Position, weaponComponents[i].SpawnPosition) > 10.0f)
            {
                ECB.DestroyEntity(unfilteredChunkIndex, entities[i]);
            }
        }
    }
}