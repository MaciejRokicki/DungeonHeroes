using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperienceBarGameObject : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    public static Slider Slider;
    [SerializeField]
    private TextMeshProUGUI levelText;
    public static TextMeshProUGUI LevelText;

    private void Awake()
    {
        Slider = slider;
        LevelText = levelText;
    }
}