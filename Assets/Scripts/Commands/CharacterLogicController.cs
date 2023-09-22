using System.Collections.Generic;
using System.Threading.Tasks;

public class CharacterLogicController : ICharacterLogicController
{
    private List<ICommand> _commands = new List<ICommand>();
    private ICharacterView _characterView;

    public CharacterLogicController(ICharacterView characterView)
    {
        _characterView = characterView;
    }

    public void Run(List<ICommand> commands)
    {
        _commands = commands;
        AsyncRun();
    }

    private async Task AsyncRun()
    {
        bool continuing = false;
        foreach (var command in _commands)
        {
            //TO DO: Check gameover???

            command.Execute(new RequestData()
            {
                view = _characterView
            }, () => { continuing = true; });

            while (!continuing)
                await Task.Yield();
        }
    }
}
