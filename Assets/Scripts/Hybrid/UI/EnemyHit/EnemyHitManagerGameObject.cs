using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyHitManagerGameObject : MonoBehaviour
{
    [SerializeField]
    private Transform canvas;
    [SerializeField]
    private Transform enemyHitText;
    [SerializeField]
    private float3 offset;

    [SerializeField]
    private Vector3 targetPositionOffset;
    [SerializeField]
    private float duration;

    private static Transform Canvas;
    private static Transform EnemyHitText;
    private static float3 Offset;

    public static float Duration;
    public static Vector3 StepTargetPositionOffset;

    public static IObjectPool<Transform> EnemyHitTextPool;

    private void Awake()
    {
        Canvas = canvas;
        EnemyHitText = enemyHitText;
        Offset = offset * 0.02508961f;
        Duration = duration;
        StepTargetPositionOffset = targetPositionOffset / Duration * 0.02508961f;
    }

    private void Start()
    {
        EnemyHitTextPool = new ObjectPool<Transform>(CreatePooledEnemyHitText, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
    }

    public static void DisplayEnemyHit(float3 position, float damage)
    {
        Transform text = EnemyHitTextPool.Get();
        text.transform.position = position + Offset;
        text.GetComponentInChildren<TextMeshProUGUI>().text = damage.ToString();
    }

    #region EnemyHitObjectPool
    private Transform CreatePooledEnemyHitText()
    {
        Transform text = Instantiate(EnemyHitText, Vector3.zero, Quaternion.identity, Canvas);

        return text;
    }

    private void OnReturnedToPool(Transform transform)
    {
        transform.position -= targetPositionOffset;
        transform.gameObject.SetActive(false);
    }

    private void OnTakeFromPool(Transform transform)
    {
        transform.gameObject.SetActive(true);
    }

    private void OnDestroyPoolObject(Transform transform)
    {
        Destroy(transform.gameObject);
    }
    #endregion
}