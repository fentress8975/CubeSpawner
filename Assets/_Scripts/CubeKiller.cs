using UnityEngine;

public class CubeKiller : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CubeBehaviour target))
        {
            target.Kill();
        }
    }
}
