using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPointsParticle : MonoBehaviour
{
    [SerializeField] private ScoreController scoreController;
    private void OnParticleCollision(GameObject other)
    {
        int scoreAmount = Random.Range(5, 15);
        scoreController.AddScore(scoreAmount);
        SoundManager.instance.Play("Pop2");
    }
}
