using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// rename to CustomNodeTag, use this rather than the default node tag.
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