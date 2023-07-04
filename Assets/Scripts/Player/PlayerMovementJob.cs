﻿using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

public partial struct PlayerMovementJob : IJobEntity
{
    [ReadOnly]
    public float3 MovementDirection;

    private void Execute(PlayerAspect playerAspect)
    {
        playerAspect.Move(MovementDirection);
    }
}