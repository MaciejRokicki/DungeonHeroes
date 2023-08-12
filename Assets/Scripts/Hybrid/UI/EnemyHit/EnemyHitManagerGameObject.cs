using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyHitManagerGameObject : Singleton<EnemyHitManagerGameObject>
{
    public Transform Canvas;
    public Transform EnemyHitText;
    public float3 Offset;

    public Vector3 TargetPositionOffset;
    public float Duration;
    [HideInInspector]
    public Vector3 StepTargetPositionOffset;

    public IObjectPool<Transform> EnemyHitTextPool;

    private void Awake()
    {
        Offset *= 0.02508961f;
        StepTargetPositionOffset = TargetPositionOffset / Duration * 0.02508961f;
    }

    private void Start()
    {
        EnemyHitTextPool = new ObjectPool<Transform>(CreatePooledEnemyHitText, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject);
    }

    public void DisplayEnemyHit(float3 position, float damage)
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
        transform.position -= TargetPositionOffset;
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