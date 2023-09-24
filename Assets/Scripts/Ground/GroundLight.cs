using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ground
{
    public class GroundLight : MonoBehaviour
    {
        [SerializeField] Material turnOffLightMat;
        [SerializeField] Material turnOnLightMat;
        public bool isTurnOn;

        private void Start()
        {
            Init();
            GameManager.OnResetLevel += TurnOffLight;
        }

        void Init()
        {
            ChangeLight(turnOffLightMat);
        }

        private void OnDestroy()
        {
            GameManager.OnResetLevel -= TurnOffLight;
        }

        void ChangeLight(Material mat)
        {
            var m = new Material(mat);
            m.name = mat.name;
            GetComponent<Renderer>().material = m;
        }

        public void TurnOnLight()
        {
            isTurnOn = true;
            ChangeLight(turnOnLightMat);
        }

        public void TurnOffLight()
        {
            isTurnOn = false;
            ChangeLight(turnOffLightMat);
        }
    }
}
