using System;
using UnityEngine;


public class CubeBehaviour : MonoBehaviour
{
    public Action<CubeBehaviour> iSKilled;

    private float m_Speed;
    private float m_Distance;

    public void Initialize(float speed, float distance)
    {
        m_Speed = speed;
        m_Distance = distance;
    }

    public void Kill()
    {
        iSKilled?.Invoke(this);
    }

    private void Move()
    {
        transform.position += Vector3.forward * m_Speed * Time.deltaTime;
    }

    private void Update()
    {
        if (isReachedFinalDestination())
        {
            Die();
        }
        else
        {
            Move();
        }
    }

    private bool isReachedFinalDestination()
    {
        return transform.position.z > m_Distance ? true : false;
    }

    private void Die()
    {
        iSKilled?.Invoke(this);
    }

}
