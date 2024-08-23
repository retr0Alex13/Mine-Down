using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSelector : MonoBehaviour
{
    [SerializeField] private GameObject[] particles;

    private void Start()
    {
        OnLevelStartTrigger.OnLevelStartTriggered += HandleParticleSelection;
    }

    private void OnDisable()
    {
        OnLevelStartTrigger.OnLevelStartTriggered -= HandleParticleSelection;
    }

    private void HandleParticleSelection(Vector3 startLevelPosition)
    {
        foreach (GameObject particle in particles)
        {
            particle.SetActive(false);
        }

        int randomIndex = Random.Range(0, particles.Length);
        particles[randomIndex].SetActive(true);
    }
}
