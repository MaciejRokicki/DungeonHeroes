using Unity.Mathematics;
using UnityEngine.UI;

public class HealthBarGameObject : Singleton<HealthBarGameObject>
{
    public Slider Slider;
    public float3 Offset;
    public float OffsetMultiplier;

    private void Awake()
    {
        Slider = GetComponent<Slider>();
        Offset *= OffsetMultiplier;
    }
}