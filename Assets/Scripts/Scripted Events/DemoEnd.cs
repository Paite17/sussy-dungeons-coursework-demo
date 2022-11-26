using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DemoEnd : MonoBehaviour
{
    public TMP_Text endText;

    // colour values
    private float colour1;
    private float colour2;
    private float colour3;
    private const float TRANSPARENT = 255f;

    private void Start()
    {
        endText.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        endText.color = new Color(colour1, colour2, colour3, TRANSPARENT);
    }

    // every few frames set the colour values to a random range of 0-255 and then set the text colour to them
    private void FixedUpdate()
    {
        colour1 = (float)UnityEngine.Random.Range(0, 255);
        colour2 = (float)UnityEngine.Random.Range(0, 255);
        colour3 = (float)UnityEngine.Random.Range(0, 255);

        //endText = GameObject.Find("EndText").GetComponent<TextMeshProUGUI>();
        
    }
}
