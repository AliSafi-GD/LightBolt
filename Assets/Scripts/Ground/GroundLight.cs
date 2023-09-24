using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundLight : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] Material turnOffLightMat;
    [SerializeField] Material turnOnLightMat;

    private void Start()
    {
        Init();
        GameManager.OnResetLevel += TurnOffLight;
    }

    void Init()
    {
        ChangeLight(turnOffLightMat);
    }

    private void Update()
    {
          //ChangeLight(turnOffLightMat);
    }

    void ChangeLight(Material mat)
    {
        var m = new Material(mat);
        m.name = mat.name;
        GetComponent<Renderer>().material = m;
    }

    public void TurnOnLight()
    {
        ChangeLight(turnOnLightMat);
    }
    public void TurnOffLight()
    {
        ChangeLight(turnOffLightMat);
    }
}
