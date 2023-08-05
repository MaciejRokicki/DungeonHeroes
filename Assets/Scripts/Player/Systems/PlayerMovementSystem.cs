using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[UpdateAfter(typeof(PlayerInputSystem))]
public partial struct PlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerComponent>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var playerInputComponent = SystemAPI.GetSingleton<PlayerInputComponent>();
        RefRW<PlayerComponent> playerComponent = SystemAPI.GetSingletonRW<PlayerComponent>();
        DynamicBuffer<PlayerHealthBufferElement> playerHealthBuffer = SystemAPI.GetSingletonBuffer<PlayerHealthBufferElement>();

        state.Dependency = new PlayerMovementJob
        {
            MovementDirection = new float3(playerInputComponent.MovementDirection, 0.0f),
        }.Schedule(state.Dependency);

        state.Dependency.Complete();

        foreach (PlayerHealthBufferElement playerHealthBufferElement in playerHealthBuffer)
        {
            playerComponent.ValueRW.Health += playerHealthBufferElement.Value;
        }

        playerHealthBuffer.Clear();
    }
}
