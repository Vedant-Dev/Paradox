using UnityEngine;

public class Action : MonoBehaviour
{
    public static Action instance;

    public RuntimeAnimatorController[] animations;
    public Animator animator;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    public void SetAnimation(int index)
    {
        animator.runtimeAnimatorController = animations[index];
    }
}