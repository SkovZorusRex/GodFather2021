using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlyerInputTest : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        RaycastHit rayInfo;

        Physics.Raycast(transform.position, Vector3.right, out rayInfo);

        Debug.DrawRay(transform.position, Vector3.right * 10);

        /*if (rayInfo.collider.gameObject.CompareTag("Ennemy"))
        {
            Debug.Log("touché")
        }*/
    }
}
