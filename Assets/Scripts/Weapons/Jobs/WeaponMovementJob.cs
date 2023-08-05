using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct WeaponMovementJob : IJobEntity
{
    [ReadOnly]
    public float DeltaTime;
    public EntityCommandBuffer Ecb;

    public void Execute(Entity entity, RefRW<LocalTransform> transform, WeaponComponent weaponCompnent)
    {
        transform.ValueRW.Position = transform.ValueRO.Position + weaponCompnent.Direction * weaponCompnent.Speed * DeltaTime;

        if(math.distance(transform.ValueRO.Position, weaponCompnent.SpawnPosition) > 10.0f)
        {
            Ecb.DestroyEntity(entity);
        }
    }
}