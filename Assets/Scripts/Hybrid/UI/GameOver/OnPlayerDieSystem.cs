using Unity.Entities;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public partial class OnPlayerDieSystem : SystemBase, GameInputAction.IGameOverActions
{
    GameInputAction gameInputAction;

    protected override void OnCreate()
    {
        RequireForUpdate<OnPlayerDieComponent>();
    }

    protected override void OnUpdate()
    {
        EntityCommandBuffer ecb = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(World.Unmanaged);

        foreach ((OnPlayerDieComponent _, Entity entity) in SystemAPI.Query<OnPlayerDieComponent>().WithEntityAccess())
        {
            GameOverUiGameObject.Instance.ShowGameOverUI();

            gameInputAction = new GameInputAction();
            gameInputAction.Player.Disable();
            gameInputAction.GameOver.Enable();
            gameInputAction.GameOver.SetCallbacks(this);

            ecb.DestroyEntity(entity);
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            gameInputAction.GameOver.Disable();
            gameInputAction.GameOver.RemoveCallbacks(this);
            SceneManager.LoadScene(0);
        }
    }
}