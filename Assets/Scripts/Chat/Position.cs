using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chat
{
    public class Position
    {
        public static Vector2 FitAreaNodeAncUpLeft(Vector2 areaSize, Vector2 nodeSize, Vector2 pos)
        {
            var ret = new Vector2(0, 0);

            Debug.Log("Random position: " + pos);

            // area origin(0, 0) is center.
            float left = areaSize.x / 2;
            float right =  left - nodeSize.x; // node anchor is upper left ,so right side on the area need much space of node. 
            float upper = areaSize.y / 2;
            float bottom = upper - nodeSize.y; // node anchor is upper left ,so bottom side on the area need much space of node. 

            ret.x = (pos.x > -left) ? pos.x : -left; 
            ret.x = (ret.x < right) ? ret.x : right; 

            ret.y = (pos.y > -bottom) ? pos.y : -bottom; 
            ret.y = (ret.y < upper) ? ret.y : upper; 

            return ret;
        }

        public static Vector2 RndPos(Vector2 area)
        {
            // area origin(0, 0) is center
            var ratex = Random.value;
            var signx = Random.value < 0.5 ? -1 : 1;
            var ratey = Random.value;
            var signy = Random.value < 0.5 ? -1 : 1;

            return new Vector2(area.x * ratex * signx , area.y * ratey * signy);
        }

    }
}
