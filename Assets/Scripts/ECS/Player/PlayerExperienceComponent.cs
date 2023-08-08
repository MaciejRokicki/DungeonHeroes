using Unity.Entities;

public struct PlayerExperienceComponent : IComponentData
{
    public int Experience;
    public int ExperienceToNextLevel;
    public int Level;
}