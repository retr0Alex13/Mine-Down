using UnityEngine;

[System.Serializable]
public class LevelSettings
{
    [field: SerializeField] public float LevelSpacing { get; private set; }
    [field: SerializeField] public float LevelClearDelay { get; private set; }
    [field: SerializeField] public int CubesPerRow { get; private set; }
    [field: SerializeField] public int CubesPerLevel { get; private set; }
    [field: SerializeField] public int RowsPerLevel { get; private set; }
    [field: SerializeField] public int MaxCubesOnRow { get; private set; }

    
    public void AddCubesPerRow(int cubesPerRow)
    {
        CubesPerRow += cubesPerRow;
    }

    public void SetCubesPerRow(int cubesPerRow)
    {
        CubesPerRow = cubesPerRow;
    }
}
