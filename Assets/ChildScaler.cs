using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildScaler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in this.transform)
        {
            GameObject obj = child.GetComponent<GameObject>();
            obj.transform.localScale = new Vector3(obj.transform.parent.localScale.x, obj.transform.parent.localScale.y, obj.transform.parent.localScale.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
