using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class BlockType
{
    public GameObject[] blockPrefabs;
    public float spawnChance;
}

public class BlockGenerator : MonoBehaviour
{

    public LevelSettings levelSettings;

    [SerializeField] private List<BlockType> blockTypes;

    [SerializeField] private GameObject wallBlock;
    [SerializeField] private GameObject dirtBlock;

    [SerializeField] private Transform blockSpawnPosition;
    [SerializeField] private Transform blockParent;

    [SerializeField] private BoxCollider startLevelCollider;
    [SerializeField] private BoxCollider endLevelCollider;

    private int rowCount = 15;

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
        Vector3 middleRowPosition = GetRowMiddlePosition(rowCount - 1);
        SetBlockSpawnPosition(middleRowPosition);
        GenerateBlocks();
    }

    private void SetBlockSpawnPosition(Vector3 middleRowPosition)
    {
        blockSpawnPosition.position = new Vector3(blockSpawnPosition.position.x, middleRowPosition.y - levelSettings.LevelSpacing, blockSpawnPosition.position.z);
    }

    private Vector3 GetRowMiddlePosition(int row)
    {
        float middleXPosition = (levelSettings.CubesPerRow - 1) * 0.5f * 1f;
        return blockSpawnPosition.position + new Vector3(middleXPosition, -row * 1f, 0f);
    }

    private void GenerateBlocks()
    {
        dfs = new DepthFirstSearch();

        // Perform DFS to get a path from top to bottom
        List<Vector2Int> path = dfs.FindPath(rowCount, levelSettings.CubesPerRow);

        for (int row = 0; row < rowCount; row++)
        {
            for (int cubeIndex = 0; cubeIndex < levelSettings.CubesPerRow; cubeIndex++)
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

                if (row <= 2 && blockPrefab.name == "SpikeBlock") // Prevent spike blocks in first and second row
                {
                    blockPrefab = dirtBlock;
                }

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
        return cubeIndex == 0 || cubeIndex == levelSettings.CubesPerRow - 1;
    }

    private void SpawnWall(int cubeIndex, int row)
    {
        Vector3 specialBlockSpawnPos = blockSpawnPosition.position + new Vector3((cubeIndex == 0) ? -1f : levelSettings.CubesPerRow * 1f, -row * 1f, 0f);
        nextLevelList.Add(Instantiate(wallBlock, specialBlockSpawnPos, Quaternion.identity, blockParent));
    }

    private GameObject ChooseBlockPrefab(float randomValue)
    {
        float totalSpawnChance = 0f;

        // Calculate total spawn chance for normalization
        foreach (var blockType in blockTypes)
        {
            totalSpawnChance += blockType.spawnChance;
        }

        float normalizedRandomValue = randomValue * totalSpawnChance;

        // Choose a block based on the weighted randomization
        foreach (var blockType in blockTypes)
        {
            if (normalizedRandomValue < blockType.spawnChance)
            {
                // Randomly select a block prefab from the chosen block type
                System.Random rand = new System.Random();
                int randomBlockIndex = rand.Next(0, blockType.blockPrefabs.Length);
                return blockType.blockPrefabs[randomBlockIndex];
            }

            normalizedRandomValue -= blockType.spawnChance;
        }

        // Return default dirt block
        return dirtBlock;
    }


    private void SetNextLevelColliders()
    {
        float colliderWidth = levelSettings.CubesPerRow * 1f;
        float colliderOffset = -1f;

        Vector3 lastBlockPosition = nextLevelList[nextLevelList.Count - 1].transform.position;

        SetTriggerCollider(endLevelCollider, lastBlockPosition, colliderOffset, colliderWidth);
        SetTriggerCollider(startLevelCollider, blockSpawnPosition.position, 0f, colliderWidth);
    }


    private void SetCubesAndRows()
    {
        rowCount += levelSettings.RowsPerLevel;
        levelSettings.AddCubesPerRow(levelSettings.CubesPerLevel);
        if (levelSettings.CubesPerRow > levelSettings.MaxCubesOnRow)
        {
            levelSettings.SetCubesPerRow(levelSettings.MaxCubesOnRow);
        }
    }

    private void SetTriggerCollider(BoxCollider collider, Vector3 position, float colliderYOffset, float colliderWidth)
    {
        float rowCenterX = blockSpawnPosition.position.x + (levelSettings.CubesPerRow - 1) * 0.5f * 1f;
        Vector3 triggerColliderPosition = new Vector3(rowCenterX, position.y + colliderYOffset, 0f);
        collider.transform.position = triggerColliderPosition;
        collider.size = new Vector2(colliderWidth, 0.1f);
        collider.gameObject.SetActive(true);
    }

    private void ProcessClearLevel()
    {
        StartCoroutine(ClearLevelWithDelay(levelSettings.LevelClearDelay));
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