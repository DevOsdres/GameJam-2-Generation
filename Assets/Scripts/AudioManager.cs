using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class AudioManager : MonoBehaviour
{
    //Variables para asignar las fuentes de audio.
    private AudioSource sfxAudioSource, musicAudioSource1;
    public static AudioManager Instance { get; private set; }
    [SerializeField] private Slider musicVolume, sfxVolume; 
    private float defaultMusicVolume = 0.7f , defaultSfxVolume = 0.7f; 
    //Lista de archivos para cada uno de los escenarios
    private AudioSource audioSource;
    public AudioClip[] musicClips;
    public string[] sceneNames; 
    private Dictionary<string, AudioClip> musicDictionary;


    //Variable para el cambio de pista de música.
    private bool isPlaying = false;
    
    //Configuración del Singleton
    private void Awake() {
        /*if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this); 
        }*/

        //Crea las fuentes de audio y las almacena. 
        musicAudioSource1 = this.gameObject.AddComponent<AudioSource>();
        sfxAudioSource = this.gameObject.AddComponent<AudioSource>();

        // Loop the music tracks
        musicAudioSource1.loop = true;

        if(PlayerPrefs.HasKey("MusicVolume")){
        //Carga la preferencia de Voulume de la Configuración
        musicVolume.value = PlayerPrefs.GetFloat("MusicVolume");
        }

        else{
            
            musicVolume.value = defaultMusicVolume;
            PlayerPrefs.SetFloat("MusicVolume", defaultMusicVolume);
        }

        if(PlayerPrefs.HasKey("SFXVolume")){

             //Carga la preferencia de Voulume de la Configuración
            sfxVolume.value = PlayerPrefs.GetFloat("SFXVolume");
        }
        else{
            
            //Setea un valor por defecto en caso de no existir uno previo
            sfxVolume.value = defaultMusicVolume;
            PlayerPrefs.SetFloat("SFXVolume", defaultSfxVolume);

        } 

    }

    private void Start() {
        
        audioSource = GetComponent<AudioSource>();
        //Inicializa el dicionario
        musicDictionary = new Dictionary<string, AudioClip>();

        //Carga el diccionario con la lista de escenarios y su respectiva música
        for (int i = 0; i< sceneNames.Length; i++){
            musicDictionary.Add(sceneNames[i],musicClips[i]);
        }

        SceneMusic();
        SceneManager.sceneLoaded += OnSceneLoaded;
         
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mod){ 

        SceneMusic();
    }

    //Volumen de la música
    public void SetVolumeMusicPrefNew()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
    }

    public void SetVolumeSfxPref(){

        PlayerPrefs.SetFloat("SFXVolume", sfxVolume.value);
    }

    //Función que le da play a la música.
    public void SceneMusic(){
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("Escena actual: " + currentScene);

        if (musicDictionary.ContainsKey(currentScene))
        {

            audioSource.clip = musicDictionary[currentScene];
            audioSource.volume = musicVolume.value;
            audioSource.Play();
        }
    }


    //Reproduce los efectos de sonido. 
    public void PlaySFX(AudioClip clip){

        sfxAudioSource.PlayOneShot(clip, sfxVolume.value);
    }

    public void FootSound(AudioClip foots, float velocity)
    {
        if(velocity > 0.1f && !isPlaying)
        {
        sfxAudioSource.clip = foots; 
        sfxAudioSource.Play();
        isPlaying = true;
        }
        else if(isPlaying && !sfxAudioSource.isPlaying)
        {
            isPlaying = false;
        }
    }

}
