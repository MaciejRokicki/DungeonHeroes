using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUiManagerGameObject : Singleton<WeaponUiManagerGameObject>
{
    [SerializeField]
    private Transform canvas;
    [SerializeField]
    private Transform container;
    [SerializeField]
    private Transform weaponUiPrefab;
    private List<Transform> weaponUis;

    private IEnumerator Start()
    {
        weaponUis = new List<Transform>();

        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        yield return new WaitForSeconds(0.2f);

        DynamicBuffer<WeaponBufferElement> weaponBuffer = entityManager
            .CreateEntityQuery(typeof(WeaponManagerComponent), typeof(WeaponBufferElement))
            .GetSingletonBuffer<WeaponBufferElement>();

        foreach (WeaponBufferElement weapon in weaponBuffer)
        {
            SpriteRenderer spriteRenderer = entityManager.GetComponentObject<SpriteRenderer>(weapon.Weapon);
            AddWeaponUI(spriteRenderer.sprite);
        }

        RevealWeapon(0);
    }

    private void AddWeaponUI(Sprite sprite)
    {
        Transform weaponUi = Instantiate(weaponUiPrefab, container);
        weaponUi.GetComponent<Image>().sprite = sprite;

        weaponUis.Add(weaponUi);
    }

    public void RevealWeapon(int id)
    {
        Transform weaponUi = weaponUis[id];
        weaponUi.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }
}
