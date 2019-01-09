using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridGenerator : MonoBehaviour
{
    public GameObject gridPoint;
    public Transform parent;
    public int widthInMeters = 20;
    public int heightInMeters = 10;
    
    void Start()
    {
        int startXpos = 0;
        int startZpos = 0;

        for (int x = startXpos; x < widthInMeters; x++)
        {
            for (int z = startZpos; z < heightInMeters; z++)
            {
                Instantiate(gridPoint, new Vector3(x, 0, z), Quaternion.identity, parent);
            }
        }
    }
}
