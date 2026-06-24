using System;

namespace SanderSaveli.GravityMaze
{
    public class AppodealAdAdapter : IAdAdapter
    {
        public Action OnEndShow { get; set; }
        public bool IsSuccsessShow { get; private set; }

        public AppodealAdAdapter(bool isSuccessShow)
        {
            IsSuccsessShow = isSuccessShow;
        }

        public void Complete()
        {
            OnEndShow?.Invoke();
        }
    }
}
