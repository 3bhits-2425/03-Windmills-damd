using UnityEngine;
using UnityEngine.UI;

public class Windmill : MonoBehaviour
{
    public Slider speedSlider;
    public Button lockButton;
    public Light windmillLight;
    private float rotationSpeed = 0f;
    private bool isLocked = false;
    private float maxRotationSpeed = 255f;
    private float decreaseRate = 100f;
    private bool canSpin = false;

    public void Initialize(Slider slider, Button button, Light light)
    {
        speedSlider = slider;
        lockButton = button;
        windmillLight = light;
        lockButton.onClick.AddListener(LockWindmill);
    }

    private void Update()
    {
        if (canSpin && !isLocked)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                AdjustSpeed(Time.deltaTime * 100f);
            }
            else
            {
                AdjustSpeed(-Time.deltaTime * decreaseRate);
            }
        }
        RotateWindmill();
    }


    private void AdjustSpeed(float delta)
    {
        rotationSpeed = Mathf.Clamp(rotationSpeed + delta, 0, maxRotationSpeed);
        if (speedSlider) speedSlider.value = rotationSpeed;
    }
    private void RotateWindmill()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    public void LockWindmill()
    {
        Debug.Log("Windmill locked!");
        isLocked = true;
        canSpin = false;
        FindObjectOfType<WindmillManager>().UpdateFinalColor();
    }

    public void EnableSpinning()
    {
        canSpin = true;
    }

    public bool CanSpin()
    {
        return canSpin && !isLocked;
    }

    public float GetCurrentRotationSpeed()
    {
        return rotationSpeed;
    }
}