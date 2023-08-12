using UnityEngine;

public class CameraGameObject : Singleton<CameraGameObject>
{
    [HideInInspector]
    public Camera Camera;
    [HideInInspector]
    public Vector2 CameraViewSize;
    [HideInInspector]
    public Vector2 ViewClampMin;
    [HideInInspector]
    public Vector2 ViewClampMax;

    [SerializeField]
    private Vector2 MapSize = new Vector2(30.0f, 15.0f);

    private void Awake()
    {
        Camera = GetComponent<Camera>();

        CameraViewSize = new Vector2(
            Camera.orthographicSize * 2.0f * Camera.aspect,
            Camera.orthographicSize * 2.0f);

        ViewClampMin = new Vector2(CameraViewSize.x / 2.0f - MapSize.x, CameraViewSize.y / 2.0f - 15.0f);
        ViewClampMax = new Vector2(MapSize.x - CameraViewSize.x / 2.0f, MapSize.y + 1.0f - CameraViewSize.y / 2.0f);
    }
}
