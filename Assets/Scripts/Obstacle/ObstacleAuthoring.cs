using Unity.Entities;
using UnityEngine;

public class ObstacleAuthoring : MonoBehaviour
{
    public Vector3 Offset = Vector2.one / 2.0f;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
# endif
}

class ObstacleBaker : Baker<ObstacleAuthoring>
{
    public override void Bake(ObstacleAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new ObstacleComponent
        {
            LeftBotBorder = authoring.transform.position - authoring.transform.localScale / 2.0f - authoring.Offset,
            RightTopBorder = authoring.transform.position + authoring.transform.localScale / 2.0f + authoring.Offset,
        });
    }
}
