using System.Text;
using Unity.Entities;

public partial class PlayerExperienceBarSystem : SystemBase
{
    StringBuilder sb;

    protected override void OnCreate()
    {
        RequireForUpdate<PlayerExperienceComponent>();
        sb = new StringBuilder();
    }

    protected override void OnUpdate()
    {
        PlayerExperienceComponent playerExperienceComponent = SystemAPI.GetSingleton<PlayerExperienceComponent>();

        sb.Append("Lvl. ").Append(playerExperienceComponent.Level);

        PlayerExperienceBarGameObject.Instance.Slider.value = playerExperienceComponent.Experience;
        PlayerExperienceBarGameObject.Instance.Slider.maxValue = playerExperienceComponent.ExperienceToNextLevel;
        PlayerExperienceBarGameObject.Instance.LevelText.text = sb.ToString();

        sb.Clear();
    }
}