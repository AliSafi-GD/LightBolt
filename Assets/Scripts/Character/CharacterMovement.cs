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
    public Vector3 NextMove { get; set; }
    public Quaternion NextRotate { get;set;}
    public GroundItem currentGround{get;set;}
    public GroundItem forwardGround{get;set;}
    public GroundItem StartGround{get;set;}
    public float startRot { get; set; }
}
public class CharacterMovement : MonoBehaviour , ICharacterView , ICharacterData ,IInitializable
{

    private void Start()
    {
        //Reset();
    }
    public void Reset()
    {
        currentGround = StartGround;
        Transform.position = new Vector3(StartGround.position.x,StartGround.Height,StartGround.position.z);
        Transform.rotation = Quaternion.AngleAxis(startRot, Vector3.up);
        NextMove = currentGround.position;
        NextRotate = Quaternion.AngleAxis(startRot, Vector3.up);
        Height = StartGround.Height;
    }

    public Transform Transform => transform;
    public float Height { get; set; }
    public Vector3 NextMove { get; set; }
    public Quaternion NextRotate { get; set; }
    [field:SerializeField]public GroundItem currentGround { get; set; }
    [field:SerializeField]public GroundItem forwardGround { get; set; }
    [field:SerializeField]public GroundItem StartGround { get; set; }
    [field:SerializeField]public float startRot { get; set; }
    public void Init()
    {
        Reset();
    }
}
