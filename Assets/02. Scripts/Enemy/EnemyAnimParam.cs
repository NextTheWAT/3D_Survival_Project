using UnityEngine;

public class EnemyAnimParam : MonoBehaviour
{
    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int Walk = Animator.StringToHash("WalkForward");
    public static readonly int Run = Animator.StringToHash("Run Forward");
    public static readonly int Attack = Animator.StringToHash("Attack1");
}

