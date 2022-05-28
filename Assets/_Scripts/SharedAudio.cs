using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedAudio : MonoBehaviour
{
    private AudioSource _audioSource;
    private int _soundsPlayed;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _soundsPlayed = 0;
        StartCoroutine(ResetCounter());
    }
    
    //Go to place and play sound
    public void PlaySharedSound(AudioClip clip, Vector3 p, float vol, bool important) {
        if (++_soundsPlayed < 5 || important){
            this.transform.position = p;
            _audioSource.PlayOneShot(clip,vol);
        }
    }

    //Reset counter used to avoid playing too many sounds simultaneously
    public IEnumerator ResetCounter(){
        while (true){
            _soundsPlayed = 0;
            yield return WaitManager.Wait(.5f);
        }
    }
}
