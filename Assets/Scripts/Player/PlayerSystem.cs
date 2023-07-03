using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerSystem : SystemBase, GameInputAction.IPlayerActions
{
    private GameInputAction inputAction;
    private float2 movementDirection;

    protected override void OnStartRunning() => inputAction.Enable();

    protected override void OnStopRunning() => inputAction.Disable();

    protected override void OnCreate()
    {
        RequireForUpdate<PlayerComponent>();

        inputAction = new GameInputAction();
        inputAction.Player.SetCallbacks(this);
    }

    protected override void OnUpdate()
    {
        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        PlayerAspect playerAspect = SystemAPI.GetAspect<PlayerAspect>(playerEntity);

        PlayerMovementJob playerMovementJob = new PlayerMovementJob
        {
            MovementDirection = new float3(movementDirection, 0.0f),
        };

        playerMovementJob.ScheduleParallel();

        //DynamicBuffer<PlayerHealthBufferElement> buffer = SystemAPI.GetSingletonBuffer<PlayerHealthBufferElement>();

        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    buffer.Add(new PlayerHealthBufferElement
        //    {
        //        Value = -1.0f
        //    });
        //}

        //playerAspect.ApplyBuffer();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();
    }
}
