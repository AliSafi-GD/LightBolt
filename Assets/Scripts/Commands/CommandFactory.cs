public class CommandFactory
{
    public ICommand GetCommand(CharacterMovement.MoveType moveType)
    {
        switch (moveType)
        {
            case CharacterMovement.MoveType.Forward:
                return new CommandForward();
            case CharacterMovement.MoveType.ClockWiseRotate:
                break;
            case CharacterMovement.MoveType.CounterClockWiseRotate:
                break;
            case CharacterMovement.MoveType.Jump:
                break;
            case CharacterMovement.MoveType.TurnOnLight:
                break;
            default:
                break;
        }

        return null;
    }
}