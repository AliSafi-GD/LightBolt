
using System.Collections.Generic;
using Commands.Enums;
using Ground;
using Manager;
using UnityEngine;
interface ICommandRequired
{
    [field:SerializeField] public List<CommandType> CommandsRequired { get; set; }
}
interface ILevelConfig
{
    [field:SerializeField] public int LightCount { get; set; }
    [field:SerializeField] public int CurrentLightTurnedOn { get; set; }
    [field:SerializeField] public List<GroundItem> Grounds { get; set; }
    [field: SerializeField] public GroundItem StartGround { get; set; }
    [field:SerializeField ] public float StartAngle { get; set; }
}

public class LevelView : MonoBehaviour ,ILevelConfig,ICommandRequired
{
    [field:SerializeField] public int LightCount { get; set; }
    [field:SerializeField] public int CurrentLightTurnedOn { get; set; }
    [field:SerializeField] public List<GroundItem> Grounds { get; set; }
    [field:SerializeField] public GroundItem StartGround { get; set; }
    [field:SerializeField ] public float StartAngle { get; set; }
    [field:SerializeField] public List<CommandType> CommandsRequired { get; set; }

    public List<Program> CommandsManagers = new List<Program>
    {
        new Program()
    };

    private void Start()
    {
        GameManager.OnResetLevel += () =>
        {
            CurrentLightTurnedOn = 0;
        };
    }
}




