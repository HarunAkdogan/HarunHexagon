using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsControl : MonoBehaviour
{
    public Dropdown grid;
    // Start is called before the first frame update
    void Start()
    {
        switch (PlayerPrefs.GetString("gridsize"))
        {
            case "67":
                grid.value = 0;
                break;
            case "78":
                grid.value = 1;
                break;
            case "89":
                grid.value = 2;
                break;
        }

    }

    public void SetGridSize(Dropdown val)
    {
        if (val.value == 0)
        {
            PlayerPrefs.SetString("gridsize", "67");
        }
        if (val.value == 1)
        {
            PlayerPrefs.SetString("gridsize", "78");
        }
        if (val.value == 2)
        {
            PlayerPrefs.SetString("gridsize", "89");
        }

    }

  
}
