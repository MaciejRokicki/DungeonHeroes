using Unity.Entities;

public struct PlayerComponent : IComponentData
{
    public float Health;
    public float MaxHealth;
    public float MovementSpeed;
    public Entity Weapon;
    public float AttackTimer;
}
