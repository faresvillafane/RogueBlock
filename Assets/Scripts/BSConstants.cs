using System.Collections;

public static class BSConstants {
    public const int CENTER_DIM = 3;

    public const int X_SIZE = 15;
    public const int Z_SIZE = 15;
    public const int Y_SIZE = 1;

    public const int ENEMY_Y_SPAWN = 0;

    public const string TAG_TRAP = "TRAP";
    public const string TAG_LEVEL_EDITOR = "LevelEditor";
    public const string TAG_ENEMY = "Enemy";
    public const string TAG_WATER = "WATER";
    public const string TAG_CENTER_TILE = "CENTER_TILE";
    public const string TAG_TILE = "Tile";
    public const string TAG_GAME_CONTROLLER = "GameController";
    public const string TAG_CANVAS = "CANVAS";
    public const string TAG_HERO = "Player";

    public const int ENEMY_POOL_SIZE = 20;
    public const int SPELL_COST = 20;
    public const int MAX_MANA = 100;

    public const float MIN_DEGREE_ANGLE = .05f;

    public const float MANA_RECOVER_TIME = .01f;
    public const float DISABLE_OBJECT_DELAY = .2f;
    public const float ENEMY_LERP_SPEED = .25f;
    public const float RECOVER_TIME_TILE = 8f;
    public const float ADVANCE_DELAY = 1.5f;
    public const float MIN_SWAP_DISTANCE = 300f;

    public const char LEVEL_LEVEL_SEPARATOR = '#';
    public const char LEVEL_TILE_SEPARATOR = '|';
    public const char LEVEL_TILE_UPDOWN_SEPARATOR = '-';
    public const char LEVEL_SIZE_SEPARATOR = ',';

    public const int DAMAGE_PER_ENEMY = 20;
    public const int NUMBER_OF_TRAPS_PER_RECTANGLE = 2;
    public static readonly float[] ENEMY_SPAWN_CHANCE = {.8f, .9f ,1f };

    public static readonly UnityEngine.Vector3 OBJECT_OUT_OF_SCREEN = new UnityEngine.Vector3(500, 500, 500);

    public const float ROTATION_SPEED = .2f;
    public const float POSITION_OVER_SCENARIO_Y = .5f;

    public const float MIN_LERP_DISTANCE = .1f;

    

    public static readonly UnityEngine.Vector3 V3_SPIKE_POSITION_IN_TILE = new UnityEngine.Vector3(-.9f, -5, -.25f);
    public static readonly UnityEngine.Vector3 V3_SPIKE_SCALE = new UnityEngine.Vector3(2, 20, 2);

    public static readonly UnityEngine.Vector3 V3_ENEMY_POSITION_IN_TILE = new UnityEngine.Vector3(0, 1.5f, 0);
    public static readonly UnityEngine.Vector3 V3_ENEMY_SCALE = new UnityEngine.Vector3(1, 10, 1);

    public static readonly UnityEngine.Vector3 V3_ENTRANCE_POSITION_IN_TILE = new UnityEngine.Vector3(0, 2f, -.2f);
    public static readonly UnityEngine.Vector3 V3_ENTRANCE_SCALE = new UnityEngine.Vector3(10, 100, 10);

}
