using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SubmitScore : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI inpitScore;
    [SerializeField]
    private TextMeshProUGUI inputName;

    public UnityEvent<string, int> OnScoreSubmitted;

    public void InputScore()
    {
        OnScoreSubmitted.Invoke(inputName.text, int.Parse(inpitScore.text));
    }
}
