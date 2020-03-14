using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField]
    List<Material> houseSkins;

    private void Start()
    {
        MeshRenderer currMesh = GetComponent<MeshRenderer>();
        ChildHouseFlag childFlag = GetComponentInChildren<ChildHouseFlag>();
        GameObject child = (childFlag != null) ? childFlag.gameObject : null;
        MeshRenderer childMesh = (child != null) ? child.GetComponent<MeshRenderer>() : null;
        Material[] materials = currMesh.materials;
        Material[] childMaterials = (childMesh != null) ? childMesh.materials : new Material[0];
        int randomSkin = Random.Range(0, houseSkins.Count);
        
        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].name == "Window_Glass_Opaque (Instance)")
                continue;

            materials[i] = houseSkins[randomSkin];
        }

        currMesh.materials = materials;

        if (childMesh != null)
        {
            for (int i = 0; i < childMaterials.Length; i++)
            {
                if (childMaterials[i].name == "Window_Glass_Opaque (Instance)")
                    continue;

                childMaterials[i] = houseSkins[randomSkin];
            }

            childMesh.materials = childMaterials;
        }
    }
    //public Transform candySpawn;
    //public GameObject candyPickUp;
    //float candyTimer = 0.2f;
    //int candyAmount;
    //bool dispenseCandy = false;
    //bool hasCandy = true;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    candyAmount = Random.Range(1, 5);
    //}

    //void Update()
    //{
    //    if(dispenseCandy && hasCandy)
    //    {
    //        dispenseCandy = false;
    //        Instantiate(candyPickUp, candySpawn.position, Quaternion.identity);
    //        candyAmount--;

    //        if (candyAmount == 0)
    //        {
    //            hasCandy = false;
    //        }

    //        StartCoroutine("CandyTimer");
    //    }
    //}

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
    //        dispenseCandy = true;
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
    //        dispenseCandy = false;
    //}

    //IEnumerator CandyTimer()
    //{
    //    yield return new WaitForSeconds(candyTimer);

    //    dispenseCandy = true;
    //}
}
