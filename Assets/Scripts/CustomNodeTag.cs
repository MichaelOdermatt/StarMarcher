using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomNodeTag : MonoBehaviour
{

    [System.Flags]
    public enum TagTypes 
    {
        None = 0,
        Swingable = 1,
        Rotatable = 2,
        KillsPlayerOnContact = 4,
    }

    public TagTypes Tags;

}