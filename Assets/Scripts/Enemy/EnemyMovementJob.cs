using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
public partial struct EnemyMovementJob : IJobEntity
{
    [ReadOnly]
    public float3 PlayerPosition;
    public DynamicBuffer<PlayerHealthBufferElement> PlayerHealthBuffer;
    [ReadOnly]
    public float DeltaTime;

    private void Execute(EnemyAspect enemyAspect)
    {
        if(math.distance(PlayerPosition, enemyAspect.Position) > 0.5f)
        {
            float3 direction = math.normalize(PlayerPosition - enemyAspect.Position);

            enemyAspect.Move(direction, DeltaTime);
        }
        else
        {
            enemyAspect.AttackPlayer(DeltaTime, PlayerHealthBuffer);
        }
    }
}
