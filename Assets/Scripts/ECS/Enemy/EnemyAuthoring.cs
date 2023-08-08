using Unity.Entities;
using UnityEngine;

public class EnemyAuthoring : MonoBehaviour
{
    public float MaxHealth;
    public float Damage;
    public float MovementSpeed;
}

public class EnemyBaker : Baker<EnemyAuthoring>
{
    public override void Bake(EnemyAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new EnemyComponent
        {
            Health = authoring.MaxHealth,
            MaxHealth = authoring.MaxHealth,
            Damage = authoring.Damage,
            MovementSpeed = authoring.MovementSpeed,
        });
    }
}