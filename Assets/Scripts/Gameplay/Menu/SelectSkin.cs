using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SelectSkin : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private Transform skinPrefab;
    public void OnPointerDown(PointerEventData eventData)
    {
        playerData.SelectedSkin = skinPrefab;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        PlatformServices.PlatformUserStats.SetAchievement($"Skin selected: {skinPrefab.name}");
    }
}
