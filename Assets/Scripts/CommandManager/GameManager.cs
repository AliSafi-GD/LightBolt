using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ToDo : درست کردن ازتفاع کاراکتر موقع ست شدن روی استارت گراند
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

    [SerializeField] private List<Season> Seasons;
    [SerializeField] private LevelData currentLevelData;
    [SerializeField] private LevelView CurrentLevelView;

    
    // Commands UI
    [SerializeField] GameObject labelContainer;
    [SerializeField] private GameObject CommandsUsableContainer;
    [SerializeField] private List<CommandItemUI> CommandUsable = new List<CommandItemUI>();
    [SerializeField] private List<CommandItemUI> CommandsStack = new List<CommandItemUI>();
    
    
    // Game UI
    public GameObject WinBtn;
    
    public CharacterLogic _characterLogic;
    public List<GroundItem> CurrentLevelGroundsItems => CurrentLevelView.Grounds;

    // managers
    private CommandsManager _commandsManager;
    public CharacterMovement _character;

    private void Awake()
    {
        OnTurnOnLight += _OnTurnOnLight;
        CreateLevel(1, 2);
        CreateUsableUICommands();
    }

    private void _OnTurnOnLight()
    {
        CurrentLevelView.CurrentLightTurnedOn += 1;
        var isWin = CheckWin(CurrentLevelView);
        if(isWin)
            WinBtn.SetActive(true);
    }

    void CreateLevel(int seasonNumber,int levelNumber)
    {
        var levels = Seasons.Find(x => x.SeasonNumber == seasonNumber).Levels;
        currentLevelData = levels.Find(x => x.LevelNumber == levelNumber);
        CurrentLevelView = Instantiate(currentLevelData.GetLevelPrefabFromResource);
        CurrentLevelView.transform.position = Vector3.zero;
    }

    public void NextLevel()
    {
        Destroy(CurrentLevelView.gameObject);
        CreateLevel(1,currentLevelData.LevelNumber+1);
       
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
        
        _commandsManager.Reset();
        
        
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

    private void Start()
    {
        Init();
    }
    void Init()
    {
        _commandsManager = new CommandsManager(this);
        _characterLogic = new CharacterLogic(_character, _character);
        _characterLogic.grounds = CurrentLevelGroundsItems;
        _character.StartGround = CurrentLevelView.StartGround;
        _character.startRot = CurrentLevelView.StartAngle;
    }

    public void RunCommands()
    {
        _commandsManager.RunCommands();
    }

    public void Reset()
    {
        _character.Reset();
        //_commandsManager.Reset();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            RunCommands();
    }
    public void AddCommand(ICommand command)
    {
        var item = CreateCommandUI(command.Type, labelContainer.transform);
        CommandsStack.Add(item);
        item.SetData(command, () =>
        {
            RemoveCommand(command);
        });
        _commandsManager.AddCommandToMain(command);
    }

    public void RemoveCommand(ICommand command)
    {
        var item = CommandsStack.Find(x => x.Command == command);
        CommandsStack.Remove(item);
        Destroy(item.gameObject);
        _commandsManager.RemoveCommand(command);
    }

    bool CheckWin(ILevelConfig data) => data.CurrentLightTurnedOn == data.LightCount;
}
public class CommandsManager
{
    public CommandsManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public static event Action<ICommand> OnAddCommand;
    public static event Action<ICommand> OnRemoveCommand;
    
    
    private List<ICommand> _commands = new List<ICommand>();

    private GameManager _gameManager;

    public void Reset()
    {
        _commands.Clear();
    }
    public void AddCommandToMain(ICommand command)
    {
        _commands.Add(command);
        OnAddCommand?.Invoke(command);
    }

    public void RemoveCommand(ICommand command)
    {
        _commands.Remove(command);
        OnRemoveCommand?.Invoke(command);
    }
    public void RunCommands()
    {
        MonoHelper.RunCoroutine(IE_RunCommands());
    }

    IEnumerator IE_RunCommands()
    {
       
        foreach (var command in _commands)
        {
            Debug.Log($"start {command.Type}");
            _gameManager._characterLogic.FindForwardGround();
            bool isDone = false;
            command.Execute(new RequestData()
            {
                CharacterData = _gameManager._character,
                CharacterView = _gameManager._character
            }, (x) => isDone = true);

            yield return new WaitUntil(()=>isDone);
            Debug.Log($"end {command.Type}");
        }
    }
}
