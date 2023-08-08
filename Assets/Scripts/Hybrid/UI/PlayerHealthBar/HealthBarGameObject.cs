using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarGameObject : MonoBehaviour
{
    public static Slider Slider;
    [SerializeField]
    private float3 offset;
    public static float3 Offset;

    private void Awake()
    {
        Slider = GetComponent<Slider>();
        Offset = offset;
    }
}