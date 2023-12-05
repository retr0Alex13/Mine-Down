using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class BlockType
{
    public GameObject blockPrefab;
    public float spawnChance;
}
// Increase rows and cubes only one time in two finished levels
public class BlockGenerating : MonoBehaviour
{
    [SerializeField] private List<BlockType> blockTypes;
    [SerializeField] private GameObject wallBlock;
    [SerializeField] private GameObject dirtBlock;

    [SerializeField] private Transform blockSpawnPosition;
    [SerializeField] private Transform blockParent;

    [SerializeField] private BoxCollider startLevelCollider;
    [SerializeField] private BoxCollider endLevelCollider;

    [Space(5), Header("Level settings")]
    [SerializeField] private float levelSpacing = 20f;
    [SerializeField] private float levelClearDelay = 2f;

    [SerializeField] private int cubesPerRow = 4;
    [SerializeField] private int rowsCount = 15;

    [SerializeField] private int addCubesPerLevel = 1;
    [SerializeField] private int addRowsPerLevel = 5;

    [SerializeField] private int maxCubesPerRow = 10;
    [SerializeField] private int maxRowsCountPerLevel = 20;


    private List<GameObject> previousLevelList = new List<GameObject>();
    private List<GameObject> nextLevelList = new List<GameObject>();

    private void Start()
    {
        Application.targetFrameRate = 60;

        OnLevelEndTrigger.OnLevelEndTriggered += ProcessClearLevel;
        OnLevelEndTrigger.OnLevelEndTriggered += ProcessBlockGenerating;
        GenerateBlocks();
    }

    private void OnDisable()
    {
        OnLevelEndTrigger.OnLevelEndTriggered -= ProcessClearLevel;
        OnLevelEndTrigger.OnLevelEndTriggered -= ProcessBlockGenerating;
    }

    private void ProcessBlockGenerating()
    {
        Vector3 middleRowPosition = GetLastRowMiddlePosition();
        SetBlockSpawnPosition(middleRowPosition);
        GenerateBlocks();
    }

    private void SetBlockSpawnPosition(Vector3 middleRowPosition)
    {
        blockSpawnPosition.position = new Vector3(blockSpawnPosition.position.x, middleRowPosition.y - levelSpacing, blockSpawnPosition.position.z);
    }

    #region GetRowPosition
    private Vector3 GetLastRowMiddlePosition()
    {
        int lastRow = rowsCount - 1;
        Vector3 lastRowMiddlePosition = GetMiddleRowPosition(lastRow);
        return lastRowMiddlePosition;
    }

    private Vector3 GetFirstRowMiddlePosition()
    {
        int firstRow = 0;
        Vector3 firstRowMiddlePosition = GetMiddleRowPosition(firstRow);
        return firstRowMiddlePosition;
    }

    private Vector3 GetMiddleRowPosition(int row)
    {
        float middleXPosition =  (cubesPerRow - 1) * 0.5f * 1f;
        Vector3 middleRowPosition = blockSpawnPosition.position + new Vector3(middleXPosition, -row * 1f, 0f);
        return middleRowPosition;
    }
    #endregion

    private void GenerateBlocks()
    {
        for (int row = 0; row < rowsCount; row++)
        {
            for (int cubeIndex = 0; cubeIndex < cubesPerRow; cubeIndex++)
            {
                if (row == 0)
                {
                    if (IsWallSpawnPoint(cubeIndex))
                    {
                        SpawnWall(cubeIndex, row);
                    }
                    continue;
                }

                Vector3 spawnPosition = blockSpawnPosition.position + new Vector3(cubeIndex * 1f, -row * 1f);

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
        SetCubesAndRows();
        SetNextLevelColliders();
    }

    private bool IsWallSpawnPoint(int cubeIndex)
    {
        return cubeIndex == 0 || cubeIndex == cubesPerRow - 1;
    }

    private void SpawnWall(int cubeIndex, int row)
    {
        Vector3 specialBlockSpawnPos = blockSpawnPosition.position + new Vector3((cubeIndex == 0) ? -1f : cubesPerRow * 1f, -row * 1f, 0f);
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

    private void SetNextLevelColliders()
    {
        float colliderWidth = cubesPerRow * 1f;
        float colliderOffset = 1f;

        SetFirstTriggerCollider(colliderWidth, colliderOffset);
        SetLastTriggerCollider(colliderWidth, colliderOffset);
    }

    private void SetCubesAndRows()
    {
        cubesPerRow += addCubesPerLevel;
        rowsCount += addRowsPerLevel;

        if (cubesPerRow >= maxCubesPerRow)
        {
            cubesPerRow = maxCubesPerRow;
        }
        if (rowsCount > maxRowsCountPerLevel)
        {
            rowsCount = maxRowsCountPerLevel;
        }
    }

    #region SetTriggerPosition
    private void SetFirstTriggerCollider(float colliderWidth, float colliderOffset)
    {
        Vector3 lastTriggerColliderPosition = new Vector3(GetLastRowMiddlePosition().x, GetLastRowMiddlePosition().y - colliderOffset, 0f);
        endLevelCollider.transform.position = lastTriggerColliderPosition;
        endLevelCollider.size = new Vector2(colliderWidth, 0.1f);
        endLevelCollider.gameObject.SetActive(true);
    }
    private void SetLastTriggerCollider(float colliderWidth, float colliderOffset)
    {
        Vector3 firstTriggerColliderPosition = new Vector3(GetFirstRowMiddlePosition().x, GetFirstRowMiddlePosition().y + colliderOffset, 0f);
        startLevelCollider.transform.position = firstTriggerColliderPosition;
        startLevelCollider.size = new Vector2(colliderWidth, 0.1f);
        startLevelCollider.gameObject.SetActive(true);
    }
    #endregion

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
