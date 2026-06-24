using SanderSaveli.GravityMaze;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D Rigidbody => _rigidbody;

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private PlayerGravityRotator _gravityRotator;

    public void SetGravity(bool isEnable)
    {
        _gravityRotator.SetEnable(isEnable);
    }
}
