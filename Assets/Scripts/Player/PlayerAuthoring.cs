using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float MaxHealth;
    public float MovmentSpeed;
}

internal class PlayerBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new PlayerComponent
        {
            Health = authoring.MaxHealth,
            MaxHealth = authoring.MaxHealth,
            MovementSpeed = authoring.MovmentSpeed
        });

        AddBuffer<PlayerHealthBufferElement>(entity);
    }
}
