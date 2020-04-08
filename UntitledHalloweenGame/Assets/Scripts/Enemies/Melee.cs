using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Pausable
{
    public bool Strike { get; set; }

    // Start is called before the first frame update
    override protected void Start()
    {
        Strike = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPaused)
            return;

        if (Strike)
        {
            Collider[] hitCollider = Physics.OverlapSphere(transform.position, 0.3f);
            int i = 0;
            while (i < hitCollider.Length)
            {
                if (hitCollider[i].gameObject.name == "Player2(Clone)")
                {
                    hitCollider[i].gameObject.GetComponent<PlayerHealth>().Damage();
                    break;
                }
                i++;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}
