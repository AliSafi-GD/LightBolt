using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private CommandLable commandLabel;
    [SerializeField] GameObject labelContainer;
    
    public CharacterLogic _characterLogic;
    public List<GroundItem> GroundItems = new List<GroundItem>();
    private List<CommandLable> _lables = new List<CommandLable>();

    // managers
    private CommandsManager _commandsManager;
    public CharacterMovement _character;
    private void Awake()
    {
       // CommandsManager.OnAddCommand += AddCommand;
    }

    private void OnDestroy()
    {
      //  CommandsManager.OnAddCommand -= AddCommand;
    }

    private void Start()
    {
        Init();
    }
    void Init()
    {
        _commandsManager = new CommandsManager(this);
        _characterLogic = new CharacterLogic(_character, _character);
        _characterLogic.grounds = GroundItems;
    }

    public void RunCommands()
    {
        _commandsManager.RunCommands();
    }

    public void Reset()
    {
        _character.Reset();
        _commandsManager.Reset();
        for (int i = 0; i < _lables.Count; i++)
        {
            Destroy(_lables[i].gameObject);
        }
        _lables.Clear();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            RunCommands();
    }
    public void AddCommand(ICommand command)
    {
        var item = InstantiateItemToContainer(GetSprite(command.Type));
        _lables.Add(item);
        item.Init(this,command);
        _commandsManager.AddCommandToMain(command);
    }

    public void RemoveCommand(ICommand command)
    {
        var item = _lables.Find(x => x._command == command);
        _lables.Remove(item);
        Destroy(item.gameObject);
        _commandsManager.RemoveCommand(command);
    }
    CommandLable InstantiateItemToContainer(Sprite sprite)
    {
        var item = MonoHelper.Instantiate(commandLabel,labelContainer.transform);
        item.SetSprite(sprite);
        item.transform.localScale = Vector3.one;
        return item;
    }
    Sprite GetSprite(CommandType type)
    {
        return sprites.Find(x => x.name == type.ToString());
    }
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
        Debug.Log("start IE_EunCommands");
        foreach (var command in _commands)
        {
            _gameManager._characterLogic.FindForwardGround();
            bool isDone = false;
            command.Execute(new RequestData()
            {
                CharacterData = _gameManager._character,
                CharacterView = _gameManager._character
            }, (x) => isDone = true);

            yield return new WaitUntil(()=>isDone);

        }
    }
}
