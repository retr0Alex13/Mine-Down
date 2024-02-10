using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallaxEffect;
    private float lentgh;
    private float startPosition;

    void Start()
    {
        startPosition = transform.position.y;
        lentgh = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        float temp = (cam.transform.position.y * (1 - parallaxEffect));
        float distance = cam.transform.position.y * parallaxEffect;

        transform.position = new Vector3(transform.position.x, startPosition + distance, transform.position.z);

        if (temp > startPosition + lentgh)
        {
            startPosition += lentgh;
        }
        else if (temp < startPosition - lentgh)
        {
            startPosition -= lentgh;
        }
    }
}
