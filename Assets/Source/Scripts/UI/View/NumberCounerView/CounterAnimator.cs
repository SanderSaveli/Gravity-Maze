using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class CounterAnimator : MonoBehaviour
    {
        [SerializeField] private List<CharSlot> _slots = new List<CharSlot>();

        public void ShowWithoutAnimation(int number)
        {
            string numberString = GetString(number);
            for (int i = 0; i < _slots.Count; i++)
            {
                _slots[i].SetNumberWithoutAnimation(numberString[i]);
            }
        }

        public async UniTask Animate(int firstNumber, int secondNumber)
        {
            string firstString = GetString(firstNumber);
            string secondString = GetString(secondNumber);
            List<UniTask> tasks = new List<UniTask>();
            for(int i =0; i < _slots.Count; i++)
            {
                char f = firstString[i];
                char s = secondString[i];
                if (f != s)
                {
                    tasks.Add(_slots[i].PlayAnimation(f, s));
                }
                else
                {
                    _slots[i].SetNumberWithoutAnimation(s);
                }
            }
            await UniTask.WhenAll(tasks);
        }

        public string GetString(int number)
        {
            string numberStr = (number % (int)Mathf.Pow(10, _slots.Count)).ToString();

            return numberStr.PadLeft(_slots.Count, '0');
        }
    }
}
