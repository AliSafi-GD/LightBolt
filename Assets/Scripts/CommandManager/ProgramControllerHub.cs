using System.Collections.Generic;
using System.Linq;
using Commands.Interface;
using UI.GameScene;
using UnityEngine;

namespace Manager
{
    [System.Serializable]
    public class ProgramControllerHub
    {
        private List<ProgramController> programControllers;
        public ProgramController activeProgramController;
        public Transform rootView;
        public ProgramControllerHub (Transform rootView,List<Program> commandsManagers)
        {
            this.rootView = rootView;
            var levelDataCommandManagerTypesToList = commandsManagers
                .Select(x =>
                {
                    return new ProgramController(ProgramView.Create(x.name, rootView, ChangeActiveProgram), x);
                }).ToList();
            programControllers = levelDataCommandManagerTypesToList;
        }
        public ProgramController GetProgramController(string name)
        {
            return programControllers.Find(x => x.logic.name == name);
        }

        public void AddCommand(ICommand command) => activeProgramController.AddCommand(command);

        public void ChangeActiveProgram(string name)
        {
            activeProgramController = programControllers.Find(x => x.logic.name == name);
        }

        public void Destroy()
        {
            foreach (var programController in programControllers)
            {
                Debug.Log($"destroy commandManagerPresenter {programController.logic.name}");
                programController.view.Destroy();
                programController.logic.Destroy();
            }
        }
    }
}