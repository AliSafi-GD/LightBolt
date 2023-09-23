using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forward : ICommand
{
    public CommandType Type => CommandType.Forward;
    public CommandStatus Status { get; set; }

    public void Execute(RequestData requestData, Action<CommandStatus> complete)
    {
        if (requestData.CharacterData.forwardGround == null)
        {
            complete?.Invoke(CommandStatus.Reject);
            return;
        }
        if (requestData.CharacterData.Height == requestData.CharacterData.forwardGround.Height)
        {
            var NextMove = requestData.CharacterData.NextMove;
            var character = requestData.CharacterView.Transform;
            NextMove += character.forward;
            requestData.CharacterData.NextMove = NextMove;
            requestData.CharacterData.currentGround = requestData.CharacterData.forwardGround;
            MonoHelper.Move(character,NextMove,()=>complete?.Invoke(CommandStatus.Accept));
        }
        else
        {
            complete?.Invoke(CommandStatus.Reject);
        }
    }
}
public class RotateClockwise : ICommand
{
    public CommandType Type => CommandType.ClockWiseRotate;
    public CommandStatus Status { get; set; }

    public void Execute(RequestData requestData, Action<CommandStatus> complete)
    {
        var NextRotate = requestData.CharacterData.NextRotate;
        var character = requestData.CharacterView.Transform;
        NextRotate = Quaternion.AngleAxis(NextRotate.eulerAngles.y + 90,Vector3.up);
        requestData.CharacterData.NextRotate = NextRotate;
        MonoHelper.Rotate(character,NextRotate,()=>complete?.Invoke(CommandStatus.Accept));
    }
}
public class RotateCounterClockwise : ICommand
{
    public CommandType Type => CommandType.ClockWiseRotate;
    public CommandStatus Status { get; set; }

    public void Execute(RequestData requestData, Action<CommandStatus> complete)
    {
        var NextRotate = requestData.CharacterData.NextRotate;
        var character = requestData.CharacterView.Transform;
        NextRotate = Quaternion.AngleAxis(NextRotate.eulerAngles.y - 90,Vector3.up);
        requestData.CharacterData.NextRotate = NextRotate;
        MonoHelper.Rotate(character,NextRotate,()=>complete?.Invoke(CommandStatus.Accept));
    }
}
public class Jump : ICommand
{
    public CommandType Type => CommandType.Jump;
    public CommandStatus Status { get; set; }

    public void Execute(RequestData requestData, Action<CommandStatus> complete)
    {

        if (requestData.CharacterData.Height + 0.5f == requestData.CharacterData.forwardGround.Height ||
            requestData.CharacterData.Height - 0.5f == requestData.CharacterData.forwardGround.Height)
        {
            requestData.CharacterData.NextMove += requestData.CharacterView.Transform.forward;
            requestData.CharacterData.NextMove = new Vector3(requestData.CharacterData.NextMove.x,
                requestData.CharacterData.forwardGround.Height, requestData.CharacterData.NextMove.z);
            requestData.CharacterData.Height = requestData.CharacterData.forwardGround.Height;
            requestData.CharacterData.currentGround = requestData.CharacterData.forwardGround;
            MonoHelper.Move(requestData.CharacterView.Transform, requestData.CharacterData.NextMove,
                () => complete?.Invoke(CommandStatus.Accept));
        }
        else
        {
            complete?.Invoke(CommandStatus.Reject);
        }



    }
}

public class TurnOnLight : ICommand
{
    public CommandType Type => CommandType.TurnOnLight;
    public CommandStatus Status { get; set; }
    public void Execute(RequestData requestData, Action<CommandStatus> complete)
    {
        if (requestData.CharacterData.currentGround.GetComponent<GroundLight>())
        {
            requestData.CharacterData.currentGround.GetComponent<GroundLight>().TurnOnLight();
            complete?.Invoke(CommandStatus.Accept);
        }
        else
        {
            complete?.Invoke(CommandStatus.Reject);
        }
    }
}
public enum CommandType
{
    Forward,
    ClockWiseRotate,
    CounterClockWiseRotate,
    Jump,
    TurnOnLight
}
public enum CommandStatus
{
    None,
    Accept,
    Reject
}
public interface ICommand
{
    CommandType Type{get;}
    CommandStatus Status { get; set; }
    void Execute(RequestData requestData,Action<CommandStatus> complete);
}

public class RequestData
{
    public ICharacterView CharacterView;
    public ICharacterData CharacterData;
}