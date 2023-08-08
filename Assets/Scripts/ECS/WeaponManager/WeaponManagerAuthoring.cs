using Unity.Entities;
using UnityEngine;

public class WeaponManagerAuthoring : MonoBehaviour
{
    public GameObject[] Weapons;
}

internal class WeaponManagerBaker : Baker<WeaponManagerAuthoring>
{
    public override void Bake(WeaponManagerAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new WeaponManagerComponent());

        DynamicBuffer<WeaponBufferElement> weaponBuffer = AddBuffer<WeaponBufferElement>(entity);

        foreach (GameObject weapon in authoring.Weapons)
        {
            weaponBuffer.Add(new WeaponBufferElement
            {
                Weapon = GetEntity(weapon, TransformUsageFlags.None),
            });
        }
    }
}