using System;
using System.Collections;

public interface ICommand
{
    CharacterMovement.MoveType MoveType { get; }
    void Execute(RequestData requestData, Action completed);
}

public class RequestData
{
    public ICharacterView view;
}
