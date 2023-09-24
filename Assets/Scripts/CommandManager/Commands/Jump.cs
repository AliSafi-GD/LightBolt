using System;
using Commands.Enums;
using Commands.Interface;
using UnityEngine;
using Util;

namespace Commands
{
    public class Jump : ICommand
    {
        public CommandType Type => CommandType.Jump;
        public CommandStatus Status { get; set; }
        public Action<CommandStatus> complete { get; set; }

        public void Execute(RequestData requestData, Action<CommandStatus> complete)
        {
            this.complete = complete;
            var forwardGround = requestData.GroundsHelper.FindForwardGround(requestData.CharacterData.CurrentGround,requestData.CharacterView.Transform);
            Debug.Log($"forward : {forwardGround}");
            if (forwardGround == null)
            {
                complete?.Invoke(CommandStatus.Reject);
                return;
            }

            if (requestData.CharacterData.Height + 0.5f == forwardGround.Height ||
                requestData.CharacterData.Height - 0.5f == forwardGround.Height)
            {
                var NextMove = requestData.CharacterView.Transform.position;
                NextMove += requestData.CharacterView.Transform.forward;
                NextMove.y = forwardGround.Height;
                requestData.CharacterData.Height = forwardGround.Height;
                requestData.CharacterData.CurrentGround = forwardGround;
                MonoHelper.Move(requestData.CharacterView.Transform, NextMove, () =>
                {
                    requestData.CharacterData.CurrentGround = forwardGround;
                    complete?.Invoke(CommandStatus.Accept);
                });
            }
            else
            {
                complete?.Invoke(CommandStatus.Reject);
            }
        }
    }
}