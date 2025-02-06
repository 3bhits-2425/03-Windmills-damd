using UnityEngine;
using UnityEngine.UI;

public class WindmillGameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] windmills;
    [SerializeField] private Slider[] windmillSliders;
    [SerializeField] private Button[] lockButtons;
    [SerializeField] private GameObject colorTarget;
    [SerializeField] private Light[] windmillLights; // Array for Point Lights

    private float[] windmillSpeeds = new float[3];
    private bool[] isLocked = new bool[3];
    private bool allLocked = false;
    private float maxRotationSpeed = 255f;
    private float decreaseRate = 100f;

    private void Start()
    {
        for (int i = 0; i < lockButtons.Length; i++)
        {
            int index = i; // Capture index for lambda expression
            lockButtons[i].onClick.AddListener(() => LockWindmill(index));
        }
    }

    private void Update()
    {
        for (int i = 0; i < windmills.Length; i++)
        {
            if (!isLocked[i])
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    IncreaseWindmillValue(i, Time.deltaTime);
                }
                else
                {
                    DecreaseWindmillValue(i, Time.deltaTime);
                }
            }
        }
        RotateWindmills();
    }

    private void IncreaseWindmillValue(int index, float deltaTime)
    {
        float newValue = Mathf.Clamp(windmillSpeeds[index] + (deltaTime * 100f), 0, maxRotationSpeed);
        windmillSpeeds[index] = newValue;
        windmillSliders[index].value = newValue;
        UpdateWindmillLightColor(index);
    }

    private void DecreaseWindmillValue(int index, float deltaTime)
    {
        if (!isLocked[index])
        {
            float newValue = Mathf.Clamp(windmillSpeeds[index] - (deltaTime * decreaseRate), 0, maxRotationSpeed);
            windmillSpeeds[index] = newValue;
            windmillSliders[index].value = newValue;
            UpdateWindmillLightColor(index);
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

    public void LockWindmill(int index)
    {
        if (!isLocked[index])
        {
            isLocked[index] = true;
            allLocked = CheckAllLocked();
            if (allLocked)
            {
                ApplyColor();
            }
        }
    }

    private bool CheckAllLocked()
    {
        foreach (bool locked in isLocked)
        {
            if (!locked) return false;
        }
        return true;
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
