using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapSprites : MonoBehaviour
{
    [SerializeField] Sprite mutedSprite;
    [SerializeField] Sprite unmutedSprite;

    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        SetStartButtonSprite();
    }

    private void SetStartButtonSprite()
    {
        if (GameManager.Instance.IsMuted)
        {
            button.image.sprite = mutedSprite;
        }
        else
        {
            button.image.sprite = unmutedSprite;
        }
    }

    public void Swap()
    {
        if (button.image.sprite == mutedSprite)
        {
            button.image.sprite = unmutedSprite;
        }
        else
        {
            button.image.sprite = mutedSprite;
        }
    }
}
