using UnityEngine.U2D;

namespace SanderSaveli.GravityMaze
{
    public abstract class ShapeCreator
    {
        public abstract void Draw();
        public abstract void Create(SpriteShapeController controller);
    }
}
