using System;
using System.Collections;
using System.Collections.Generic;
using Public;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    // UI
    [SerializeField] private GameObject MainRoot;
    [SerializeField] private GameObject SeasonRoot;
    [SerializeField] private GameObject LevelsRoot;
    [SerializeField] private GameManager _gameManager;
    
    public Transform seasonContainer;
    public Transform levelsContainer;

    private List<Levels.Season> Seasons => ResourceManager.Instance.Levels.Seasons;

    [SerializeField] private List<Btn> levelsBtn;
    private void Start()
    {
        InitSeasons();
    }

    void InitSeasons()
    {
        foreach (var season in Seasons)
        {
            var btn = Instantiate(Resources.Load<Btn>($"UI/MainMenu/BtnSeason"),seasonContainer);
            btn.SetData($"Season {season.SeasonNumber}",()=>InitLevels(season.SeasonNumber));
        }
    }

    void InitLevels(int seasonNumber)
    {
        LevelsRoot.gameObject.SetActive(true);
        foreach (var level in Seasons.Find(x=>x.SeasonNumber == seasonNumber).Levels)
        {
            var btn = Instantiate(Resources.Load<Btn>($"UI/MainMenu/BtnLevel"),levelsContainer);
            btn.SetData($"{level.LevelNumber}",()=>LoadLevel(level));
            levelsBtn.Add(btn);
        }
    }

    public void ClearLevelsContainer()
    {
        foreach (var btn in levelsBtn)
        {
            Destroy(btn.gameObject);
        }
        levelsBtn.Clear();
    }
    void LoadLevel(LevelData levelData)
    {
        MainRoot.SetActive(false);
        _gameManager.LoadLevel(levelData);
    }
}
