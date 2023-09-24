using System;
using Commands.Enums;

namespace Commands.Interface
{
    public interface ICommand
    {
        CommandType Type{get;}
        CommandStatus Status { get; set; }
        Action<CommandStatus> complete { get; set; }
        void Execute(RequestData requestData,Action<CommandStatus> complete);
    }
}