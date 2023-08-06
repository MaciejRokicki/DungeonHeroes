using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
public partial struct EnemyMovementJob : IJobChunk
{
    public EntityCommandBuffer.ParallelWriter ECB;
    public EntityTypeHandle EntityTypeHandle;
    public ComponentTypeHandle<LocalTransform> TransformTypeHandle;
    public ComponentTypeHandle<PhysicsVelocity> PhysicsVelocityTypeHandle;
    public ComponentTypeHandle<PhysicsMass> PhysicsMassTypeHandle;
    public ComponentTypeHandle<EnemyComponent> EnemyComponentTypeHandle;
    [ReadOnly]
    public float3 PlayerPosition;
    public Entity PlayerEntity;
    [ReadOnly]
    public float DeltaTime;

    public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
    {
        NativeArray<Entity> entities = chunk.GetNativeArray(EntityTypeHandle);
        NativeArray<LocalTransform> transforms = chunk.GetNativeArray(ref TransformTypeHandle);
        NativeArray<PhysicsVelocity> velocities = chunk.GetNativeArray(ref PhysicsVelocityTypeHandle);
        NativeArray<PhysicsMass> masses = chunk.GetNativeArray(ref PhysicsMassTypeHandle);
        NativeArray<EnemyComponent> enemies = chunk.GetNativeArray(ref EnemyComponentTypeHandle);

        for(int i = 0; i < chunk.Count; i++)
        {
            if(math.distance(PlayerPosition, transforms[i].Position) > 0.5f)
            {
                float3 direction = math.normalize(PlayerPosition - transforms[i].Position);

                PhysicsVelocity vel = velocities[i];
                vel.Linear = enemies[i].MovementSpeed * direction;
                PhysicsMass mass = masses[i];
                mass.InverseInertia = float3.zero;
                LocalTransform transform = transforms[i];

                if(direction.x < 0.0f)
                {
                    transform.Rotation = quaternion.EulerXYZ(new float3(0.0f, math.radians(180.0f), 0.0f));
                }

                if(direction.x > 0.0f)
                {
                    transform.Rotation = quaternion.EulerXYZ(new float3(0.0f, 0.0f, 0.0f));
                }

                ECB.SetComponent(unfilteredChunkIndex, entities[i], vel);
                ECB.SetComponent(unfilteredChunkIndex, entities[i], mass);
                ECB.SetComponent(unfilteredChunkIndex, entities[i], transform);
            }
            else
            {
                EnemyComponent enemyComponent = enemies[i];

                if (enemies[i].HitTimer > 1.0f)
                {
                    ECB.AppendToBuffer(unfilteredChunkIndex, PlayerEntity, new PlayerHealthBufferElement { Value = -enemies[i].Damage });
                    enemyComponent.HitTimer = 0.0f;
                }
                else
                {
                    enemyComponent.HitTimer += DeltaTime;
                }

                ECB.SetComponent(unfilteredChunkIndex, entities[i], enemyComponent);
            }
        }
    }
}