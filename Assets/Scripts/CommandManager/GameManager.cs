using System;
using System.Collections;
using System.Collections.Generic;
using Public;
using UnityEngine;

interface IInitializable
{
    void Init();
}
public class GameManager : MonoBehaviour , IInitializable
{
    // ToDo : موقع بردن اگر مراحل یک فصل تمام شده بودن به صفحه انتخاب فصل ها برود
    // event 
    public static Action OnTurnOnLight;
    // Game Resource
    [System.Serializable]
    public struct Season
    {
        public int SeasonNumber;
        public List<LevelData> Levels;
    }
    [SerializeField] private LevelData currentLevelData;
    [SerializeField] private LevelView CurrentLevelView;

    
    // Commands UI
    [SerializeField] GameObject activeContainer;
    [SerializeField] GameObject labelContainer;
    [SerializeField] GameObject program1Container;
    [SerializeField] private GameObject CommandsUsableContainer;
    [SerializeField] private List<CommandItemUI> CommandUsable = new List<CommandItemUI>();
    [SerializeField] private List<CommandItemUI> CommandsStack = new List<CommandItemUI>();
    
    
    // Game UI
    public GameObject WinBtn;
    
    public CharacterLogic _characterLogic;
    public List<GroundItem> CurrentLevelGroundsItems => CurrentLevelView.Grounds;

    // managers
    public static List<CommandsManager> CommandsManager;
    private CommandsManager currentCommandsManager;
    public CharacterMovement _character;


    private List<IInitializable> Initializables = new List<IInitializable>();

    private void Start()
    {
        Initializables.Add(this);
        Initializables.Add(_character);
    }

    public void LoadLevel(LevelData levelData)
    {
        OnTurnOnLight += _OnTurnOnLight;
        CreateLevel(levelData);
        CreateUsableUICommands();
        CommandsManager = levelData.GetLevelPrefabFromResource.CommandsManagers;
        foreach (var initializable in Initializables)
        {
            initializable.Init();
        }
    }

    private void _OnTurnOnLight()
    {
        CurrentLevelView.CurrentLightTurnedOn += 1;
    }

    void CreateLevel(LevelData levelData)
    {
        //var levels = Seasons.Find(x => x.SeasonNumber == seasonNumber).Levels;
        currentLevelData = levelData;
        CurrentLevelView = Instantiate(currentLevelData.GetLevelPrefabFromResource);
        CurrentLevelView.transform.position = Vector3.zero;
    }

    public void NextLevel()
    {
        Destroy(CurrentLevelView.gameObject);
        CreateLevel(ResourceManager.Instance.Levels.GetLevel(currentLevelData.Seasion,currentLevelData.LevelNumber+1));
       
        _characterLogic.grounds = CurrentLevelGroundsItems;
        _character.StartGround = CurrentLevelView.StartGround;
        _character.startRot = CurrentLevelView.StartAngle;
        Reset();
        
        foreach (var commandItemUI in CommandsStack)
        {
            Destroy(commandItemUI.gameObject);
        }
        CommandsStack.Clear();
        
        foreach (var commandItemUI in CommandUsable)
        {
            Destroy(commandItemUI.gameObject);
        }
        CommandUsable.Clear();
        
        currentCommandsManager.Reset();
        
        
        CreateUsableUICommands();
        WinBtn.SetActive(false);
    }
    CommandItemUI CreateCommandUI(CommandType command,Transform parent)
    {
        return Instantiate(Resources.Load<CommandItemUI>($"UI/Commands/Command item _ {command.ToString()}"),parent);
    }
    void CreateUsableUICommands()
    {
        foreach (var command in CurrentLevelView.CommandsRequired)
        {
            var c = CreateCommandUI(command,CommandsUsableContainer.transform);
            CommandUsable.Add(c);
            var Icommand = FactoryMapper.GetCommand(command);
            c.SetData(Icommand,()=>
            {
                AddCommand(Icommand);
            });
        }
    }
    public void Init()
    {
        ChangeActiveContainer(labelContainer);
        currentCommandsManager = CommandsManager[0];
        _characterLogic = new CharacterLogic(_character, _character);
        _characterLogic.grounds = CurrentLevelGroundsItems;
        _character.StartGround = CurrentLevelView.StartGround;
        _character.startRot = CurrentLevelView.StartAngle;
    }

    public void RunCommands()
    {
        CommandsManager[0].RunCommands(new RequestData()
        {
            CharacterData = _character,
            CharacterView = _character
        }, () =>
        {
            var isWin = CheckWin(CurrentLevelView);
            if(isWin)
                WinBtn.SetActive(true);
        });
    }

    public void Reset()
    {
        _character.Reset();
        //_commandsManager.Reset();
    }

    private void Update()
    {
        _characterLogic.FindForwardGround();
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            RunCommands();
    }

    public void ChangeActiveContainer(GameObject container) => activeContainer = container;
    public void ChangeActiveCommandManager(int number) => currentCommandsManager = CommandsManager[number];
    
    public void AddCommand(ICommand command)
    {
        var item = CreateCommandUI(command.Type, activeContainer.transform);
        CommandsStack.Add(item);
        item.SetData(command, () =>
        {
            RemoveCommand(command);
        });
       
        currentCommandsManager.AddCommandToMain(command);
    }

    public void RemoveCommand(ICommand command)
    {
        var item = CommandsStack.Find(x => x.Command == command);
        CommandsStack.Remove(item);
        Destroy(item.gameObject);
        currentCommandsManager.RemoveCommand(command);
    }

    bool CheckWin(ILevelConfig data) => data.CurrentLightTurnedOn == data.LightCount;
    
}
[Serializable]
public class CommandsManager
{
    
    public string name;
    
    public static event Action<ICommand> OnAddCommand;
    public static event Action<ICommand> OnRemoveCommand;
    
    
    public List<ICommand> _commands = new List<ICommand>();

    //private GameManager _gameManager;

    public void Reset()
    {
        _commands.Clear();
    }
    public void AddCommandToMain(ICommand command)
    {
        Debug.Log($"add command to {name}");
        _commands.Add(command);
        OnAddCommand?.Invoke(command);
    }

    public void RemoveCommand(ICommand command)
    {
        Debug.Log($"add command to {name}");
        _commands.Remove(command);
        OnRemoveCommand?.Invoke(command);
    }
    public void RunCommands(RequestData requestData,Action OnFinish)
    {
        Debug.Log($"Run Commands {name}");
        MonoHelper.RunCoroutine(IE_RunCommands(requestData,OnFinish));
    }

    IEnumerator IE_RunCommands(RequestData requestData,Action OnFinish)
    {
        Debug.Log($"commands count : {_commands.Count} in {name}");
        foreach (var command in _commands)
        {
            Debug.Log($"start {command.Type}");
            bool isDone = false;
            command.Execute(requestData, (x) => isDone = true);
            yield return new WaitUntil(()=>isDone);
            Debug.Log($"end {command.Type}");
        }
        OnFinish?.Invoke();
    }
}
