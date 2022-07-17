using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    InputControl controls;
    InputStates currentInput;

    void Start()
    {
        controls = GameObject.Find("MainMenuController").GetComponent<InputControl>();
        currentInput = InputStates.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        currentInput = controls.ProcessInput();
    }

    public void PlayButton() {
        SceneManager.LoadScene("GameplayScene");
    }

    public void CreditsButton() {
        SceneManager.LoadScene("CreditsScene");
    }

    public void HowdyaButton() {
        SceneManager.LoadScene("HowdyaScene");
    }
}