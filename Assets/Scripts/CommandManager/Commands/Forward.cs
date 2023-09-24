using System;
using Commands.Enums;
using Commands.Interface;
using UnityEngine;
using Util;

namespace Commands
{
    public class Forward : ICommand
    {
        public CommandType Type => CommandType.Forward;
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
            if (requestData.CharacterData.Height == forwardGround.Height)
            {
                var NextMove = requestData.CharacterView.Transform.position;
                var character = requestData.CharacterView.Transform;
                NextMove += character.forward;
                //requestData.CharacterData.NextMove = NextMove;
            
                MonoHelper.Move(character,NextMove,()=>
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