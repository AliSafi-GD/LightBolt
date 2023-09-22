using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandItemUI : MonoBehaviour
{
    [SerializeField] private Button btn;
    public MoveTask task;
    [SerializeField] private CommandsManager _manager;
    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(()=>_manager.AddCommandToMain(task));
    }
}
