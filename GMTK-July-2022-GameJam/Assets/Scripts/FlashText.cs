using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlashText : MonoBehaviour
{
    TextMeshProUGUI guiText;
    public float speed = 1.0f;

    void Start() 
    {
        guiText = gameObject.GetComponent<TextMeshProUGUI>();    
    }
    private void FixedUpdate() {
        guiText.alpha = Mathf.PingPong(Time.fixedTime * speed, 1.0f);
    }
}
