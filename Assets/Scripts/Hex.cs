using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Hex : MonoBehaviour
{
    
    public int i, j;
    int[] tripleHex;
    GameObject frame;
    GameControl gControl;
    bool isTripleSelected;
    int width, hight;

    

    void Start()
    {
        gControl = FindObjectOfType<GameControl>();
        
        width = gControl.width;
        hight = gControl.hight;

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnMouseUp()
    {
        //PC Test 
        //if (SystemInfo.deviceType == DeviceType.Desktop)
        //putFrame(); 
      

        if (gControl.isTouched)
        {
            putFrame();
        }
    }

    void putFrame() {

        tripleHex = new int[6];
        frame = GameObject.Find("frame");

        isTripleSelected = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {

            if (hit.collider == transform.GetComponents<BoxCollider>()[0] && i > 0)
            {


                if (j != 0 && i % 2 == 0)
                {
                    frame.transform.eulerAngles = new Vector3(0, 0, 90);
                    frame.transform.position = new Vector3(transform.position.x - 0.455F, transform.position.y - 0.01F, transform.position.z);
                    tripleHex[0] = i;
                    tripleHex[1] = j;
                    tripleHex[2] = i - 1;
                    tripleHex[3] = j - 1;
                    tripleHex[4] = i - 1;
                    tripleHex[5] = j;
                    isTripleSelected = true;
                }
                else if (j != hight - 1 && i % 2 == 1)
                {
                    frame.transform.eulerAngles = new Vector3(0, 0, 90);
                    frame.transform.position = new Vector3(transform.position.x - 0.455F, transform.position.y - 0.01F, transform.position.z);
                    tripleHex[0] = i;
                    tripleHex[1] = j;
                    tripleHex[2] = i - 1;
                    tripleHex[3] = j;
                    tripleHex[4] = i - 1;
                    tripleHex[5] = j + 1;
                    isTripleSelected = true;
                }



            }
            else if (hit.collider == transform.GetComponents<BoxCollider>()[1] && i > 0 && j < hight - 1)
            {
                if (i % 2 == 0)
                {
                    frame.transform.eulerAngles = new Vector3(0, 0, 30);
                    frame.transform.position = new Vector3(transform.position.x - 0.235F, transform.position.y + 0.37F, transform.position.z);
                    tripleHex[0] = i;
                    tripleHex[1] = j;
                    tripleHex[2] = i - 1;
                    tripleHex[3] = j;
                    tripleHex[4] = i;
                    tripleHex[5] = j + 1;
                    isTripleSelected = true;
                }
                else if (i % 2 == 1)
                {
                    frame.transform.eulerAngles = new Vector3(0, 0, 30);
                    frame.transform.position = new Vector3(transform.position.x - 0.235F, transform.position.y + 0.37F, transform.position.z);
                    tripleHex[0] = i;
                    tripleHex[1] = j;
                    tripleHex[2] = i - 1;
                    tripleHex[3] = j + 1;
                    tripleHex[4] = i;
                    tripleHex[5] = j + 1;
                    isTripleSelected = true;
                }
            }
            else if (hit.collider == transform.GetComponents<BoxCollider>()[2] && i < width - 1 && j < hight - 1)
            {
                if (i % 2 == 0)
                {
                    frame.transform.eulerAngles = new Vector3(0, 0, 90);
                    frame.transform.position = new Vector3(transform.position.x + 0.205F, transform.position.y + 0.37F, transform.position.z);
                    tripleHex[0] = i;
                    tripleHex[1] = j;
                    tripleHex[2] = i;
                    tripleHex[3] = j + 1;
                    tripleHex[4] = i + 1;
                    tripleHex[5] = j;
                    isTripleSelected = true;
                }
                else if (i % 2 == 1)
                {
                    frame.transform.eulerAngles = new Vector3(0, 0, 90);
                    frame.transform.position = new Vector3(transform.position.x + 0.205F, transform.position.y + 0.37F, transform.position.z);
                    tripleHex[0] = i;
                    tripleHex[1] = j;
                    tripleHex[2] = i;
                    tripleHex[3] = j + 1;
                    tripleHex[4] = i + 1;
                    tripleHex[5] = j + 1;
                    isTripleSelected = true;
                }
            }
            else if (hit.collider == transform.GetComponents<BoxCollider>()[3] && i < width - 1)
            {
                if (i % 2 == 0 && j > 0)
                {
                    frame.transform.eulerAngles = new Vector3(0, 0, 30);
                    frame.transform.position = new Vector3(transform.position.x + 0.415F, transform.position.y - 0.01F, transform.position.z);
                    tripleHex[0] = i;
                    tripleHex[1] = j;
                    tripleHex[2] = i + 1;
                    tripleHex[3] = j;
                    tripleHex[4] = i + 1;
                    tripleHex[5] = j - 1;
                    isTripleSelected = true;
                }
                else if (i % 2 == 1 && j < hight - 1)
                {
                    frame.transform.eulerAngles = new Vector3(0, 0, 30);
                    frame.transform.position = new Vector3(transform.position.x + 0.415F, transform.position.y - 0.01F, transform.position.z);
                    tripleHex[0] = i;
                    tripleHex[1] = j;
                    tripleHex[2] = i + 1;
                    tripleHex[3] = j + 1;
                    tripleHex[4] = i + 1;
                    tripleHex[5] = j;
                    isTripleSelected = true;
                }
            }
            else if (hit.collider == transform.GetComponents<BoxCollider>()[4] && i < width - 1 && j > 0)
            {
                if (i % 2 == 0)
                {
                    frame.transform.eulerAngles = new Vector3(0, 0, 90);
                    frame.transform.position = new Vector3(transform.position.x + 0.21F, transform.position.y - 0.38F, transform.position.z);
                    tripleHex[0] = i;
                    tripleHex[1] = j;
                    tripleHex[2] = i + 1;
                    tripleHex[3] = j - 1;
                    tripleHex[4] = i;
                    tripleHex[5] = j - 1;
                    isTripleSelected = true;
                }
                else if (i % 2 == 1)
                {
                    frame.transform.eulerAngles = new Vector3(0, 0, 90);
                    frame.transform.position = new Vector3(transform.position.x + 0.21F, transform.position.y - 0.38F, transform.position.z);
                    tripleHex[0] = i;
                    tripleHex[1] = j;
                    tripleHex[2] = i + 1;
                    tripleHex[3] = j;
                    tripleHex[4] = i;
                    tripleHex[5] = j - 1;
                    isTripleSelected = true;
                }
            }
            else if (hit.collider == transform.GetComponents<BoxCollider>()[5] && i > 0 && j > 0)
            {
                if (i % 2 == 0)
                {
                    frame.transform.eulerAngles = new Vector3(0, 0, 30);
                    frame.transform.position = new Vector3(transform.position.x - 0.24F, transform.position.y - 0.38F, transform.position.z);
                    tripleHex[0] = i;
                    tripleHex[1] = j;
                    tripleHex[2] = i;
                    tripleHex[3] = j - 1;
                    tripleHex[4] = i - 1;
                    tripleHex[5] = j - 1;
                    isTripleSelected = true;
                }
                else if (i % 2 == 1)
                {
                    frame.transform.eulerAngles = new Vector3(0, 0, 30);
                    frame.transform.position = new Vector3(transform.position.x - 0.24F, transform.position.y - 0.38F, transform.position.z);
                    tripleHex[0] = i;
                    tripleHex[1] = j;
                    tripleHex[2] = i;
                    tripleHex[3] = j - 1;
                    tripleHex[4] = i - 1;
                    tripleHex[5] = j;
                    isTripleSelected = true;
                }
            }

        }

        if (isTripleSelected)
        {
            GameObject.Find("GameController").GetComponent<GameControl>().tripleHex = tripleHex;
            GameObject.Find("GameController").GetComponent<GameControl>().isTripleSelected = isTripleSelected;
            // print("Kardeşler:" + tripleHex[0] + " , " + tripleHex[1] + "\n" + tripleHex[2] + " , " + tripleHex[3] + "\n" + tripleHex[4] + " , " + tripleHex[5] + "\n");
        }

    }

    
        

         
}
