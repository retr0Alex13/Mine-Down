using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowButtons : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;

    public void Show()
    {
        foreach (var button in buttons)
        {
            button.SetActive(true);
        }
    }
}
