using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "Levels Data", menuName = "Levels", order = 0)]
    public class LevelsData : ScriptableObject
    {
        [SerializeField] public List<GameManager.Season> Seasons;

        public LevelData GetLevel(int seasonNumber, int levelNumber) => 
            Seasons.Find(x => x.SeasonNumber == seasonNumber)
            .Levels.Find(y => y.LevelNumber == levelNumber);
    }
}