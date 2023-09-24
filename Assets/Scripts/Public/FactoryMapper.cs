
using Commands;
using Commands.Enums;
using Commands.Interface;

namespace Util
{
    public class FactoryMapper
    {
        public static ICommand GetCommand(CommandType type)
        {
            return type switch
            {
                CommandType.Forward => new Forward(),
                CommandType.Jump => new Jump(),
                CommandType.ClockWiseRotate => new RotateClockwise(),
                CommandType.CounterClockWiseRotate => new RotateCounterClockwise(),
                CommandType.TurnOnLight => new TurnOnLight(),
                CommandType.Program1 => new Program1(),
                _ => default
            };
        }
    }
}
