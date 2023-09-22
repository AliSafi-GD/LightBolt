using System;
using UnityEngine;

public class CommandForward : ICommand
{
    public CharacterMovement.MoveType MoveType => CharacterMovement.MoveType.Forward;

    public void Execute(RequestData requestData, Action completed)
    {
        requestData.view.viewTransform.position +=  new Vector3(1,0,0);
        completed?.Invoke();
    }
}
