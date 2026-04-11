using System;

namespace SanderSaveli.GravityMaze
{
    public interface IAdAdapter
    {
        public bool IsSuccsessShow { get; }
        public Action OnEndShow { get; set; }
    }
}
