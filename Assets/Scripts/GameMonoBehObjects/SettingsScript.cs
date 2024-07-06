using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TMP_Dropdown musicDropdown;
    [SerializeField] private Slider volumeSlider;

    private AudioClip[] audioClips;
    private void Start()
    {
        audioClips = Resources.LoadAll<AudioClip>("Music");
        
        musicDropdown.options.Clear();
        foreach (AudioClip clip in audioClips)
        {
            musicDropdown.options.Add(new TMP_Dropdown.OptionData(clip.name));
        }
        
        musicDropdown.onValueChanged.AddListener(ChangeMusic);
        volumeSlider.onValueChanged.AddListener(ChangeVolume);

        volumeSlider.value = Constants.InitialVolume;
        int startMusicNum = 0;
        musicDropdown.captionText.text = musicDropdown.options[startMusicNum].text;
        ChangeMusic(startMusicNum);
        ChangeVolume(volumeSlider.value);
    }
    
    private void ChangeMusic(int index)
    {
        if (index < audioClips.Length)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
    }

    private void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
