using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public ResourceObject resourceObject;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RunTest();
        }

    }
    void RunTest()
    {
        int id = 0;
        resourceObject.TryHarvest(out id);
        Debug.Log("TryHarvest: "+ id);

    }
}
