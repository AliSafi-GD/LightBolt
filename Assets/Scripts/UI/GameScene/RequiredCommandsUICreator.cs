using System;
using System.Collections.Generic;
using Commands.Enums;
using Commands.Interface;
using Public;
using UnityEngine;
using Util;

namespace UI.GameScene
{
    [System.Serializable]
    public class RequiredCommandsUICreator
    {
        [SerializeField] private List<CommandItemUI> commandsUI = new List<CommandItemUI>();
        public void Create(List<CommandType> commandsRequired,Transform parent,Action<ICommand> act)
        {
            foreach (var command in commandsRequired)
            {
                var c = UI_Util.CreateCommandUI(command,parent);
                commandsUI.Add(c);
                var Icommand = FactoryMapper.GetCommand(command);
                c.SetData(Icommand,()=>
                {
                    act?.Invoke(Icommand);
                });
            }
        }

        public void Destroy()
        {
            foreach (var commandUI in commandsUI)
            {
                MonoHelper.Destroy(commandUI.gameObject);
            }
            commandsUI.Clear();
        }
    }
}