using UnityEngine;
using UnityEngine.UI;

public class WindmillGameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] windmills;
    [SerializeField] private Slider[] windmillSliders;
    [SerializeField] private Button lockButton;
    [SerializeField] private GameObject colorTarget;
    [SerializeField] private Light[] windmillLights; // Array for Point Lights

    private int currentWindmillIndex = 0;
    private float[] windmillSpeeds = new float[3];
    private bool[] isLocked = new bool[3];
    private bool allLocked = false;
    private float maxRotationSpeed = 255f;
    private float decreaseRate = 100f;

    private void Start()
    {
        lockButton.onClick.AddListener(LockCurrentWindmill);
    }

    private void Update()
    {
        if (!allLocked && currentWindmillIndex < windmills.Length)
        {
            if (!isLocked[currentWindmillIndex])
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    IncreaseWindmillValue(Time.deltaTime);
                }
                else
                {
                    DecreaseWindmillValue(Time.deltaTime);
                }
            }
        }
        RotateWindmills();
    }

    private void IncreaseWindmillValue(float deltaTime)
    {
        float newValue = Mathf.Clamp(windmillSpeeds[currentWindmillIndex] + (deltaTime * 100f), 0, maxRotationSpeed);
        windmillSpeeds[currentWindmillIndex] = newValue;
        windmillSliders[currentWindmillIndex].value = newValue;
        UpdateWindmillLightColor(currentWindmillIndex);
    }

    private void DecreaseWindmillValue(float deltaTime)
    {
        if (!isLocked[currentWindmillIndex])
        {
            float newValue = Mathf.Clamp(windmillSpeeds[currentWindmillIndex] - (deltaTime * decreaseRate), 0, maxRotationSpeed);
            windmillSpeeds[currentWindmillIndex] = newValue;
            windmillSliders[currentWindmillIndex].value = newValue;
            UpdateWindmillLightColor(currentWindmillIndex);
        }
    }

    private void RotateWindmills()
    {
        for (int i = 0; i < windmills.Length; i++)
        {
            float speed = windmillSpeeds[i];
            windmills[i].transform.Rotate(0, 0, speed * Time.deltaTime);
        }
    }

    public void LockCurrentWindmill()
    {
        if (currentWindmillIndex < windmills.Length && !isLocked[currentWindmillIndex])
        {
            isLocked[currentWindmillIndex] = true;

            if (currentWindmillIndex < windmills.Length - 1)
            {
                currentWindmillIndex++;
            }
            else
            {
                allLocked = true;
                ApplyColor();
            }
        }
    }

    private void ApplyColor()
    {
        Color newColor = new Color(windmillSpeeds[0] / 255f, windmillSpeeds[1] / 255f, windmillSpeeds[2] / 255f);
        colorTarget.GetComponent<Renderer>().material.color = newColor;
    }

    private void UpdateWindmillLightColor(int index)
    {
        if (index < windmillLights.Length && windmillLights[index] != null)
        {
            Color lightColor = Color.black;
            if (index == 0)
                lightColor = new Color(windmillSpeeds[index] / 255f, 0, 0); // Red
            else if (index == 1)
                lightColor = new Color(0, windmillSpeeds[index] / 255f, 0); // Green
            else if (index == 2)
                lightColor = new Color(0, 0, windmillSpeeds[index] / 255f); // Blue

            windmillLights[index].color = lightColor;
        }
    }
}
