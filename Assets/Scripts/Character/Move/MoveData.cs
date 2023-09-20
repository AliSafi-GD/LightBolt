using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveData
{
    public Vector3 Direction;

    public bool Equals(Vector3 vec) => Direction.Equals(vec);

}
