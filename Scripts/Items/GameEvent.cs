using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public const string HEALTH_UPDATE = "HEALTH_UPDATE";
    public const string LEVEL_COMPLETE = "LEVEL_COMPLETE";
    public const string LEVEL_FAILED = "LEVEL_FAILED";
    public const string GAME_COMPLETE = "GAME_COMPLETE";
    public const string ENEMY_DIE = "ENEMY_DIE";
    public const string SPEED_CHANGE = "SPEED_CHANGE";
    public const string POWER_CHANGE = "POWER_CHANGE";
    public const string NO_ENEMY = "NO_ENEMY";
    public const string DEBUG = "DEBUG";
}
