using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Hex
{
    
    public Sprite [] sprite;
    public int countDown;
    public SpriteRenderer rendererSp;
    private GameControl gControl;
    public GameObject count;

    void Start()
    {
        gControl =  GameObject.FindObjectOfType<GameControl>();
        count = new GameObject("Count");
        rendererSp = count.AddComponent<SpriteRenderer>();

        countDown = UnityEngine.Random.Range(3, 7);

        rendererSp.sprite = sprite[countDown];
        rendererSp.transform.localScale = new Vector3(2, 2, 0);
        rendererSp.transform.position = transform.position + new Vector3(0,0,-0.03F);
      
    }

    // Update is called once per frame
    void Update()
    {
        rendererSp.transform.position = transform.position + new Vector3(0, 0, -0.03F);
        //count.transform.position = transform.position + new Vector3(0, 0, -0.03F);

        if (countDown == 0 && !gControl.lastChance)
        {
            gControl.gameOver = true;
        }
    }

    public void decreaseCount()
    {
        countDown--;
        if(countDown > -1)
            rendererSp.sprite = sprite[countDown];
    }
}
