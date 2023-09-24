using Commands.Enums;
using UnityEngine;

namespace Util
{
    public static class UI_Util
    {
        public static CommandItemUI CreateCommandUI(CommandType command,Transform parent)
        {
            return MonoHelper.Instantiate(Resources.Load<CommandItemUI>($"UI/Commands/Command item _ {command.ToString()}"),parent);
        }
    }
}