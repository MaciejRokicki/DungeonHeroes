using Unity.Entities;

public struct EnemyComponent : IComponentData
{
    public float Health;
    public float MaxHealth;
    public float Damage;
    public float MovementSpeed;
    public float HitTimer;
}
