using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPowerUpController : MonoBehaviour
{
    //[SerializeField] private Image Capacity = null;
    [SerializeField] private Image Energy = null;
    [SerializeField] private Image ForceFeild = null;
    [SerializeField] private Image Stabilize = null;
    [SerializeField] private Image Lantern = null;
    [SerializeField] private Image Invisible = null;

    [SerializeField] private Image powerUpBackground = null;

    private void Start()
    {
        ResetIcon();
    }

    public void SetIcon(PowerUpItem.Type type)
    {
        ResetIcon();
        powerUpBackground.color = new Color32(255, 255, 140, 100);
        switch (type)
        {
            case PowerUpItem.Type.ForceFeld:
                ForceFeild.gameObject.SetActive(true);
                break;
            case PowerUpItem.Type.EnergyBattery:
                Energy.gameObject.SetActive(true);
                break;
            //case PowerUpItem.Type.EnergyCapacity:
            //    Capacity.gameObject.SetActive(true);
            //    break;
            case PowerUpItem.Type.Stabilize:
                Stabilize.gameObject.SetActive(true);
                break;
            case PowerUpItem.Type.Lantern:
                Lantern.gameObject.SetActive(true);
                break;
            case PowerUpItem.Type.Invisible:
                Invisible.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void TurnOn()
    {
        powerUpBackground.color = new Color32(100, 220, 50, 100);
    }

    public void TurnOff()
    {
        powerUpBackground.color = new Color32(255, 255, 140, 100);
    }

    public void ResetIcon()
    {
        powerUpBackground.color = new Color32(255, 255, 255, 100);
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(false);
        }
    }

}
