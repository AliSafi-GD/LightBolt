using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ground
{
    public class GroundItem : MonoBehaviour
    {
        public float Height;
        public Transform center;
        public Vector3 position => transform.position;
    }
}
