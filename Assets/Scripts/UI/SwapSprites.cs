using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapSprites : MonoBehaviour
{
    [SerializeField] Sprite sprite1;
    [SerializeField] Sprite sprite2;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    public void Swap()
    {
        if (button.image.sprite == sprite1)
        {
            button.image.sprite = sprite2;
        }
        else
        {
            button.image.sprite = sprite1;
        }
    }
}
