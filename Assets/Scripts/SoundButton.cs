using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioClip putOnMouseSound;
    private AudioSource fuenteAudio;

    void Start()
    {
        fuenteAudio = GetComponent<AudioSource>();
    }
    public void OnButtonClick()
    {
        AudioManager2.Instance.PlaySFX(clickSound);
    }

    public void PutOnButton()
    {
        AudioManager2.Instance.PlaySFX(putOnMouseSound);
    }
    
}
