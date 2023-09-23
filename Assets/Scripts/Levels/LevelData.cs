using UnityEngine;

[CreateAssetMenu(fileName = "Level",menuName = "Level Creator")]
public class LevelData : ScriptableObject
{
    public int Seasion;
    public int LevelNumber;
    [SerializeField] private string LevelName;
    private string prefabPath => $"Levels/Season_{Seasion}/{LevelName}";
    public LevelView GetLevelPrefabFromResource => Resources.Load<LevelView>(prefabPath);
    
}