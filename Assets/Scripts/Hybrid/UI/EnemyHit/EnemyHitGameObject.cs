using UnityEngine;

public class EnemyHitGameObject : MonoBehaviour
{
    private float timer = 0.0f;

    private void Update()
    {
        timer += Time.deltaTime;

        transform.position = transform.position + EnemyHitManagerGameObject.StepTargetPositionOffset * Time.deltaTime;

        if (timer > EnemyHitManagerGameObject.Duration)
        {
            timer = 0.0f;
            EnemyHitManagerGameObject.EnemyHitTextPool.Release(GetComponent<Transform>());
        }
    }
}