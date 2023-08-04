using Unity.Entities;
using Unity.Physics.Stateful;
using UnityEngine;

public class WeaponAuthoring : MonoBehaviour
{
    public float Damage;
    public float Speed;
    public float Firerate;
}

internal class WeaponBaker : Baker<WeaponAuthoring>
{
    public override void Bake(WeaponAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new WeaponComponent
        {
            Damage = authoring.Damage,
            Speed = authoring.Speed,
            Firerate = authoring.Firerate
        });

        AddBuffer<StatefulTriggerEvent>(entity);

    }
}
