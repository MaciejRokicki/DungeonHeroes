using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerInputSystem : SystemBase, GameInputAction.IPlayerActions
{
    private GameInputAction inputAction;
    private float2 movementDirection;

    public void OnMovement(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();
    }

    protected override void OnStartRunning() => inputAction.Enable();

    protected override void OnStopRunning() => inputAction.Disable();

    protected override void OnCreate()
    {
        RequireForUpdate<PlayerInputComponent>();

        inputAction = new GameInputAction();
        inputAction.Player.SetCallbacks(this);
    }

    protected override void OnUpdate()
    {
        RefRW<PlayerInputComponent> playerInputComponent = SystemAPI.GetSingletonRW<PlayerInputComponent>();

        playerInputComponent.ValueRW.MovementDirection = movementDirection;
    }
}