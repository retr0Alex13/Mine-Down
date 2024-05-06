using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSprites : MonoBehaviour
{
    private static bool isTutorialSpritesShown;

    void Start()
    {
        if (!isTutorialSpritesShown)
        {
            gameObject.SetActive(true);
            isTutorialSpritesShown = true;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
