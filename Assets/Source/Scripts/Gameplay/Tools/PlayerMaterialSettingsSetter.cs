using UnityEngine;
using Zenject;

namespace SanderSaveli.GravityMaze
{
    public class PlayerMaterialSettingsSetter : MonoBehaviour
    {
        [SerializeField] private PhysicsMaterial2D _playerMaterial;


        [Inject]
        public void Construct(IGameplayConfig gameplayConfig)
        {
            _playerMaterial.friction = gameplayConfig.Friction;
            _playerMaterial.bounciness = gameplayConfig.Bounciness;
        }
    }
}
