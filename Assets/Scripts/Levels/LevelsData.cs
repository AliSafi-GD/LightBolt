using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Levels
{
    [System.Serializable]
    public struct Season
    {
        public int SeasonNumber;
        public List<LevelData> Levels;
    }
    [CreateAssetMenu(fileName = "Levels Data", menuName = "Levels", order = 0)]
    public class LevelsData : ScriptableObject
    {
        [SerializeField] public List<Season> Seasons;

        public Season GetSeason(int number) => Seasons.FirstOrDefault(x => x.SeasonNumber == number);
        public List<LevelData> GetSeasonLevels(int seasonNumber) => Seasons.FirstOrDefault(x => x.SeasonNumber == seasonNumber).Levels;
        [CanBeNull] public LevelData GetLevel(int seasonNumber, int levelNumber) => 
            Seasons?.FirstOrDefault(x => x.SeasonNumber == seasonNumber)
            .Levels?.FirstOrDefault(y => y.LevelNumber == levelNumber);
    }
}