using Unity.Entities;

public struct PlayerWeaponBufferElement : IBufferElementData
{
    public Entity Weapon;
    public float AttackTimer;
}