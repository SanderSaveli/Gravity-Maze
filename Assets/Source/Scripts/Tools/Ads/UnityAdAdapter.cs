using System;

namespace SanderSaveli.GravityMaze
{
    public class UnityAdAdapter : IAdAdapter
    {
        public Action OnEndShow { get; set; }

        public bool IsSuccsessShow { get; private set; }

        public UnityAdAdapter(bool isSuccessShow)
        {
            IsSuccsessShow = isSuccessShow;
        }

        public void Complete()
        {
            OnEndShow?.Invoke();
        }
    }
}
