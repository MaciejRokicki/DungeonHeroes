using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float MaxHealth;
    public float MovmentSpeed;
    public GameObject[] Weapons;
}

internal class PlayerBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new PlayerInputComponent());
        AddComponent(entity, new PlayerComponent
        {
            Health = authoring.MaxHealth,
            MaxHealth = authoring.MaxHealth,
            MovementSpeed = authoring.MovmentSpeed,
        });
        AddComponent(entity, new PlayerExperienceComponent
        {
            Experience = 0,
            ExperienceToNextLevel = 100,
            Level = 1
        });

        AddBuffer<PlayerHealthBufferElement>(entity);
        DynamicBuffer<PlayerWeaponBufferElement> playerWeaponBuffer = AddBuffer<PlayerWeaponBufferElement>(entity);

        foreach(GameObject weapon in authoring.Weapons)
        {
            playerWeaponBuffer.Add(new PlayerWeaponBufferElement
            {
                Weapon = GetEntity(weapon, TransformUsageFlags.Dynamic),
                AttackTimer = 0.0f
            });
        }
    }
}
