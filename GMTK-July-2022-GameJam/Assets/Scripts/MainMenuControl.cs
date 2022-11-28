using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Gross hack to finish this project up
public class MainMenuControl : MonoBehaviour
{
    private AudioControl AUDIO;

    private GameObject tutorialImage;
    private GameObject tutorialLeftArrow;
    private GameObject tutorialRightArrow;
    private GameObject tutorialMainMenuButton;
    private GameObject smoke;

    private enum TutorialSprites {
        image1,
        image2,
        image3,
        image4,
        image5
    }

    // Tutorial Source Images
    private TutorialSprites currentTutorialImage;
    public Sprite tutorialImage1;
    public Sprite tutorialImage2;
    public Sprite tutorialImage3;
    public Sprite tutorialImage4;
    public Sprite tutorialImage5;

    private void Start() {
        GameObject audioManagerObject = GameObject.Find("AudioManager");
        AUDIO = audioManagerObject.GetComponent<AudioControl>();

        tutorialImage = GameObject.Find("TutorialImage");
        tutorialLeftArrow = GameObject.Find("LeftArrow");
        tutorialRightArrow = GameObject.Find("RightArrow");
        tutorialMainMenuButton = GameObject.Find("MainMenuButton");
        smoke = GameObject.Find("Smoke");
        HideHowdyaMenu();
    }
    public void PlayButton() {
        SceneManager.LoadScene("GameplayScene");
    }

    public void HowdyaButton() {
        AUDIO.PlaySound(AudioControl.SoundEffects.hoverTile);
        AUDIO.SetBackgroundSound(AudioControl.BackgroundEffects.tutorial);
        ShowHowdyaMenu();
    }

    public void ExitButton() {
        Debug.Log("Game Quit");
        Application.Quit();
    }

    public void MainMenuButton() {
        AUDIO.PlaySound(AudioControl.SoundEffects.hoverTile);
        AUDIO.SetBackgroundSound(AudioControl.BackgroundEffects.mainMenu);
        HideHowdyaMenu();
    }
    
    public void LeftArrowButton() {
        if (currentTutorialImage == TutorialSprites.image2) currentTutorialImage = TutorialSprites.image1;
        else if (currentTutorialImage == TutorialSprites.image3) currentTutorialImage = TutorialSprites.image2;
        else if (currentTutorialImage == TutorialSprites.image4) currentTutorialImage = TutorialSprites.image3;
        else if (currentTutorialImage == TutorialSprites.image5) currentTutorialImage = TutorialSprites.image4;
        AUDIO.PlaySound(AudioControl.SoundEffects.hoverTile);
        SetTutorialImage();
    }

    public void RightArrowButton() {
        if (currentTutorialImage == TutorialSprites.image1) currentTutorialImage = TutorialSprites.image2;
        else if (currentTutorialImage == TutorialSprites.image2) currentTutorialImage = TutorialSprites.image3;
        else if (currentTutorialImage == TutorialSprites.image3) currentTutorialImage = TutorialSprites.image4;
        else if (currentTutorialImage == TutorialSprites.image4) currentTutorialImage = TutorialSprites.image5;
        AUDIO.PlaySound(AudioControl.SoundEffects.hoverTile);
        SetTutorialImage();
    }

    private void SetTutorialImage() {
        Image image = tutorialImage.GetComponent<Image>();
        if (currentTutorialImage == TutorialSprites.image1) image.sprite = tutorialImage1;
        else if (currentTutorialImage == TutorialSprites.image2) image.sprite = tutorialImage2;
        else if (currentTutorialImage == TutorialSprites.image3) image.sprite = tutorialImage3;
        else if (currentTutorialImage == TutorialSprites.image4) image.sprite = tutorialImage4;
        else if (currentTutorialImage == TutorialSprites.image5) image.sprite = tutorialImage5;
    }

    private void HideHowdyaMenu() {
        tutorialImage.SetActive(false);
        tutorialLeftArrow.SetActive(false);
        tutorialRightArrow.SetActive(false);
        tutorialMainMenuButton.SetActive(false);
        smoke.SetActive(true);
    }

    private void ShowHowdyaMenu() {
        currentTutorialImage = TutorialSprites.image1;
        SetTutorialImage();
        tutorialImage.SetActive(true);
        tutorialLeftArrow.SetActive(true);
        tutorialRightArrow.SetActive(true);
        tutorialMainMenuButton.SetActive(true);
        smoke.SetActive(false);
    }
}