using Unity.Entities;

public partial class PlayerExperienceBarSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<PlayerExperienceComponent>();
    }

    protected override void OnUpdate()
    {
        PlayerExperienceComponent playerExperienceComponent = SystemAPI.GetSingleton<PlayerExperienceComponent>();

        PlayerExperienceBarGameObject.Slider.value = playerExperienceComponent.Experience;
        PlayerExperienceBarGameObject.Slider.maxValue = playerExperienceComponent.ExperienceToNextLevel;
        PlayerExperienceBarGameObject.LevelText.text = playerExperienceComponent.Level.ToString();
    }
}