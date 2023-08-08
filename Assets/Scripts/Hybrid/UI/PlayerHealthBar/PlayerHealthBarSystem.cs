using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class PlayerHealthBarSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<PlayerComponent>();
    }

    protected override void OnStopRunning()
    {
        HealthBarGameObject.Slider.gameObject.SetActive(false);
    }

    protected override void OnUpdate()
    {
        Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerComponent>();
        LocalTransform playerTransform = SystemAPI.GetComponent<LocalTransform>(playerEntity);
        PlayerComponent playerComponent = SystemAPI.GetComponent<PlayerComponent>(playerEntity);

        HealthBarGameObject.Slider.transform.position = playerTransform.Position + new float3(0.0f, -30.0f, 0.0f) * 0.02508961f;
        HealthBarGameObject.Slider.value = playerComponent.Health;
        HealthBarGameObject.Slider.maxValue = playerComponent.MaxHealth;
    }
}