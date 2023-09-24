using Commands.Interface;
using Public;
using UI.GameScene;
using Util;

namespace Manager
{
    [System.Serializable]
    public class ProgramController
    {
        public ProgramController(ProgramView view,Program logic)
        {
            this.logic = logic;
            this.view = view;
        }
        public Program logic;
        public ProgramView view;
        public void AddCommand(ICommand command)
        {
            if(logic._commands.Count >= logic.LimitCount)
                return;
            var item = UI_Util.CreateCommandUI(command.Type, view.Container.transform);
            view.AddItem(item);
            item.SetData(command, () =>
            {
                RemoveCommand(command);
            });
            logic.AddCommand(command);
        }
        public void RemoveCommand(ICommand command)
        {
            var item = view.commandsUI.Find(x => x.Command == command);
            view.commandsUI.Remove(item);
            MonoHelper.Destroy(item.gameObject);
            logic.RemoveCommand(command);
        }
    }
}