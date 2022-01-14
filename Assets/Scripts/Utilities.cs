using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static Vector3 Wrap(Vector3 v, Vector3 min, Vector3 max)
    {
        Vector3 range = new Vector3(max.x - min.x, max.y - min.y, max.z - min.z);

        return new Vector3
            (v.x > max.x ? (min.x + ((v.x - max.x) % range.x)) : v.x < min.x ? (max.x - ((min.x - v.x) % range.x)) : v.x,
            v.y > max.y ? (min.y + ((v.y - max.y) % range.y)) : v.y < min.y ? (max.y - ((min.y - v.y) % range.y)) : v.y,
            v.z > max.z ? (min.z + ((v.z - max.z) % range.z)) : v.z < min.z ? (max.z - ((min.z - v.z) % range.z)) : v.z);
    }
}
