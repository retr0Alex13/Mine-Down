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
    private int cubesPerRow = 4;
    [SerializeField]
    private int rowsCount = 10;

    private void Start()
    {
        Application.targetFrameRate = 60;
        Random.InitState(System.DateTime.Now.Millisecond);
        BlockCreating();
    }

    private void BlockCreating()
    {
        for (int row = 0; row < rowsCount; row++)
        {
            for (int cubeIndex = 0; cubeIndex < cubesPerRow; cubeIndex++)
            {
                // Використовуйте blockSpawnPosition як базову позицію
                Vector3 spawnPosition = blockSpawnPosition.position + new Vector3(cubeIndex * 1f, -row * 1f, 0f);

                float randomValue = Random.value;
                GameObject blockPrefab = ChooseBlockPrefab(randomValue);
                if (blockPrefab != null)
                {
                    Instantiate(blockPrefab, spawnPosition, Quaternion.identity, blockParent);
                }

                if (cubeIndex == 0 || cubeIndex == cubesPerRow - 1)
                {
                    // Wall block spawning
                    Vector3 specialBlockSpawnPos = blockSpawnPosition.position + new Vector3((cubeIndex == 0) ? -1f : cubesPerRow * 1f, -row * 1f, 0f);
                    Instantiate(wallBlock, specialBlockSpawnPos, Quaternion.identity, blockParent);
                }
            }
        }
    }

    private GameObject ChooseBlockPrefab(float randomValue)
    {
        float accumulatedChance = 0f;

        foreach (BlockType blockType in blockTypes)
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
}
