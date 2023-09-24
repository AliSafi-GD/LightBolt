using System;
using Commands;
using Ground;
using Manager;
using Public;
using UI.GameScene;
using UnityEngine;
public class GameManager : MonoBehaviour
{

    #region Events
    public static event Action OnResetLevel;
    public static Action OnTurnOnLight;
    #endregion
   


    private LevelData currentLevelData;
    private LevelView CurrentLevelView;
    private GroundsHelper groundsHelper;
    
    // Commands UI
    [SerializeField] private GameObject programCommandsContainer;
    [SerializeField] private GameObject mainCommandsContainer;
    
    
    // Game UI
    [SerializeField] GameObject WinBtn;

    // managers
    private RequiredCommandsUICreator _requiredCommandsUICreator;
    public ProgramControllerHub programControllerHub;
    [SerializeField] private Character _character;

    private void Start()
    {
        OnTurnOnLight += _OnTurnOnLight;
        _requiredCommandsUICreator = new RequiredCommandsUICreator();
    }

    private void _OnTurnOnLight()
    {
        CurrentLevelView.CurrentLightTurnedOn += 1;
    }
    public void LoadLevel(LevelData levelData)
    {
        CreateLevelPrefab(levelData);
        var levelDataCommandManager = levelData.GetLevelPrefabFromResource.CommandsManagers;
        programControllerHub = new ProgramControllerHub(programCommandsContainer.transform,levelDataCommandManager);
        programControllerHub.ChangeActiveProgram("Main");
        groundsHelper = new GroundsHelper(CurrentLevelView.Grounds);
        _character.Init(CurrentLevelView.StartGround,CurrentLevelView.StartAngle,CurrentLevelView.StartGround.Height);
        _requiredCommandsUICreator.Create(CurrentLevelView.CommandsRequired,mainCommandsContainer.transform, programControllerHub.AddCommand);
    }

    void UnloadLevel()
    {
        Destroy(CurrentLevelView.gameObject);
        programControllerHub.Destroy();
        _requiredCommandsUICreator.Destroy();
    }

    void CreateLevelPrefab(LevelData levelData)
    {
        currentLevelData = levelData;
        CurrentLevelView = Instantiate(currentLevelData.GetLevelPrefabFromResource);
        CurrentLevelView.transform.position = Vector3.zero;
    }

    public void NextLevel()
    {
        var nextLevelData = GetNextLevel();
        if(nextLevelData == null)
            return;
        UnloadLevel();
        LoadLevel(nextLevelData);
        WinBtn.SetActive(false);
    }

    private LevelData GetNextLevel()
    {
        int seasonNumber = currentLevelData.Seasion;
        int levelNumber = currentLevelData.LevelNumber + 1;
        if (levelNumber > ResourceManager.Instance.Levels.GetSeasonLevels(seasonNumber).Count)
        {
            seasonNumber++;
            levelNumber = 1;
        }

        var nextLevelData = ResourceManager.Instance.Levels.GetLevel(seasonNumber, levelNumber);
        return nextLevelData;
    }

    public void RunCommands()
    {
        programControllerHub.activeProgramController.logic.RunCommands(new RequestData()
        {
            Manager =  this,
            CharacterData = _character,
            CharacterView = _character,
            GroundsHelper = groundsHelper
        }, () =>
        {
            var isWin = CheckWin(CurrentLevelView);
            if(isWin)
                WinBtn.SetActive(true);
        });
    }

    public void Reset()
    {
        OnResetLevel?.Invoke();
    }

    bool CheckWin(ILevelConfig data) => data.CurrentLightTurnedOn == data.LightCount;
}

