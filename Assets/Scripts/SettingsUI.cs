using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Transform volumePercentageTMPro;
    public AudioMixer audioMixer;   

    void Awake()
    {
        volumePercentageTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefs.GetInt("volumeSliderPercent") + "%";
        volumeSlider.value = PlayerPrefs.GetFloat("volume");  
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolume(float volume)
    {
        int volumeSliderPercent = (int) (volume - volumeSlider.minValue);
        volumePercentageTMPro.GetComponent<TMPro.TextMeshProUGUI>().text = volumeSliderPercent.ToString() + "%";
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.SetInt("volumeSliderPercent", volumeSliderPercent);
        /*Debug.Log(volume);
        Debug.Log(volumeSliderPercent);*/
    }
}
