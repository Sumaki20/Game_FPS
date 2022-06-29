using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    public void ApplyDamage(float damage)
    {
        //Debug.Log("We got hit damage" + damage);
        health -= damage;

        if(health <= 0f)
        {
            Destroy(gameObject);
        }
    }


}
