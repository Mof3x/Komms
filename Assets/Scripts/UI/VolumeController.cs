using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider;

    private const string VolumePrefKey = "MasterVolumeSlider";

    void Start()
    {
        if (volumeSlider == null)
        {
            Debug.LogWarning("VolumeController: volumeSlider is not assigned.");
            return;
        }

        if (PlayerPrefs.HasKey(VolumePrefKey))
        {
            float savedValue = PlayerPrefs.GetFloat(VolumePrefKey);
            volumeSlider.value = savedValue;
        }

        ApplyVolume(volumeSlider.value);
        volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnDestroy()
    {
        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }

    void OnSliderValueChanged(float value)
    {
        ApplyVolume(value);
        PlayerPrefs.SetFloat(VolumePrefKey, value);
        PlayerPrefs.Save();
    }

    void ApplyVolume(float value)
    {
        AudioListener.volume = value;
    }
}