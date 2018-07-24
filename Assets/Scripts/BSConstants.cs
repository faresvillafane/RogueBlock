using System.Collections;

public static class BSConstants {
    public const int CENTER_DIM = 3;

    public const int X_SIZE = 10;
    public const int Z_SIZE = 10;
    public const int Y_SIZE = 1;

    public const int ENEMY_Y_SPAWN = 0;
    public const string TAG_TRAP = "TRAP";
    public const string TAG_ENEMY = "ENEMY";
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

    public const int DAMAGE_PER_ENEMY = 20;
    public const int NUMBER_OF_TRAPS_PER_RECTANGLE = 2;
    public static readonly float[] ENEMY_SPAWN_CHANCE = {.8f, .9f ,1f };

    public static readonly UnityEngine.Vector3 OBJECT_OUT_OF_SCREEN = new UnityEngine.Vector3(500, 500, 500);

    public const float ROTATION_SPEED = .2f;

}
