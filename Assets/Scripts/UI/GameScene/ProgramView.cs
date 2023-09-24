using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI.GameScene
{
    [System.Serializable]
    public class ProgramView
    {
        private string name;
        public List<CommandItemUI> commandsUI = new List<CommandItemUI>();
        public GameObject Container;

        public ProgramView(string name,Transform rootContainer,Action<string> containerButtonAct)
        {
            this.name = name;
            var containerPrefab = Resources.Load<GameObject>("UI/GameScene/ProgramCommandsContainer");
            Container = MonoHelper.Instantiate(containerPrefab, rootContainer);
            Container.gameObject.name = name;
            Button containerButton;
            if (Container.GetComponent<Button>())
            {
                Container.AddComponent<Button>();
            }

            containerButton = Container.GetComponent<Button>();
            containerButton.onClick.AddListener(()=>containerButtonAct?.Invoke(name));


        }
        public void AddItem(CommandItemUI item)
        {
            commandsUI.Add(item);
        }
        public void Destroy()
        {
            MonoHelper.Destroy(Container.gameObject);
            commandsUI.Clear();
        }

        public static ProgramView Create(string name,Transform rootContainer,Action<string> containerButtonAct)
        {
            return new ProgramView(name,rootContainer,containerButtonAct);
        }
    }
}