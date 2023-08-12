using Unity.Entities.Serialization;
using UnityEngine;

public class GameOverUiGameObject : Singleton<GameOverUiGameObject>
{
    [SerializeField]
    private GameObject gameOverUi;

    public void ShowGameOverUI()
    {
        gameOverUi.SetActive(true);
    }
}