using System;
using System.Collections;
using System.Collections.Generic;
using Ground;
using UnityEngine;

public interface ICharacterView
{
    Transform Transform { get; }
}

public interface ICharacterData
{
    public float Height { get; set; }
    public GroundItem StartGround{get;set;}
    public GroundItem CurrentGround{get;set;}
    public float startRot { get; set; }
    void Init(GroundItem startGround,float startRot,float Height);
    
}


public class Character : MonoBehaviour , ICharacterView , ICharacterData 
{
    public Transform Transform => transform;
    [field:SerializeField]public float Height { get; set; }
    public GroundItem StartGround { get; set; }
    [field:SerializeField]public GroundItem CurrentGround{get;set;}
    public float startRot { get; set; }
    private void Start()
    {
        GameManager.OnResetLevel += () =>
        {
            Init(StartGround,startRot,StartGround.Height);
        };
    }

    public void Init(GroundItem startGround, float startRot,float Height)
    {
        StartGround = startGround;
        CurrentGround = startGround;
        this.startRot = startRot;
        Transform.position = StartGround.position;
        Transform.rotation = Quaternion.AngleAxis(startRot, Vector3.up);
        this.Height = Height;
    }
}
