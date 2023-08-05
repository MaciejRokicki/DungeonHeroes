using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct PlayerWeaponSpawnJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    [ReadOnly]
    public float DeltaTime;
    [ReadOnly]
    public Entity WeaponEntity;
    [ReadOnly]
    public WeaponComponent WeaponCompnent;
    [ReadOnly]
    public NativeArray<float3> AttackDirections;

    private void Execute(LocalTransform transform, PlayerComponent playerComponent, DynamicBuffer<PlayerAttackTimerBufferElement> playerAttackTimerBufferElement)
    {
        if (playerComponent.AttackTimer > 1.0f / WeaponCompnent.Firerate)
        {
            foreach (float3 attackDirection in AttackDirections)
            {
                float angleZ = math.radians(math.degrees(math.atan2(attackDirection.y, attackDirection.x)) - 90.0f);

                Entity weaponEntity = ECB.Instantiate(WeaponEntity);
                ECB.SetComponent(weaponEntity, LocalTransform.FromPositionRotation(
                    transform.Position + attackDirection,
                    quaternion.AxisAngle(new float3(0.0f, 0.0f, 1.0f), angleZ)
                    )
                );

                WeaponComponent tmpWeaponComponent = WeaponCompnent;
                tmpWeaponComponent.Direction = attackDirection;
                tmpWeaponComponent.SpawnPosition = transform.Position + attackDirection;

                ECB.SetComponent(weaponEntity, tmpWeaponComponent);
            }

            playerAttackTimerBufferElement.Add(new PlayerAttackTimerBufferElement { Value = 0.0f });
        }
        else
        {
            playerAttackTimerBufferElement.Add(new PlayerAttackTimerBufferElement { Value = playerComponent.AttackTimer + DeltaTime });
        }
    }
}