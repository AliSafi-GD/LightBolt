
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
                _ => default
            };
        }
    }
