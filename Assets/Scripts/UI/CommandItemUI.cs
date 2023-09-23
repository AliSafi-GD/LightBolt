using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandItemUI : MonoBehaviour
{
    private Button btn;

    private Button Btn
    {
        get
        {
            if (btn == null)
                btn = GetComponent<Button>();
            return btn;
        }
    }
    public ICommand Command;

    public void SetData(ICommand command,Action btnAct)
    {
        this.Command = command;
        Btn.onClick.AddListener(()=>btnAct?.Invoke());
        // btn.onClick.AddListener(()=>_manager.AddCommand(FactoryMapper.GetCommand(Type)));
    }
}
