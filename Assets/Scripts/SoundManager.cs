using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource _audioplayer; //audioplayer du soundmanager

    void Start()
    {
        _audioplayer = GetComponent<AudioSource>();
    }

    public void JouerSon(AudioClip _sound)
    {
        _audioplayer.PlayOneShot(_sound); //joue l<audioclip corespondant au parametre
    }
}
