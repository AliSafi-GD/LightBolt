using System;
using System.Collections;
using System.Collections.Generic;
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
    void SetSetup(GroundItem startGround,float startRot,float Height);
    
}


public class Character : MonoBehaviour , ICharacterView , ICharacterData ,IInitializable
{

  
   
    public Transform Transform => transform;
    public float Height { get; set; }
    public GroundItem StartGround { get; set; }
    public GroundItem CurrentGround{get;set;}
    public float startRot { get; set; }
    public void Init()
    {
        
    }

    public void SetSetup(GroundItem startGround, float startRot,float Height)
    {
        StartGround = startGround;
        CurrentGround = startGround;
        this.startRot = startRot;
        Transform.position = StartGround.position;
        Transform.rotation = Quaternion.AngleAxis(startRot, Vector3.up);
        this.Height = Height;
    }
}
