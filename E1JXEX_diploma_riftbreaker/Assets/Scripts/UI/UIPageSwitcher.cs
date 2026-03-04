using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPageSwitcher : MonoBehaviour
{
    public Button showWeaponUIButton;
    public Button showMechSuitUIButton;
    public Button showBaseUIButton;

    public Canvas weaponCanvas;
    public Canvas mechSuitCanvas;
    public Canvas baseUICanvas;

    PlayerManager playerManager;

    private void Start()
    {
        playerManager = PlayerManager.Instance;
        playerManager.OnMenuToggle += ToggleMenus;
        showWeaponUIButton.onClick.AddListener(ShowWeaponCanvas);
        showMechSuitUIButton.onClick.AddListener(ShowMechsuitCanvas);
        showBaseUIButton.onClick.AddListener(ShowBaseCanvas);

    }

    private void ToggleMenus(bool obj)
    {
        weaponCanvas.enabled = obj;
        mechSuitCanvas.enabled = false;
        baseUICanvas.enabled = false;
        showMechSuitUIButton.gameObject.SetActive(obj);
        showWeaponUIButton.gameObject.SetActive(obj);
        showBaseUIButton.gameObject.SetActive(obj);
    }
    private void ShowMechsuitCanvas()
    {
        weaponCanvas.enabled = false;
        mechSuitCanvas.enabled = true;
        baseUICanvas.enabled = false;
    }
    private void ShowWeaponCanvas()
    {
        weaponCanvas.enabled = true;
        mechSuitCanvas.enabled = false;
        baseUICanvas.enabled = false;
    }

    private void ShowBaseCanvas()
    {
        weaponCanvas.enabled = false;
        mechSuitCanvas.enabled = false;
        baseUICanvas.enabled = true;
    }
}
