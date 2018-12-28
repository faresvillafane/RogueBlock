using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSEnums{

    public enum TileType { ROTATING, TRAP, FIXED, AIR, ENTRANCE, EXIT, ENEMY}
    public enum EnemyType { BASIC = 0, FLOOR_FREEZING = 1, FLOOR_INVERTED = 2 }
    public enum SwipeDirection { FORWARD = 0, BACK = 1, RIGHT = 2, LEFT = 3 }
}
