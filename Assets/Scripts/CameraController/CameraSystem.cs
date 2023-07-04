using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class CameraSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<PlayerComponent>();
    }

    protected override void OnUpdate()
    {
        Entity player = SystemAPI.GetSingletonEntity<PlayerComponent>();
        LocalTransform playerTransform = SystemAPI.GetComponent<LocalTransform>(player);

        float3 pos = math.lerp(
            CameraGameObject.Instance.transform.position, 
            new float3(playerTransform.Position.x, playerTransform.Position.y, -10.0f), 
            0.075f);

        pos = math.clamp(pos,
            new float3(CameraGameObject.ViewClampMin, -10.0f),
            new float3(CameraGameObject.ViewClampMax, -10.0f));

        CameraGameObject.Instance.transform.position = pos;
    }
}
