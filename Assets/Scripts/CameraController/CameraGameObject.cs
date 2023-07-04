using UnityEngine;

public class CameraGameObject : MonoBehaviour
{
    public static Camera Instance;
    public static Vector2 ViewClampMin;
    public static Vector2 ViewClampMax;

    [SerializeField]
    private Vector2 MapSize = new Vector2(30.0f, 15.0f);

    private void Awake()
    {
        Instance = GetComponent<Camera>();

        Vector2 CameraViewSize = new Vector2(
            Instance.orthographicSize * 2.0f * Instance.aspect,
            Instance.orthographicSize * 2.0f);

        ViewClampMin = new Vector2(CameraViewSize.x / 2.0f - MapSize.x, CameraViewSize.y / 2.0f - 15.0f);
        ViewClampMax = new Vector2(MapSize.x - CameraViewSize.x / 2.0f, MapSize.y + 1.0f - CameraViewSize.y / 2.0f);
    }
}
