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
    [SerializeField, Range(1, 20)] private int addRowsPerLevel = 10;
    [SerializeField] private int addCubesPerLevel = 1;
    [SerializeField, Range(1, 5)] private int maxCubesOnRow = 4;

    private List<GameObject> previousLevelList = new List<GameObject>();
    private List<GameObject> nextLevelList = new List<GameObject>();
    private DepthFirstSearch dfs;

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
        Vector3 middleRowPosition = GetRowMiddlePosition(rowsCount - 1);
        SetBlockSpawnPosition(middleRowPosition);
        GenerateBlocks();
    }

    private void SetBlockSpawnPosition(Vector3 middleRowPosition)
    {
        blockSpawnPosition.position = new Vector3(blockSpawnPosition.position.x, middleRowPosition.y - levelSpacing, blockSpawnPosition.position.z);
    }

    private Vector3 GetRowMiddlePosition(int row)
    {
        float middleXPosition = (cubesPerRow - 1) * 0.5f * 1f;
        return blockSpawnPosition.position + new Vector3(middleXPosition, -row * 1f, 0f);
    }

    private void GenerateBlocks()
    {
        dfs = new DepthFirstSearch();

        // Perform DFS to get a path from top to bottom
        List<Vector2Int> path = dfs.FindPath(rowsCount, cubesPerRow);

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

                // Set dirt block for the path created by DFS
                if (path.Contains(new Vector2Int(cubeIndex, row)))
                {
                    blockPrefab = dirtBlock;
                }

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

        foreach (var blockType in blockTypes)
        {
            accumulatedChance += blockType.spawnChance;
            if (randomValue < accumulatedChance)
            {
                return blockType.blockPrefab;
            }
        }

        // Return default dirt block
        return dirtBlock;
    }

    private void SetNextLevelColliders()
    {
        float colliderWidth = cubesPerRow * 1f;
        float colliderOffset = -1f;

        Vector3 lastBlockPosition = nextLevelList[nextLevelList.Count - 1].transform.position;

        SetTriggerCollider(endLevelCollider, lastBlockPosition, colliderOffset, colliderWidth);
        SetTriggerCollider(startLevelCollider, blockSpawnPosition.position, 0f, colliderWidth);
    }


    private void SetCubesAndRows()
    {
        rowsCount += addRowsPerLevel;
        cubesPerRow += addCubesPerLevel;
        if (cubesPerRow > maxCubesOnRow)
        {
            cubesPerRow = maxCubesOnRow;
        }
    }

    private void SetTriggerCollider(BoxCollider collider, Vector3 position, float colliderYOffset, float colliderWidth)
    {
        float rowCenterX = blockSpawnPosition.position.x + (cubesPerRow - 1) * 0.5f * 1f;
        Vector3 triggerColliderPosition = new Vector3(rowCenterX, position.y + colliderYOffset, 0f);
        collider.transform.position = triggerColliderPosition;
        collider.size = new Vector2(colliderWidth, 0.1f);
        collider.gameObject.SetActive(true);
    }

    private void ProcessClearLevel()
    {
        StartCoroutine(ClearLevelWithDelay(levelClearDelay));
    }

    private IEnumerator ClearLevelWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        ClearAndInstantiateLevel(previousLevelList);
        ClearAndInstantiateLevel(nextLevelList);
    }

    private void ClearAndInstantiateLevel(List<GameObject> levelList)
    {
        foreach (var cube in levelList)
        {
            if (cube != null)
            {
                Destroy(cube);
            }
        }

        levelList.Clear();

        foreach (var cube in nextLevelList)
        {
            if (cube != null)
            {
                levelList.Add(Instantiate(cube, blockParent));
                Destroy(cube);
            }
        }
    }
}