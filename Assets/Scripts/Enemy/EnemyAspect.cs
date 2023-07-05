using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public readonly partial struct EnemyAspect : IAspect
{
    public readonly Entity Entity;

    readonly RefRW<LocalTransform> transform;
    readonly RefRW<EnemyComponent> enemyComponent;
    readonly RefRW<PhysicsVelocity> velocity;
    readonly RefRW<PhysicsMass> mass;
    //readonly DynamicBuffer<PlayerHealthBufferElement> playerHealthBuffer;

    public readonly float3 Position
    {
        get { return transform.ValueRO.Position; }
    }

    public readonly float MovementSpeed
    {
        get { return enemyComponent.ValueRO.MovementSpeed; }
    }

    public readonly float Damage
    {
        get { return enemyComponent.ValueRO.Damage; }
    }

    public readonly float HitTimer
    {
        get { return enemyComponent.ValueRO.HitTimer; }
        set { enemyComponent.ValueRW.HitTimer = value; }
    }

    [BurstCompile]
    public void Move(float3 dir, float deltaTime)
    {
        velocity.ValueRW.Linear = MovementSpeed * dir;
        mass.ValueRW.InverseInertia = float3.zero;

        if (dir.x < 0.0f)
        {
            transform.ValueRW.Rotation = quaternion.EulerXYZ(new float3(0.0f, math.radians(180.0f), 0.0f));
        }

        if (dir.x > 0.0f)
        {
            transform.ValueRW.Rotation = quaternion.EulerXYZ(new float3(0.0f, 0.0f, 0.0f));
        }
    }

    [BurstCompile]
    public void AttackPlayer(float deltaTime, DynamicBuffer<PlayerHealthBufferElement> playerHealthBuffer)
    {
        if (enemyComponent.ValueRO.HitTimer > 1.0f)
        {
            playerHealthBuffer.Add(new PlayerHealthBufferElement { Value = -Damage });
            enemyComponent.ValueRW.HitTimer = 0.0f;
        }
        else
        {
            enemyComponent.ValueRW.HitTimer += deltaTime;
        }
    }

    //[BurstCompile]
    //public void ApplyBuffer()
    //{
    //    foreach (PlayerHealthBufferElement playerHealthBufferElement in playerHealthBuffer)
    //    {
    //        playerComponent.ValueRW.Health += playerHealthBufferElement.Value;
    //    }

    //    playerHealthBuffer.Clear();
    //}
}
