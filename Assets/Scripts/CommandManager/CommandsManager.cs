using System.Collections.Generic;
using UnityEngine;


public class CommandsManager : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private CommandLable commandLabel;
    [SerializeField] GameObject labelContainer;

    public void AddCommandToMain(MoveTask task)
    {
        InstantiateItemToContainer(GetSprite(task.Type));
    }
    Sprite GetSprite(CharacterMovement.MoveType type)
    {
        return sprites.Find(x => x.name == type.ToString());
        // switch (type)
        // {
        //     case CharacterMovement.MoveType.Forward:
        //         
        //         break;
        //     case CharacterMovement.MoveType.Jump:
        //         
        //         break;
        //     case CharacterMovement.MoveType.ClockWiseRotate:
        //         
        //         break;
        //     case CharacterMovement.MoveType.CounterClockWiseRotate:
        //         
        //         break;
        //     case CharacterMovement.MoveType.TurnOnLight:
        //         
        //         break;
        // }
    }
    void InstantiateItemToContainer(Sprite sprite)
    {
        var item = Instantiate(commandLabel,labelContainer.transform);
        item.SetSprite(sprite);
        item.transform.localScale = Vector3.one;
    }
}
