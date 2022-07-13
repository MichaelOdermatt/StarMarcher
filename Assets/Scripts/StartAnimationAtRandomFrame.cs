using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StartAnimationAtRandomFrame : MonoBehaviour
{
    private void Awake()
    {
        Animator animator = GetComponent<Animator>();
        animator.Play("Star Animation", -1, Random.Range(0.0f, 1.0f));
    }
}
