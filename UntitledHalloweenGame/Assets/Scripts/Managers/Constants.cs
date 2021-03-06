﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    // level generation
    public const int X_INCREMENT = 60;
    public const int Z_INCREMENT = 60;
    public const int LEVEL_WIDTH = 4;  // range from 0 to 4
    public const int LEVEL_HEIGHT = 4; // range from 0 to 4
    public const float LEVEL_COMPLETION_PERCENTAGE = 0.8f;

    // navmesh baking
    public const int LINK_START_POINT_OFFSET = 29;
    public const int LINK_END_POINT_OFFSET = 31;
    public const int LINK_WIDTH = 60;

    // scoring
    public const int ENEMY_TOTAL_SCORE = 10000;
    public const int CANDY_TOTAL_SCORE = 4000;
}
