using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    public const string Running = nameof(Running);
    public const string Jumped = nameof(Jumped);

    [SerializeField] private Animator _animator;

    public void EnableRunning()
    {
        _animator.SetBool(Running, true);
    }

    public void DisableRunning()
    {
        _animator.SetBool(Running, false);
    }
    
    public void EnableJumping()
    {
        _animator.SetBool(Jumped, true);
    }

    public void DisableJumping()
    {
        _animator.SetBool(Jumped, false);
    }
}
