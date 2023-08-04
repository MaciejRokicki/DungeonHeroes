using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
public readonly partial struct PlayerAspect : IAspect
{
    public readonly Entity Entity;

    readonly RefRW<LocalTransform> transform;
    readonly RefRW<PlayerComponent> playerComponent;
    readonly RefRW<PhysicsVelocity> velocity;
    readonly RefRW<PhysicsMass> mass;
    readonly DynamicBuffer<PlayerHealthBufferElement> playerHealthBuffer;
    readonly DynamicBuffer<PlayerAttackTimerBufferElement> playerAttackTimerBuffer;

    public readonly float3 Position
    {
        get { return transform.ValueRO.Position; }
    }

    public readonly float MovementSpeed
    {
        get { return playerComponent.ValueRO.MovementSpeed; }
    }

    [BurstCompile]
    public void Move(float3 dir)
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
    public void ApplyHealthBuffer()
    {
        foreach (PlayerHealthBufferElement playerHealthBufferElement in playerHealthBuffer)
        {
            playerComponent.ValueRW.Health += playerHealthBufferElement.Value;
        }

        playerHealthBuffer.Clear();
    }

    [BurstCompile]
    public void ApplyAttackTimerBuffer()
    {
        foreach (PlayerAttackTimerBufferElement playerAttackTimerBufferElement in playerAttackTimerBuffer)
        {
            playerComponent.ValueRW.AttackTimer = playerAttackTimerBufferElement.Value;
        }

        playerAttackTimerBuffer.Clear();
    }
}
