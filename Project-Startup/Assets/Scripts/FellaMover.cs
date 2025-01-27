using System.Collections.Generic;
using UnityEngine;

public class FellaMover : MonoBehaviour
{
    public bool followMouse = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(followMouse == true)
        {
            GameManager.instance.canMove = false;
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

            if (Input.GetMouseButtonUp(0))
            {
                
            }
        }
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(0) && !GameManager.instance.userFrozen)
        {
            GameManager.instance.isHoldingFella = true;
            followMouse = true;
            transform.parent = null;
        }
    }
}
