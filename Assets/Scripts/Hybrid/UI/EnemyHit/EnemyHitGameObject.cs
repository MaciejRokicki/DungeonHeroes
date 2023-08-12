using UnityEngine;

public class EnemyHitGameObject : MonoBehaviour
{
    private EnemyHitManagerGameObject enemyHitManagerGameObject;
    private float timer = 0.0f;

    private void Awake()
    {
        enemyHitManagerGameObject = EnemyHitManagerGameObject.Instance;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        transform.position = transform.position + enemyHitManagerGameObject.StepTargetPositionOffset * Time.deltaTime;

        if (timer > enemyHitManagerGameObject.Duration)
        {
            timer = 0.0f;
            enemyHitManagerGameObject.EnemyHitTextPool.Release(GetComponent<Transform>());
        }
    }
}