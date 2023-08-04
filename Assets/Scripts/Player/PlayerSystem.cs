using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class PlayerSystem : SystemBase, GameInputAction.IPlayerActions
{
    private GameInputAction inputAction;
    private float2 movementDirection;
    private readonly NativeArray<float3> directions = new NativeArray<float3>(new float3[8]
    {
        new float3(0.0f, 1.0f, 0.0f),
        new float3(1.0f, 1.0f, 0.0f),
        new float3(1.0f, 0.0f, 0.0f),
        new float3(1.0f, -1.0f, 0.0f),
        new float3(0.0f, -1.0f, 0.0f),
        new float3(-1.0f, -1.0f, 0.0f),
        new float3(-1.0f, 0.0f, 0.0f),
        new float3(-1.0f, 1.0f, 0.0f)
    }, Allocator.TempJob);

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
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);
        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        PlayerAspect playerAspect = SystemAPI.GetAspect<PlayerAspect>(playerEntity);

        PlayerMovementJob playerMovementJob = new PlayerMovementJob
        {
            MovementDirection = new float3(movementDirection, 0.0f),
        };

        Dependency = playerMovementJob.Schedule(Dependency);

        Dependency.Complete();

        playerAspect.ApplyHealthBuffer();

        DestroyPlayerJob destroyPlayerJob = new DestroyPlayerJob
        {
            Ecb = ecb
        };

        Dependency = destroyPlayerJob.Schedule(Dependency);

        Dependency.Complete();

        PlayerWeaponSpawnJob playerWeaponSpawnJob = new PlayerWeaponSpawnJob
        {
            Ecb = ecb,
            DeltaTime = SystemAPI.Time.DeltaTime,
            WeaponCompnent = SystemAPI.GetComponent<WeaponComponent>(SystemAPI.GetComponent<PlayerComponent>(playerEntity).Weapon),
            AttackDirections = directions,
            WeaponEntity = SystemAPI.GetComponent<PlayerComponent>(playerEntity).Weapon,
        };

        Dependency = playerWeaponSpawnJob.Schedule(Dependency);

        Dependency.Complete();

        playerAspect.ApplyAttackTimerBuffer();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>();
    }
}
