using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public int Height;
    public Transform center;
    public Vector3 position => center.position;
}
