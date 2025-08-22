using UnityEngine;

public static class EnemyAnimParam
{
    public static readonly int Idle = Animator.StringToHash("Idle");
    public static readonly int Walk = Animator.StringToHash("WalkForward");
    public static readonly int Run = Animator.StringToHash("Run Forward");
    public static readonly int[] Attack = new int[]
    {
        Animator.StringToHash("Attack1"),
        Animator.StringToHash("Attack2"),
        Animator.StringToHash("Attack3"),
        Animator.StringToHash("Attack5"),
    };
    public static readonly int Death = Animator.StringToHash("Death");
}