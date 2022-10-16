using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CoroutinesHolder : MonoBehaviour
{
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
