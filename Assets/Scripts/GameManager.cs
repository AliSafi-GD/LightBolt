using UnityEngine;

public class GameManager : MonoBehaviour
{
    public CharacterMovement characterMovement;
    private CommandsManager _commandsManager;

    private ICharacterLogicController _characterLogicController;

    private void Start()
    {
        GameInitialize();
    }

    private void GameInitialize()
    {
        _characterLogicController = new CharacterLogicController(characterMovement);
        _commandsManager.Setup(_characterLogicController);
    }
}
