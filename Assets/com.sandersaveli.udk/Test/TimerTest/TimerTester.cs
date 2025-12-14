using UnityEngine;
using UnityEngine.UI;

namespace SanderSaveli.UDK
{
    public class TimerTester : MonoBehaviour
    {
        [SerializeField] private Button _addButton;
        [SerializeField] private TimerTestView _timerPrefab;
        [SerializeField] private Transform _container;

        private ObjectPool<TimerTestView> _pool;
        private int _counter = 0;

        private void Awake()
        {
            _pool = new ObjectPool<TimerTestView>(_timerPrefab, _container, isFillAtStart: false);
            _addButton.onClick.AddListener(AddTimer);
        }

        private void AddTimer()
        {
            _counter++;
            int id = _counter;

            var view = _pool.Get();

            TimerHandle handle = Timer.StartTimer(
                10f,
                () =>
                {
                    Debug.Log($"Таймер {id} завершен");
                    view.OnBackToPool?.Invoke(view);
                },
                remaining => view.UpdateTime(remaining)
            );

            view.Bind(id, handle);
        }
    }
}
