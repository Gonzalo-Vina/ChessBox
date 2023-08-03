using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicGame;
    private float volumenAudio;
    [SerializeField] private Slider sliderVolumen;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sliderVolumen.value = 1f;
        SetVolumen(sliderVolumen.value);
    }

    public void SetVolumen(float value)
    {
        volumenAudio = value;
        musicGame.volume = volumenAudio;
    }
    public float GetVolumenAudio()
    {
        return volumenAudio;
    }
}
