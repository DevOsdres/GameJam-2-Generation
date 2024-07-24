using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Variables para asignar las fuentes de audio.
    [SerializeField] private AudioSource sfxAudioSource, musicAudioSource1, musicAudioSource2;
    public static AudioManager Instance { get; private set; }

    //Variable para el cambio de pista de música.
    private bool firstMusicSourceIsPlaying; 
    
    //Configuración del Singleton
    private void Awake() {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this); 
        }

        //Crea las fuentes de audio y las almacena. 
        musicAudioSource1 = this.gameObject.AddComponent<AudioSource>();
        musicAudioSource2 = this.gameObject.AddComponent<AudioSource>();
        sfxAudioSource = this.gameObject.AddComponent<AudioSource>();

        // Loop the music tracks
        musicAudioSource1.loop = true;
        musicAudioSource2.loop = true; 
    }

    //Función que le da play a la música.
    public void PlayMusic(AudioClip musicClip)
    {
        //Elige que pista de audio empieza a reproducirse.
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicAudioSource1 : musicAudioSource2; 
        activeSource.clip = musicClip;
        activeSource.volume = 1;
        activeSource.Play();  
    }

    //Disolvencia entre musica
    public void PlayMusicWithFade(AudioClip newClip, float musicVolume, float transitionTime = 1.0f)
    {
        //Elige la pista de música que se reproducirá
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicAudioSource1 : musicAudioSource2;

        //Tiempo de transición entre piezas musicales
        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, musicVolume, transitionTime));
    }

    public void PlayMusicWithCrossFade(AudioClip musicClip, float musicVolume, float transitionTime = 1.0f)
    {

        //Determina que fuente de música está activa
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicAudioSource1 : musicAudioSource2;
        AudioSource newSource = (firstMusicSourceIsPlaying) ? musicAudioSource2 : musicAudioSource1;

        // Cambiar la fuente de música.
        firstMusicSourceIsPlaying = !firstMusicSourceIsPlaying;

        //Reproducir la música y empezar la corutina para realizar el crossfade 
        newSource.clip = musicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, musicVolume,transitionTime));
    }

    //Cálculo de la transición entre músicas
    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float musicVolume, float transitionTime)
    {
        //
        if(!activeSource.isPlaying)
        {
            activeSource.Play();
        }

        float t = 0.0f;
        //Fade out
        for (t=0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = (musicVolume - ((t / transitionTime)*musicVolume));
            yield return null;
        }
        
        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play(); 

        //Fade in
        for (t=0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = (t / transitionTime)*musicVolume;
            yield return null;
        }
        
    }

    //Calcular la transisión de la segunda fuente
    private IEnumerator UpdateMusicWithCrossFade(AudioSource original, AudioSource newSource, float musicVolume, float transitionTime)
    {
        float t = 0.0f;
        for(t = 0.0f; t <= transitionTime; t += Time.deltaTime)
        {
            original.volume = (musicVolume - ((t/transitionTime)*musicVolume));
            newSource.volume = (t/transitionTime)*musicVolume;
            yield return null;
        }

        original.Stop();
    }

    //Reproduce los efectos de sonido. 
    public void PlaySFX(AudioClip clip){

        sfxAudioSource.PlayOneShot(clip);
    }

    //Voumen de los efectos de sonido.
    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxAudioSource.PlayOneShot(clip, volume);
    }

    //Variables para configuración del volumen de la música.
    public void SetMusicVolume(float volume)
    {
        musicAudioSource1.volume = volume;
        musicAudioSource2.volume = volume;
    }

     //Variables para configuración del volumen de los efectos de sonido.
    public void SetSFXVolume(float volume)
    {
        sfxAudioSource.volume = volume; 
    }
}
