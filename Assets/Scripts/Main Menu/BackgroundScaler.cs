using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundScaler : MonoBehaviour
{
    Image backgroundImage;
    RectTransform rect;
    float ratio;

    // Start is called before the first frame update
    void Start()
    {
        backgroundImage = GetComponent<Image>();
        rect = backgroundImage.rectTransform;
        ratio = backgroundImage.sprite.bounds.size.x / backgroundImage.sprite.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!rect)
        {
            return;
        }

        // scale image proportionally to fit the screen
        if (Screen.height * ratio >= Screen.width)
        {
            rect.sizeDelta = new Vector2(Screen.height * ratio, Screen.height);
        }
        else
        {
            rect.sizeDelta = new Vector2(Screen.width, Screen.width / ratio);
        }
    }
}
