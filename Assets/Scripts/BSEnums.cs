﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSEnums{

    public enum TileType { AIR = 0, ROTATING = 1, TRAP = 2, FIXED = 3, CENTER = 4, INVERSE_ROTATING = 5, ENTRANCE = 6, EXIT = 7 }
    public enum EnemyType { BASIC = 0, FLOOR_FREEZING = 1, FLOOR_INVERTED = 2 }
    public enum SwipeDirection { FORWARD = 0, BACK = 1, RIGHT = 2, LEFT = 3 }
}
