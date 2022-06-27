using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTag : MonoBehaviour
{

    [System.Flags]
    public enum TagTypes 
    {
        None,
        Swingable,
        Rotatable,
    }

    public TagTypes Tags;

}