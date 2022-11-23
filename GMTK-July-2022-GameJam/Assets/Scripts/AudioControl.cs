using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Sources and logic for audio control.
*/
public class AudioControl : MonoBehaviour
{
    public static AudioControl instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip hoverDie;
    public AudioClip hoverTile;
    public AudioClip move;


    public enum SoundEffects {
        hoverDie,
        hoverTile,
        move,

    }

    private void Awake() {
        // DontDestroyOnLoad(this.gameObject);

        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void PlaySound(SoundEffects sfx) {
        if (sfx is SoundEffects.hoverDie) sfxSource.PlayOneShot(hoverDie);
        else if (sfx is SoundEffects.hoverTile) sfxSource.PlayOneShot(hoverTile, 1.5f);
        else if (sfx is SoundEffects.move) sfxSource.PlayOneShot(move, 1);
    }
}
