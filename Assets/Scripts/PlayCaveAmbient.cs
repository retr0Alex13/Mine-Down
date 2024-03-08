using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCaveAmbient : MonoBehaviour
{
    private void PlayCaveAmbientSound()
    {
        SoundManager.instance.Play("CaveAmb", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayCaveAmbientSound();
            Destroy(this);
        }
    }
}
