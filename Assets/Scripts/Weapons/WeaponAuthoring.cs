using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Stateful;
using UnityEngine;

public class WeaponAuthoring : MonoBehaviour
{
    public float Damage;
    public float Speed;
    public float Firerate;
    public float3[] ShotDirections;
}

internal class WeaponBaker : Baker<WeaponAuthoring>
{
    public override void Bake(WeaponAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);

        BlobAssetReference<WeaponStatistics> weaponStatistics = PrepareWeaponStatistics(authoring);

        AddComponent(entity, new WeaponComponent
        {
            WeaponStatisticsBlob = weaponStatistics
        });

        AddBuffer<StatefulTriggerEvent>(entity);
    }

    private BlobAssetReference<WeaponStatistics> PrepareWeaponStatistics(WeaponAuthoring authoring)
    {
        BlobBuilder blobBuilder = new BlobBuilder(Allocator.Temp);

        ref WeaponStatistics weaponStatistics = ref blobBuilder.ConstructRoot<WeaponStatistics>();

        weaponStatistics.Damage = authoring.Damage;
        weaponStatistics.Speed = authoring.Speed;
        weaponStatistics.Firerate = authoring.Firerate;

        BlobBuilderArray<float3> shotDirectionsBlob = blobBuilder.Allocate(ref weaponStatistics.ShotDirections, authoring.ShotDirections.Length);

        for(int i = 0; i < authoring.ShotDirections.Length; i++)
        {
            shotDirectionsBlob[i] = authoring.ShotDirections[i];
        }

        BlobAssetReference<WeaponStatistics> result = blobBuilder.CreateBlobAssetReference<WeaponStatistics>(Allocator.Persistent);

        blobBuilder.Dispose();

        return result;
    }
}
