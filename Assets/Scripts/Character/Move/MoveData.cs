using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MoveData
{
    
}

[Serializable]
public class MoveTask
{
    public bool isDone;
    public CharacterMovement.MoveType Type;
    public TaskStatus Status;
}

public enum TaskStatus
{
    None,
    Accept,
    Reject
}
