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

        NativeArray<ObstacleComponent> obstacles = SystemAPI.QueryBuilder()
            .WithAspect<ObstacleAspect>()
            .Build()
            .ToComponentDataArray<ObstacleComponent>(Allocator.TempJob);

        PlayerMovementJob playerMovementJob = new PlayerMovementJob
        {
            movementDirection = new float3(movementDirection, 0.0f) * playerAspect.MovementSpeed * SystemAPI.Time.DeltaTime,
            obstacleAspects = obstacles
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
