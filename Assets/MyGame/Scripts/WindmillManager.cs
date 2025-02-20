using UnityEngine;

public class WindmillManager : MonoBehaviour
{
    [SerializeField] private GameObject[] windmills;
    [SerializeField] private GameObject colorTarget;
    private int activeWindmillIndex = 0;

    private void Start()
    {
        if (windmills.Length > 0)
        {
            windmills[0].GetComponent<Windmill>().EnableSpinning(); // Sicherstellen das erste WINDMILL sich dreht 
        }
    }

    private void Update()
    {
        UpdateLightColors();
        ActivateNextWindmill();
    }

    private void UpdateLightColors()
    {
        for (int i = 0; i < windmills.Length; i++)
        {
            Windmill windmill = windmills[i].GetComponent<Windmill>();
            float intensity = windmill.GetCurrentRotationSpeed() / 255f;
            windmill.windmillLight.color = Color.HSVToRGB((float)i / windmills.Length, 1, intensity);
        }
    }

    public void UpdateFinalColor()
    {
        Color newColor = new Color(0, 0, 0);
        for (int i = 0; i < windmills.Length; i++)
        {
            Windmill windmill = windmills[i].GetComponent<Windmill>();
            if (!windmill.CanSpin())
            {
                float intensity = windmill.GetCurrentRotationSpeed() / 255f;
                Color windmillColor = Color.HSVToRGB((float)i / windmills.Length, 1, intensity);
                newColor += windmillColor;
            }
        }

        newColor = new Color(Mathf.Clamp01(newColor.r), Mathf.Clamp01(newColor.g), Mathf.Clamp01(newColor.b));

        if (colorTarget)
            colorTarget.GetComponent<Renderer>().material.color = newColor;
    }

    private void ActivateNextWindmill()
    {
        if (activeWindmillIndex < windmills.Length && !windmills[activeWindmillIndex].GetComponent<Windmill>().CanSpin())
        {
            activeWindmillIndex++;
        }
        if (activeWindmillIndex < windmills.Length)
        {
            windmills[activeWindmillIndex].GetComponent<Windmill>().EnableSpinning();
        }
    }
}
