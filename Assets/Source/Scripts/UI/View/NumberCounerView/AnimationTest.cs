using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class AnimationTest : MonoBehaviour
    {
        [SerializeField] private CounterAnimator slot;

        private async void Start()
        {
            slot.ShowWithoutAnimation(9);
            await UniTask.WaitForSeconds(1);
            await slot.Animate(9, 10);
            await UniTask.WaitForSeconds(1);
            await slot.Animate(10, 11);
            await UniTask.WaitForSeconds(1);
            await slot.Animate(10, 999);
            Debug.Log("COMPLETE");
        }
    }
}
