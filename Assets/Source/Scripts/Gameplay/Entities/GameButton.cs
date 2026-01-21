using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SanderSaveli.GravityMaze
{
    public class GameButton : MonoBehaviour
    {
        public bool IsActive {  get; private set; }
        public Action OnActive {  get; set; }   

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Player>(out _))
            {
                if(!IsActive)
                {
                    IsActive = true;
                    OnActive?.Invoke();
                }
            }
        }
    }
}
