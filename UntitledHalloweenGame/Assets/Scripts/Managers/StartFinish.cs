using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFinish : MonoBehaviour
{
    [SerializeField]
    private Material idleMat, startMat;

    // timer
    float waitTimer = 1;

    //flags
    bool waiting = false;
    bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material = idleMat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (waiting)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !started)
        {
            GameManager.Instance.StartGameTimer();
            GetComponent<MeshRenderer>().material = startMat;
            waiting = true;
            started = true;
            StartCoroutine(Wait());
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player") && started)
        {
            waiting = true;
            GameManager.Instance.EndGame();
            GetComponent<MeshRenderer>().material = idleMat;
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTimer);

        waiting = false;
    }
}
