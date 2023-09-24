using System;
using System.Collections;
using System.Collections.Generic;
using Commands;
using Commands.Interface;
using UnityEngine;
using Util;

namespace Manager
{
    [Serializable]
    public class Program
    {
        
        public string name;
        public int LimitCount;

        public List<ICommand> _commands = new List<ICommand>();

    
        public void Destroy()
        {
            _commands.Clear();
        }
        public void AddCommand(ICommand command)
        {
            Debug.Log($"add command to {name}");
            _commands.Add(command);
        }
    
        public void RemoveCommand(ICommand command)
        {
            Debug.Log($"add command to {name}");
            _commands.Remove(command);
        }
        public void RunCommands(RequestData requestData,Action OnFinish)
        {
            Debug.Log($"Run Commands {name}");
            MonoHelper.RunCoroutine(IE_RunCommands(requestData,OnFinish));
        }
        IEnumerator IE_RunCommands(RequestData requestData,Action OnFinish)
        {
            Debug.Log($"commands count : {_commands.Count} in {name}");
            foreach (var command in _commands)
            {
                Debug.Log($"start {command.Type}");
                bool isDone = false;
                command.Execute(requestData, (x) => isDone = true);
                yield return new WaitUntil(()=>isDone);
                Debug.Log($"end {command.Type}");
            }
            OnFinish?.Invoke();
        }
    }
}