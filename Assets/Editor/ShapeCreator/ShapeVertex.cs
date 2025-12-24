using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class ShapeVertex
    {
        public Vector3 Pos;
        public float Angle;

        public ShapeVertex()
        {
            Pos = new Vector3();
            Angle = 0;
        }

        public ShapeVertex(Vector3 pos, float angle)
        {
            Pos = pos;
            Angle = angle;
        }
    }
}
