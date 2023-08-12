using Unity.Entities;
using Unity.Transforms;

public partial class PlayerHealthBarSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<PlayerComponent>();
    }

    protected override void OnStopRunning()
    {
       HealthBarGameObject.Instance.Slider?.gameObject.SetActive(false);
    }

    protected override void OnUpdate()
    {
        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        LocalTransform playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
        PlayerComponent playerComponent = SystemAPI.GetComponent<PlayerComponent>(playerEntity);

        HealthBarGameObject.Instance.Slider.transform.position = playerTransform.Position + HealthBarGameObject.Instance.Offset;
        HealthBarGameObject.Instance.Slider.value = playerComponent.Health;
        HealthBarGameObject.Instance.Slider.maxValue = playerComponent.MaxHealth;
    }
}