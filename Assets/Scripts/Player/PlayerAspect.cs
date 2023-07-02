using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public readonly partial struct PlayerAspect : IAspect
{
    public readonly Entity Entity;

    readonly RefRW<LocalTransform> transform;
    readonly RefRW<PlayerComponent> playerComponent;
    readonly DynamicBuffer<PlayerHealthBufferElement> playerHealthBuffer;

    public readonly float3 Position
    {
        get
        {
            return transform.ValueRO.Position;
        }
    }

    public readonly float MovementSpeed
    {
        get { return playerComponent.ValueRO.MovementSpeed; }
    }

    [BurstCompile]
    public void Move(float3 dir)
    {
        transform.ValueRW = transform.ValueRO.Translate(dir);

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
    public void ApplyBuffer()
    {
        foreach (PlayerHealthBufferElement playerHealthBufferElement in playerHealthBuffer)
        {
            playerComponent.ValueRW.Health += playerHealthBufferElement.Value;
        }

        playerHealthBuffer.Clear();
    }
}
