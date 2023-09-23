using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandItemUI : MonoBehaviour
{
    private Button btn;
    public CommandType Type;
    [SerializeField] private GameManager _manager;
    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(()=>_manager.AddCommand(FactoryMapper.GetCommand(Type)));
    }
}
