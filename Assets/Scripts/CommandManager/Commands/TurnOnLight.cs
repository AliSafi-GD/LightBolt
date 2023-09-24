using System;
using Commands.Enums;
using Commands.Interface;
using Ground;

namespace Commands
{
    public class TurnOnLight : ICommand
    {
        public CommandType Type => CommandType.TurnOnLight;
        public CommandStatus Status { get; set; }
        public Action<CommandStatus> complete { get; set; }

        public void Execute(RequestData requestData, Action<CommandStatus> complete)
        {
            this.complete = complete;
            var groundLight = requestData.CharacterData.CurrentGround.GetComponent<GroundLight>();
            if (groundLight && !groundLight.isTurnOn)
            {
                groundLight.TurnOnLight();
                complete?.Invoke(CommandStatus.Accept);
                GameManager.OnTurnOnLight();
            }
            else
            {
                complete?.Invoke(CommandStatus.Reject);
            }
        }
    }
}