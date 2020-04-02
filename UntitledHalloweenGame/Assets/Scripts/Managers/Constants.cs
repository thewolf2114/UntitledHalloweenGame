using System.Collections;
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
    public const int LINK_START_POINT_OFFSET = 25;
    public const int LINK_END_POINT_OFFSET = 35;
    public const int LINK_WIDTH = 60;
}
