using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forward : ICommand
{
    public CommandType Type => CommandType.Forward;
    public CommandStatus Status { get; set; }
    public Action<CommandStatus> complete { get; set; }

    public void Execute(RequestData requestData, Action<CommandStatus> complete)
    {
        this.complete = complete;
        var forwardGround = requestData.GroundsHelper.FindForwardGround(requestData.CharacterData.StartGround,requestData.CharacterView.Transform);
        if (forwardGround == null)
        {
            complete?.Invoke(CommandStatus.Reject);
            return;
        }
        if (requestData.CharacterData.Height == forwardGround.Height)
        {
            var NextMove = requestData.CharacterView.Transform.position;
            var character = requestData.CharacterView.Transform;
            NextMove += character.forward;
            //requestData.CharacterData.NextMove = NextMove;
            requestData.CharacterData.CurrentGround = forwardGround;
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
    public Action<CommandStatus> complete { get; set; }

    public void Execute(RequestData requestData, Action<CommandStatus> complete)
    {
        this.complete = complete;
        var NextRotate = requestData.CharacterView.Transform.rotation;
        var character = requestData.CharacterView.Transform;
        NextRotate = Quaternion.AngleAxis(NextRotate.eulerAngles.y + 90,Vector3.up);
        //requestData.CharacterData.NextRotate = NextRotate;
        MonoHelper.Rotate(character,NextRotate,()=>complete?.Invoke(CommandStatus.Accept));
    }
}
public class RotateCounterClockwise : ICommand
{
    public CommandType Type => CommandType.ClockWiseRotate;
    public CommandStatus Status { get; set; }
    public Action<CommandStatus> complete { get; set; }

    public void Execute(RequestData requestData, Action<CommandStatus> complete)
    {
        this.complete = complete;
        var NextRotate = requestData.CharacterView.Transform.rotation;
        var character = requestData.CharacterView.Transform;
        NextRotate = Quaternion.AngleAxis(NextRotate.eulerAngles.y - 90,Vector3.up);
        //requestData.CharacterData.NextRotate = NextRotate;
        MonoHelper.Rotate(character,NextRotate,()=>complete?.Invoke(CommandStatus.Accept));
    }
}
public class Jump : ICommand
{
    public CommandType Type => CommandType.Jump;
    public CommandStatus Status { get; set; }
    public Action<CommandStatus> complete { get; set; }

    public void Execute(RequestData requestData, Action<CommandStatus> complete)
    {
        this.complete = complete;
        var forwardGround = requestData.GroundsHelper.FindForwardGround(requestData.CharacterData.StartGround,requestData.CharacterView.Transform);
        if (requestData.CharacterData.Height + 0.5f == forwardGround.Height ||
            requestData.CharacterData.Height - 0.5f == forwardGround.Height)
        {
            var NextMove = requestData.CharacterView.Transform.position;
            NextMove += requestData.CharacterView.Transform.forward;
            NextMove.y = forwardGround.Height;
            requestData.CharacterData.Height = forwardGround.Height;
            requestData.CharacterData.CurrentGround = forwardGround;
            MonoHelper.Move(requestData.CharacterView.Transform, NextMove, () => complete?.Invoke(CommandStatus.Accept));
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
    public Action<CommandStatus> complete { get; set; }

    public void Execute(RequestData requestData, Action<CommandStatus> complete)
    {
        this.complete = complete;
        if (requestData.CharacterData.CurrentGround.GetComponent<GroundLight>())
        {
            requestData.CharacterData.CurrentGround.GetComponent<GroundLight>().TurnOnLight();
            complete?.Invoke(CommandStatus.Accept);
            GameManager.OnTurnOnLight();
        }
        else
        {
            complete?.Invoke(CommandStatus.Reject);
        }
    }
}

public class Program1 : ICommand
{
    public CommandType Type => CommandType.Program1;
    public CommandStatus Status { get; set; }
    public Action<CommandStatus> complete { get; set; }

    public void Execute(RequestData requestData, Action<CommandStatus> complete)
    {
        GameManager.CommandsManager.Find(x=>x.name == "Program").RunCommands(requestData,()=>complete?.Invoke(CommandStatus.Accept));
        this.complete = complete;
    }
}
public enum CommandType
{
    Forward,
    ClockWiseRotate,
    CounterClockWiseRotate,
    Jump,
    TurnOnLight,
    Program1
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
    Action<CommandStatus> complete { get; set; }
    void Execute(RequestData requestData,Action<CommandStatus> complete);
}

public class RequestData
{
    public ICharacterView CharacterView;
    public ICharacterData CharacterData;
    public GroundsHelper GroundsHelper;
}