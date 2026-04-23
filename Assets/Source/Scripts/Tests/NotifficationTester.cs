using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class NotifficationTester : MonoBehaviour
    {
        [SerializeField] private NotifficationView _notifficationView;
        [SerializeField] private Color _color = Color.white;
        [SerializeField] private float _startOffset = 1f;

        private async void Start()
        {
            await UniTask.WaitForSeconds(_startOffset);
            _notifficationView.ShowNewColor(_color);
        }
    }
}
