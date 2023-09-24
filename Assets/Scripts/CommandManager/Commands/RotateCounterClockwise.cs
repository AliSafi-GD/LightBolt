using System;
using Commands.Enums;
using Commands.Interface;
using UnityEngine;
using Util;

namespace Commands
{
    public class RotateCounterClockwise : ICommand
    {
        public CommandType Type => CommandType.ClockWiseRotate;
        public CommandStatus Status { get; set; }
        public Action<CommandStatus> complete { get; set; }

        public void Execute(RequestData requestData, Action<CommandStatus> complete)
        {
            this.complete = complete;
            var NextRotate = requestData.CharacterView.Transform.rotation;
            var character = requestData.CharacterView.Transform;
            NextRotate = Quaternion.AngleAxis(NextRotate.eulerAngles.y - 90,Vector3.up);
            //requestData.CharacterData.NextRotate = NextRotate;
            MonoHelper.Rotate(character,NextRotate,()=>
            {
                complete?.Invoke(CommandStatus.Accept);
            });
        }
    }
}