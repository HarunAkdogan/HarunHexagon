using System.Collections;
using System.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{

    public GameObject hex, bomb, frame;
    public Material red;
    public Material green;
    public Material blue;
    public Material yellow;
    public Material purple;
    public Material orange;
    private List<GameObject> columns;

    public List<List<GameObject>> hexagons = new List<List<GameObject>>();

    public Text scoreTxt, highScoreTxt;
    public int[] tripleHex;
    public bool isTripleSelected = false;
    public bool isMatched = false;
    public bool gameOver = false;
    public bool lastChance = false;
    public bool isTouched = false;

    public int score = 0, highScore = 0, scoreHolder=0;
    public int width = 8, hight = 9;
    public int move = 0;

    private float dragDistance;
    private Vector3 fp;
    private Vector3 lp;

    void Start()
    {
        int i, j;
        float ek;
        frame = GameObject.FindGameObjectWithTag("frame");
        dragDistance = Screen.height * 5 / 100;
        tripleHex = new int[6];
        
        PlayerPrefs.SetString("state", "playing");

        switch (PlayerPrefs.GetString("gridsize"))
        {
            case "67":
                width = 6;
                hight = 7;
                break;
            case "78":
                width = 7;
                hight = 8;
                break;
            case "89":
                width = 8;
                hight = 9;
                break;
        }

        //Instantiate(frame, new Vector3(frame.transform.position.x, frame.transform.position.y, frame.transform.position.z), frame.transform.rotation );
        for (i = 0; i < width; i++)
        {
            columns = new List<GameObject>();
            hexagons.Add(columns);
            for (j = 0; j < hight; j++)
            {
                if (i % 2 == 1)
                    ek = 0.38F;
                else
                    ek = 0;

                columns.Add(Instantiate(hex, new Vector3((float)(i * (0.76 * (Mathf.Sqrt(3) / 2))), (float)(j * 0.76 + ek), 0), hex.transform.rotation));
             
                columns[j].GetComponent<Hex>().i = i;
                columns[j].GetComponent<Hex>().j = j;

                SetRandomMaterial(hexagons[i][j]);
            }
        }

        int k, l;
        for (k = 0; k < width; k++)
        {
            for (l = 0; l < hight; l++)
            {
                while (getSiblings(hexagons, k, l).Count > 0)
                {
                    SetRandomMaterial(hexagons[k][l]);
                }
            }
        }

        highScore = PlayerPrefs.GetInt("highscore");
        highScoreTxt.text = "High Score: " + highScore.ToString();
    }


    void Update()
    {

        //PC Test
        if(Input.GetKeyDown(KeyCode.A) && isTripleSelected)
        {
            tripleTurnLeft();
        }else if (Input.GetKeyDown(KeyCode.D) && isTripleSelected)
        {
            tripleTurnRight();
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                lp = touch.position;

                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {
                    if (isTripleSelected)
                    {
                        if (Mathf.Abs(lp.x - fp.x) < Mathf.Abs(lp.y - fp.y))
                        {
                            if (lp.y > fp.y)
                            {   //Up swipe
                                if (fp.x < frame.transform.position.x)
                                {
                                    tripleTurnRight();
                                }
                                else
                                {
                                    tripleTurnLeft();
                                }
                            }
                            else if (lp.y < fp.y)
                            {   //Down swipe
                                if (fp.x > frame.transform.position.x)
                                {
                                    tripleTurnRight();
                                }
                                else
                                {
                                    tripleTurnLeft();
                                }
                            }
                        }
                    }
                    isTouched = false;
                }
                else
                {
                    isTouched = true;
                }
            }
        }
        if (gameOver)
        {
            PlayerPrefs.SetString("state", "gameover");
            PlayerPrefs.SetInt("score", score);
            SceneManager.LoadScene(0);
        }
    }

    public void tripleTurnLeft()
    {

        for (int i = 0; i < 3; i++)
        {
            turnLeft();
            if (isMatched)
            {
                move++;

                for (int k = 0; k < width; k++)
                {
                    for (int l = 0; l < hight; l++)
                    {
                        //Aslında herkesin kendine ait bir son şansı olmalı.
                        if (hexagons[k][l].GetComponent<Bomb>() != null && !lastChance)
                        {
                            hexagons[k][l].GetComponent<Bomb>().decreaseCount();
                        }
                    }
                }
                isMatched = false;
                break;
            }
        }
    }

    public void tripleTurnRight()
    {

        for (int i = 0; i < 3; i++)
        {
            turnRight();
            if (isMatched)
            {
                move++;

                for (int k = 0; k < width; k++)
                {
                    for (int l = 0; l < hight; l++)
                    {
                        //Aslında herkesin kendine ait bir son şansı olmalı.
                        if (hexagons[k][l].GetComponent<Bomb>() != null && !lastChance)
                        {
                            hexagons[k][l].GetComponent<Bomb>().decreaseCount();
                        }
                    }
                }
                isMatched = false;
                break;
            }
        }
    }

    void SetRandomMaterial(GameObject gameobject)
    {

        MeshRenderer mr = gameobject.transform.GetChild(0).GetComponent<MeshRenderer>();

        switch (Random.Range(1, 7))
        {
            case 1:
                mr.material = yellow;
                break;
            case 2:
                mr.material = red;
                break;
            case 3:
                mr.material = green;
                break;
            case 4:
                mr.material = blue;
                break;
            case 5:
                mr.material = purple;
                break;
            case 6:
                mr.material = orange;
                break;
        }
    }

    //Ortak arkadaşı olan arkadaşlar: kardeşler
    public List<int[]> getSiblings(List<List<GameObject>> hexagons, int i, int j)
    {

        List<int[]> neighbors = getNeighbors(i, j);
        List<int[]> friends = getFriends(neighbors, i, j);
        List<int[]> siblings = new List<int[]>();

        int m, n;
        for (m = 0; m < friends.Count - 1; m++)
        {
            for (n = m + 1; n < friends.Count; n++)
            {
                //if( (float)Mathf.Sqrt(Mathf.Pow(friends[m][0] - friends[n][0],2) + Mathf.Pow(friends[m][1] - friends[n][1],2) ) < 1.5F){
                //if (!((friends[m][0] % 2 == 0 && friends[n][0] % 2 == 1 && friends[m][1] < friends[n][1]) || (friends[m][0] % 2 == 1 && friends[n][0] % 2 == 0 && friends[m][1] > friends[n][1])))

                foreach (int[] neighbor in getNeighbors(friends[m][0], friends[m][1]))
                {
                    if ((neighbor[0] == friends[n][0] && neighbor[1] == friends[n][1]))
                    {
                        if (!siblings.Contains(friends[m]))
                        {
                            siblings.Add(friends[m]);
                        }
                        if (!siblings.Contains(friends[n]))
                        {
                            siblings.Add(friends[n]);
                        }
                    }
                }
            }
        }

        if (siblings.Count > 0)
        {
            siblings.Add(new int[] { i, j });
        }
        return siblings;
    }

    //Altı adet temas eden hücre: komşular
    List<int[]> getNeighbors(int i, int j)
    {
        List<int[]> neighbors = new List<int[]>(); ;

        if (i % 2 == 0)
        {
            neighbors.Add(new int[] { i, j + 1 });
            neighbors.Add(new int[] { i + 1, j });
            neighbors.Add(new int[] { i + 1, j - 1 });
            neighbors.Add(new int[] { i, j - 1 });
            neighbors.Add(new int[] { i - 1, j - 1 });
            neighbors.Add(new int[] { i - 1, j });
        }
        else if (i % 2 == 1)
        {
            neighbors.Add(new int[] { i, j + 1 });
            neighbors.Add(new int[] { i + 1, j + 1 });
            neighbors.Add(new int[] { i + 1, j });
            neighbors.Add(new int[] { i, j - 1 });
            neighbors.Add(new int[] { i - 1, j });
            neighbors.Add(new int[] { i - 1, j + 1 });
        }

        for (int k = 0; k < neighbors.Count; k++)
        {
            if (neighbors[k][0] < 0 || neighbors[k][0] > width - 1 || neighbors[k][1] < 0 || neighbors[k][1] > hight - 1)
            {
                neighbors.Remove(neighbors[k]);
            }
        }
        return neighbors;

    }

    //Aynı renkte olan komşular: arkadaşlar
    List<int[]> getFriends(List<int[]> neighbors, int i, int j)
    {
        List<int[]> friends = new List<int[]>();

        foreach (int[] indexe in neighbors)
        {
            int k = indexe[0], l = indexe[1];

            if (k > -1 && k < width && l > -1 && l <hight)
            {
                if (hexagons.Count > k)
                {
                    if (hexagons[k].Count > l)
                    {
                        if (hexagons[k][l] != null)
                        {
                            if (hexagons[k][l].transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial == hexagons[i][j].transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial)
                            {
                                if (!(k == i && l == j))
                                {
                                    friends.Add(new int[] { k, l });
                                }
                            }
                        }
                    }
                }
            }
        }

        return friends;
    }

    
    //Kardeşleri yok et :) Sütun güncellendikten sonra, yok edilen kardeşlerin ait olduğu sütun için yinelemeli olarak çağrılır.
    void destroySiblings(List<int[]> siblings)
    {

        foreach (int[] index in siblings)
        {
            if (hexagons[index[0]][index[1]] != null)
            {
                //print("Bir" + index[0] + " " + index[1] + "\n");

                for (int k = 0; k < width; k++)
                {
                    for (int l = 0; l < hight; l++)
                    {
                        if(hexagons[k][l] != null)
                        {
                            if (hexagons[k][l].GetComponent<Bomb>() != null)
                            {
                                if (index[0] == k && index[1] == l)
                                {
                                    Destroy(hexagons[k][l].GetComponent<Bomb>().rendererSp);
                                    hexagons[k][l].GetComponent<Bomb>().rendererSp = null;
                                    lastChance = true;
                                }
                            }
                        }
                    }
                }

                Destroy(hexagons[index[0]][index[1]]);

                hexagons[index[0]][index[1]] = null;
                score += 5;
                lastChance = false;
                
                if(score > highScore)
                {
                    highScore = score;
                }

                scoreTxt.text = score.ToString();
                highScoreTxt.text = "High Score: " + highScore.ToString();
                PlayerPrefs.SetInt("highscore", highScore);

                if((index[0] == tripleHex[0] && index[1] == tripleHex[1]) || (index[0] == tripleHex[2] && index[1] == tripleHex[3]) || (index[0] == tripleHex[4] && index[1] == tripleHex[5]) )
                {
                    isMatched = true;
                }
                /*if (isNoMove())
                {
                    Debug.Log("There is no legal move!");
                }*/
            }
        }

        foreach (int[] index in siblings)
        {
            updateColumn(index[0]);
        }

        foreach (int[] index in siblings)
        {
            //print("Bir" + index[0] + " " + index[1] + "\n");

            for (int j = 0; j < hight; j++)
            {
                if (hexagons[index[0]][j] != null)
                    destroySiblings(getSiblings(hexagons, index[0], j));
            }
        }
    }

    void turnRight()
    {
        //Vector3 rotOrigin = new Vector3((hexagons[tripleHex[0]][tripleHex[1]].transform.position.x + hexagons[tripleHex[2]][tripleHex[2]].transform.position.x + hexagons[tripleHex[4]][tripleHex[5]].transform.position.x) / 3
        //  , (hexagons[tripleHex[0]][tripleHex[1]].transform.position.y + hexagons[tripleHex[2]][tripleHex[2]].transform.position.y + hexagons[tripleHex[4]][tripleHex[5]].transform.position.y) / 3 , hexagons[tripleHex[4]][tripleHex[5]].transform.position.z);

        Vector3 tempPos = hexagons[tripleHex[0]][tripleHex[1]].transform.position;

        /*
        for (int i = 0; i < 5; i += 2)
        {
            if (hexagons[tripleHex[i]][tripleHex[i + 1]].GetComponent<Bomb>() != null && isMatched)
            {
                if (i == 4)
                {
                    hexagons[tripleHex[i]][tripleHex[i + 1]].GetComponent<Bomb>().rendererSp.transform.position = tempPos + new Vector3(0, 0, -0.03F);
                }
                else
                {
                    hexagons[tripleHex[i]][tripleHex[i + 1]].GetComponent<Bomb>().rendererSp.transform.position = hexagons[tripleHex[i + 2]][tripleHex[i + 3]].GetComponent<Hex>().transform.position + new Vector3(0, 0, -0.03F); ;
                }
            }
        }*/

        hexagons[tripleHex[0]][tripleHex[1]].transform.position = hexagons[tripleHex[2]][tripleHex[3]].transform.position;
        hexagons[tripleHex[2]][tripleHex[3]].transform.position = hexagons[tripleHex[4]][tripleHex[5]].transform.position;
        hexagons[tripleHex[4]][tripleHex[5]].transform.position = tempPos;


        int tempi = hexagons[tripleHex[0]][tripleHex[1]].GetComponent<Hex>().i;
        int tempj = hexagons[tripleHex[0]][tripleHex[1]].GetComponent<Hex>().j;
        hexagons[tripleHex[0]][tripleHex[1]].GetComponent<Hex>().i = hexagons[tripleHex[2]][tripleHex[3]].GetComponent<Hex>().i;
        hexagons[tripleHex[0]][tripleHex[1]].GetComponent<Hex>().j = hexagons[tripleHex[2]][tripleHex[3]].GetComponent<Hex>().j;
        hexagons[tripleHex[2]][tripleHex[3]].GetComponent<Hex>().i = hexagons[tripleHex[4]][tripleHex[5]].GetComponent<Hex>().i;
        hexagons[tripleHex[2]][tripleHex[3]].GetComponent<Hex>().j = hexagons[tripleHex[4]][tripleHex[5]].GetComponent<Hex>().j;
        hexagons[tripleHex[4]][tripleHex[5]].GetComponent<Hex>().i = tempi;
        hexagons[tripleHex[4]][tripleHex[5]].GetComponent<Hex>().j = tempj;

        GameObject temp = hexagons[tripleHex[4]][tripleHex[5]];
        hexagons[tripleHex[4]][tripleHex[5]] = hexagons[tripleHex[2]][tripleHex[3]];
        hexagons[tripleHex[2]][tripleHex[3]] = hexagons[tripleHex[0]][tripleHex[1]];
        hexagons[tripleHex[0]][tripleHex[1]] = temp;

        if (hexagons[tripleHex[0]][tripleHex[1]] != null)
            destroySiblings(getSiblings(hexagons, tripleHex[0], tripleHex[1]));
        if (hexagons[tripleHex[2]][tripleHex[3]] != null)
            destroySiblings(getSiblings(hexagons, tripleHex[2], tripleHex[3]));
        if (hexagons[tripleHex[4]][tripleHex[5]] != null)
            destroySiblings(getSiblings(hexagons, tripleHex[4], tripleHex[5]));

    }

    void turnLeft()
    {
        Vector3 tempPos = hexagons[tripleHex[4]][tripleHex[5]].transform.position;

        /*
        for (int i = 4; i > -1; i -= 2)
        {
            if (hexagons[tripleHex[i]][tripleHex[i + 1]].GetComponent<Bomb>() != null && isMatched)
            {
                if (i == 0)
                {
                    hexagons[tripleHex[i]][tripleHex[i + 1]].GetComponent<Bomb>().rendererSp.transform.position = tempPos + new Vector3(0, 0, -0.03F);
                }
                else
                {
                    hexagons[tripleHex[i]][tripleHex[i + 1]].GetComponent<Bomb>().rendererSp.transform.position = hexagons[tripleHex[i - 2]][tripleHex[i - 1]].GetComponent<Hex>().transform.position + new Vector3(0, 0, -0.03F); ;
                }
            }
        }
        */

        hexagons[tripleHex[4]][tripleHex[5]].transform.position = hexagons[tripleHex[2]][tripleHex[3]].transform.position;
        hexagons[tripleHex[2]][tripleHex[3]].transform.position = hexagons[tripleHex[0]][tripleHex[1]].transform.position;
        hexagons[tripleHex[0]][tripleHex[1]].transform.position = tempPos;


        int tempi = hexagons[tripleHex[4]][tripleHex[5]].GetComponent<Hex>().i;
        int tempj = hexagons[tripleHex[4]][tripleHex[5]].GetComponent<Hex>().j;
        hexagons[tripleHex[4]][tripleHex[5]].GetComponent<Hex>().i = hexagons[tripleHex[2]][tripleHex[3]].GetComponent<Hex>().i;
        hexagons[tripleHex[4]][tripleHex[5]].GetComponent<Hex>().j = hexagons[tripleHex[2]][tripleHex[3]].GetComponent<Hex>().j;
        hexagons[tripleHex[2]][tripleHex[3]].GetComponent<Hex>().i = hexagons[tripleHex[0]][tripleHex[1]].GetComponent<Hex>().i;
        hexagons[tripleHex[2]][tripleHex[3]].GetComponent<Hex>().j = hexagons[tripleHex[0]][tripleHex[1]].GetComponent<Hex>().j;
        hexagons[tripleHex[0]][tripleHex[1]].GetComponent<Hex>().i = tempi;
        hexagons[tripleHex[0]][tripleHex[1]].GetComponent<Hex>().j = tempj;

        GameObject temp = hexagons[tripleHex[0]][tripleHex[1]];
        hexagons[tripleHex[0]][tripleHex[1]] = hexagons[tripleHex[2]][tripleHex[3]];
        hexagons[tripleHex[2]][tripleHex[3]] = hexagons[tripleHex[4]][tripleHex[5]];
        hexagons[tripleHex[4]][tripleHex[5]] = temp;

        if (hexagons[tripleHex[0]][tripleHex[1]] != null)
            destroySiblings(getSiblings(hexagons, tripleHex[0], tripleHex[1]));
        if (hexagons[tripleHex[2]][tripleHex[3]] != null)
            destroySiblings(getSiblings(hexagons, tripleHex[2], tripleHex[3]));
        if (hexagons[tripleHex[4]][tripleHex[5]] != null)
            destroySiblings(getSiblings(hexagons, tripleHex[4], tripleHex[5]));

    }

    void updateColumn(int i)
    {
        for (int k = 0; k < hight; k++)
        {
            for (int l = k; l < hight; l++)
            {
                if (l == width)
                {
                    if (hexagons[i][l] == null)
                    {
                        float ek;
                        if (i % 2 == 1)
                            ek = 0.38F;
                        else
                            ek = 0;

                        GameObject falling;

                       
                        if (score > 100 && score > scoreHolder)
                        {
                            falling = Instantiate(bomb, new Vector3((float)(i * (0.76 * (Mathf.Sqrt(3) / 2))), (float)(l * 0.76 + ek), 0), bomb.transform.rotation);
                            scoreHolder = score + 50;
                        }
                        else
                        {
                            falling = Instantiate(hex, new Vector3((float)(i * (0.76 * (Mathf.Sqrt(3) / 2))), (float)(l * 0.76 + ek), 0), hex.transform.rotation);
                        }

                        falling.GetComponent<Hex>().i = i;
                        falling.GetComponent<Hex>().j = hight - 1;

                        SetRandomMaterial(falling);
                        hexagons[i][l] = falling;

                        //print(falling.GetComponent<Hex>().i);
                    }
                }
                else
                {
                    if (hexagons[i][l] == null)
                    {
                        if (hexagons[i][l + 1] != null)
                        {
                            float ek;
                            if (i % 2 == 1)
                                ek = 0.38F;
                            else
                                ek = 0;

                            hexagons[i][l + 1].transform.position = new Vector3((float)(i * (0.76 * (Mathf.Sqrt(3) / 2))), (float)(l * 0.76 + ek), 0);
                            hexagons[i][l] = hexagons[i][l + 1];
                            hexagons[i][l + 1] = null;

                            if (hexagons[i][l].GetComponent<Bomb>() != null && hexagons[i][l].GetComponent<Bomb>().rendererSp != null)
                            {
                                hexagons[i][l].GetComponent<Bomb>().rendererSp.transform.position = hexagons[i][l].transform.position + new Vector3(0, 0, -0.03F); ;
                                hexagons[i][l].GetComponent<Bomb>().j -= 1;
                            }
                            else
                            {
                                hexagons[i][l].GetComponent<Hex>().j -= 1;
                            }

                        }
                    }
                }
            }
        }

        /*
        if (isNoMove())
        {
            gameOver = true;
        }*/

    }

    //Har arkadaş ile ortak komşuların  (2 adet) kendi komşuları içinde benimle aynı renkte olan varsa kardeşlik sağlanabilir, mümkün hamle vardır, false döner.
    //Tamamlanmadı
    bool isNoMove()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < hight; j++)
            {
                List<int[]> neighbors = getNeighbors(i, j);
                List<int[]> friends = getFriends(neighbors, i, j);

                foreach (int[] friend in friends)
                {
                    foreach (int[] neighbor in neighbors)
                    {
                        if (getNeighbors(friend[0], friend[1]).Contains<int[]>(neighbor))
                        {
                            if (!(neighbor[0] == i && neighbor[1] == j))
                            {
                                List<int[]> commanNeighborsNeighbors = getNeighbors(neighbor[0], neighbor[1]);
                                foreach (int[] commanNeighborsNeighbor in commanNeighborsNeighbors)
                                {
                                    if (hexagons[commanNeighborsNeighbor[0]][commanNeighborsNeighbor[1]].transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial == hexagons[i][j].transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial)
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }

                /*
                foreach (int[] friend in friends)
                {
                    foreach (int[] neighbor in neighbors)
                    {
                        List<int[]> friendsNeighbors = getNeighbors(friend[0], friend[1]);
                        foreach (int[] friendsNeighbor in friendsNeighbors)
                        {
                            if (!(friendsNeighbor[0] == i && friendsNeighbor[1] == j))
                            {
                                if (friendsNeighbors.Contains<int[]>(neighbor))
                                {
                                    List<int[]> commonFriendsNeighbors = getNeighbors(neighbor[0], neighbor[1]);
                                    foreach (int[] commonFriendsNeighbor in commonFriendsNeighbors)
                                    {
                                        if(hexagons[commonFriendsNeighbor[0]][commonFriendsNeighbor[1]].transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial == hexagons[i][j].transform.GetChild(0).GetComponent<MeshRenderer>().sharedMaterial)
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }*/
            }
        }
                return true;
    }
    //Author -> Harun Akdoğan 09.2020
}
