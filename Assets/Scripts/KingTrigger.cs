using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingTrigger : MonoBehaviour
{
    public GameObject king;
    public Animator queen;
    public GameObject stunParticle;

    private void OnTriggerEnter(Collider col)
    {
        Destroy(col.gameObject);
        if(!king.activeSelf)
        {
            king.SetActive(true);
        }
        queen.SetTrigger("Damage");
        stunParticle.SetActive(true);
    }
}
