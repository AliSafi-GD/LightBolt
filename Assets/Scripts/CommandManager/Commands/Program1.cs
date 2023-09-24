using System;
using Commands.Enums;
using Commands.Interface;

namespace Commands
{
    public class Program1 : ICommand
    {
        public CommandType Type => CommandType.Program1;
        public CommandStatus Status { get; set; }
        public Action<CommandStatus> complete { get; set; }

        public void Execute(RequestData requestData, Action<CommandStatus> complete)
        {
            requestData.Manager.programControllerHub.GetProgramController("Program").logic.RunCommands(requestData,()=>complete?.Invoke(CommandStatus.Accept));
            this.complete = complete;
        }
    }
}