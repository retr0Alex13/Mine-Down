using System.Collections;
using UnityEngine;

public class CollapseController : MonoBehaviour
{
    [SerializeField] private float spawnCollapseDelay = 2f;
    [SerializeField] private float collapseOffsetSpawn = 10f;

    private Vector3 spawnPosition;

    private void Start()
    {
        SoundManager.instance.Play("Collapse", transform);
    }

    private void OnEnable()
    {
        OnLevelStartTrigger.OnLevelStartTriggered += HandleCollapsePosition;
    }

    private void OnDisable()
    {
        OnLevelStartTrigger.OnLevelStartTriggered -= HandleCollapsePosition;
    }

    private void HandleCollapsePosition(Vector3 startLevelPosition)
    {
        spawnPosition = startLevelPosition;
        StartCoroutine(EnableCollapse());
    }

    private IEnumerator EnableCollapse()
    {
        yield return new WaitForSeconds(spawnCollapseDelay);
        transform.position = new Vector3(spawnPosition.x, spawnPosition.y + collapseOffsetSpawn);
    }
}
