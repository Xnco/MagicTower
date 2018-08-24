using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class Floor
    {
        public int num;
        public int[,] map;
        public bool isFind;

        public Floor(int num, int[, ] room)
        {
            this.num = num;
            map = room;
            isFind = false;
        }
    }
}

