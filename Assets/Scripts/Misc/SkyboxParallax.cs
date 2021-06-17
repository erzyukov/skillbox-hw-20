using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

/// <summary>
/// Компонент вращает Skybox в зависимости от положения мыши (параллакс эффект)
/// </summary>
public class SkyboxParallax : MonoBehaviour
{
    [SerializeField] Volume volume = null;
    [SerializeField] float parallaxMaxOffset = 2;

    private float startRotation;

    private void Start()
    {
        HDRISky sky;
        volume.profile.TryGet(out sky);
        startRotation = sky.rotation.value;
    }

    // Update is called once per frame
    private void Update()
    {
        float offset = Input.mousePosition.x - (Screen.width / 2);
        offset = - offset * parallaxMaxOffset / (Screen.width / 2);
        SetHdriRotation(startRotation + offset);
    }

    private void SetHdriRotation(float rot)
    {
        HDRISky sky;
        volume.profile.TryGet(out sky);
        sky.rotation.value = rot;
    }
}
