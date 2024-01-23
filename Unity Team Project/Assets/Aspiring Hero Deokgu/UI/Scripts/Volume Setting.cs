using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = WorldSoundManager.Instance.GetBGMSource();
        volumeSlider.value = audioSource.volume;
        volumeSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnValueChanged()
    {
        audioSource.volume = volumeSlider.value;
    }

}
