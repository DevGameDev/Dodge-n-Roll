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

    public AudioClip mainMenuTheme;
    public AudioClip tutorialTheme;
    public AudioClip gameplayTheme;

    public AudioClip hoverDie;
    public AudioClip hoverTile;
    public AudioClip move;

    public enum SoundEffects {
        hoverDie,
        hoverTile,
        move,
    }
    
    public enum BackgroundEffects {
        mainMenu,
        tutorial,
        gameplay
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
        else if (sfx is SoundEffects.hoverTile) sfxSource.PlayOneShot(hoverTile);
        else if (sfx is SoundEffects.move) sfxSource.PlayOneShot(move);
    }

    public void SetBackgroundSound(BackgroundEffects sfx) {
        if (sfx is BackgroundEffects.mainMenu) {
            musicSource.volume = 0.7f;
            musicSource.clip = mainMenuTheme;
        }
        else if (sfx is BackgroundEffects.tutorial) {
            musicSource.volume = 0.15f;
            musicSource.clip = tutorialTheme;
        }
        else if (sfx is BackgroundEffects.gameplay) musicSource.clip = gameplayTheme;
        musicSource.Play();
    }
}
