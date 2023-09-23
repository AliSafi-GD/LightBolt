using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

interface ICommandRequired
{
    [field:SerializeField] public List<CommandType> CommandsRequired { get; set; }
}


