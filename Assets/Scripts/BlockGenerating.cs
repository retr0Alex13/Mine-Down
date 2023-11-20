using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockType
{
    public GameObject blockPrefab;
    public float spawnChance;
}

public class BlockGenerating : MonoBehaviour
{
    [SerializeField]
    private List<BlockType> blockTypes;
    [SerializeField]
    private GameObject wallBlock;
    [SerializeField]
    private GameObject dirtBlock;
    [SerializeField]
    private Transform blockSpawnPosition;
    [SerializeField]
    private Transform blockParent;
    [SerializeField]
    private BoxCollider nextLevelCollider;
    [SerializeField]
    private float levelClearDelay = 2f;

    [SerializeField]
    private int cubesPerRow = 4;
    [SerializeField]
    private int rowsCount = 10;

    private float blockSpacing = 1f;

    private List<GameObject> previousLevelList = new List<GameObject>();
    private List<GameObject> nextLevelList = new List<GameObject>();


    private void Start()
    {
        Application.targetFrameRate = 60;

        NextLevelTrigger.OnNextLevelTriggered += ProcessClearLevel;
        NextLevelTrigger.OnNextLevelTriggered += ProcessBlockGenerating;
        GenerateBlocks();
    }

    private void OnDisable()
    {
        NextLevelTrigger.OnNextLevelTriggered -= ProcessClearLevel;
        NextLevelTrigger.OnNextLevelTriggered -= ProcessBlockGenerating;
    }

    private void ProcessBlockGenerating()
    {
        Vector3 middleRowPosition = GetLastRowMiddlePosition();
        blockSpawnPosition.position = new Vector3(blockSpawnPosition.position.x, middleRowPosition.y - 20f, blockSpawnPosition.position.z);
        GenerateBlocks();
    }

    private Vector3 GetLastRowMiddlePosition()
    {
        int lastRow = rowsCount - 1;
        float middleXPosition = (cubesPerRow - 1) * 0.5f * blockSpacing;
        Vector3 middleRowPosition = blockSpawnPosition.position + new Vector3(middleXPosition, -lastRow * blockSpacing, 0f);
        return middleRowPosition;
    }

    private void GenerateBlocks()
    {
        for (int row = 0; row < rowsCount; row++)
        {
            for (int cubeIndex = 0; cubeIndex < cubesPerRow; cubeIndex++)
            {
                Vector3 spawnPosition = blockSpawnPosition.position + new Vector3(cubeIndex * blockSpacing, -row * blockSpacing);

                float randomValue = Random.value;
                GameObject blockPrefab = ChooseBlockPrefab(randomValue);

                if (blockPrefab != null)
                {
                    nextLevelList.Add(Instantiate(blockPrefab, spawnPosition, Quaternion.identity, blockParent));
                }

                if (IsWallSpawnPoint(cubeIndex))
                {
                    SpawnWall(cubeIndex, row);
                }
            }
        }
        SetNextLevelCollider();
    }

    private bool IsWallSpawnPoint(int cubeIndex)
    {
        return cubeIndex == 0 || cubeIndex == cubesPerRow - 1;
    }

    private void SpawnWall(int cubeIndex, int row)
    {
        Vector3 specialBlockSpawnPos = blockSpawnPosition.position + new Vector3((cubeIndex == 0) ? -blockSpacing : cubesPerRow * blockSpacing, -row * blockSpacing, 0f);
        nextLevelList.Add(Instantiate(wallBlock, specialBlockSpawnPos, Quaternion.identity, blockParent));
    }

    private GameObject ChooseBlockPrefab(float randomValue)
    {
        float accumulatedChance = 0f;

        for (int i = 0; i < blockTypes.Count; i++)
        {
            accumulatedChance += blockTypes[i].spawnChance;
            if (randomValue < accumulatedChance)
            {
                return blockTypes[i].blockPrefab;
            }
        }
        // Return default dirt block
        return dirtBlock;
    }

    private void SetNextLevelCollider()
    {
        float lastRowYPos = -rowsCount * 1f;
        float colliderWidth = cubesPerRow * 1f;

        float xOffset = (colliderWidth - 1f) / 2f;

        Vector3 triggerColliderPosition = blockSpawnPosition.position + new Vector3(xOffset, lastRowYPos, 0f);
        nextLevelCollider.transform.position = triggerColliderPosition;
        nextLevelCollider.size = new Vector2(colliderWidth, 0.1f);
        nextLevelCollider.gameObject.SetActive(true);
    }

    private void ProcessClearLevel()
    {
        StartCoroutine(ClearLevelWithDelay(levelClearDelay));
    }

    // Don't touch
    private IEnumerator ClearLevelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (GameObject cube in previousLevelList)
        {
            if (cube != null)
            {
                Destroy(cube);
            }
        }
        previousLevelList.Clear();
        foreach (GameObject cube in nextLevelList)
        {
            if (cube != null)
            {
                previousLevelList.Add(Instantiate(cube, blockParent));
                Destroy(cube);
            }
        }
        nextLevelList.Clear();
    }
}
