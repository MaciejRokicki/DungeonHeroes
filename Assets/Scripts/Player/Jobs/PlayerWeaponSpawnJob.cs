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
    public ComponentLookup<WeaponComponent> WeaponCompnentLookup;

    private void Execute(Entity entity, LocalTransform transform, PlayerComponent playerComponent, DynamicBuffer<PlayerWeaponBufferElement> playerWeaponBuffer)
    {
        for (int i = 0; i < playerWeaponBuffer.Length; i++)
        {
            if (playerWeaponBuffer[i].AttackTimer > 1.0f / WeaponCompnentLookup[playerWeaponBuffer[i].Weapon].WeaponStatisticsBlob.Value.Firerate)
            {
                for (int j = 0; j < WeaponCompnentLookup[playerWeaponBuffer[i].Weapon].WeaponStatisticsBlob.Value.ShotDirections.Length; j++)
                {
                    float3 attackDirection = WeaponCompnentLookup[playerWeaponBuffer[i].Weapon].WeaponStatisticsBlob.Value.ShotDirections[j];
                    float angleZ = math.radians(math.degrees(math.atan2(attackDirection.y, attackDirection.x)) - 90.0f);

                    Entity weaponEntity = ECB.Instantiate(playerWeaponBuffer[i].Weapon);
                    ECB.SetComponent(weaponEntity, LocalTransform.FromPositionRotation(
                        transform.Position + attackDirection,
                        quaternion.AxisAngle(new float3(0.0f, 0.0f, 1.0f), angleZ)
                        )
                    );

                    WeaponComponent tmpWeaponComponent = WeaponCompnentLookup[playerWeaponBuffer[i].Weapon];
                    tmpWeaponComponent.Direction = attackDirection;
                    tmpWeaponComponent.SpawnPosition = transform.Position + attackDirection;

                    ECB.SetComponent(weaponEntity, tmpWeaponComponent);
                }

                PlayerWeaponBufferElement tmpPlayerWeaponBufferElement = playerWeaponBuffer[i];
                tmpPlayerWeaponBufferElement.AttackTimer = 0.0f;
                playerWeaponBuffer[i] = tmpPlayerWeaponBufferElement;
            }
            else
            {
                PlayerWeaponBufferElement tmpPlayerWeaponBufferElement = playerWeaponBuffer[i];
                tmpPlayerWeaponBufferElement.AttackTimer += DeltaTime;
                playerWeaponBuffer[i] = tmpPlayerWeaponBufferElement;
            }
        }
    }
}