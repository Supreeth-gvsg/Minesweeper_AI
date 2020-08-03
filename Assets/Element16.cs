using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
public class Element16 : MonoBehaviour
{
    public bool mine;
    public bool flag;
    public Sprite[] emptyTextures;
    public Sprite mineTexture, flagTexture, defaultTexture, emptyTexture;
    System.Random r = new System.Random();
    public static Element16 ele;
    public int prob;
    public static int done = 0;
    void Start()
    {
        //playfield16.minecount = 0;
        //playfield16.minesleft = 0;
        flag = false;
        if (playfield16.minecount < 35)
            mine = UnityEngine.Random.value < 0.15;
        if (mine)
        {
            playfield16.minecount++;
            playfield16.minesleft++;
        }
        int q = (int)transform.position.x;
        int p = (int)transform.position.y;
        playfield16.elements[q, p] = this;
        ele = this;
        for (int i = 0; i < playfield16.w; i++)
            for (int j = 0; j < playfield16.h; j++)
                playfield16.visit[i, j] = false;
        playfield16.s = 0;
        int x, y;//
                 // playfield16.visit[x, y] = true;//
        while (playfield16.s < 3)
        { //
            x = r.Next(playfield16.w);//
            y = r.Next(playfield16.h);//
            StartCoroutine(fncall(x, y));//
            playfield16.s++;
        }
        prob = 0;

    }
    public void loadTexture(int adjacentCount)//
    {
        if (mine)
        {
            GetComponent<SpriteRenderer>().sprite = mineTexture;
        }
        else
            GetComponent<SpriteRenderer>().sprite = emptyTextures[adjacentCount];
    }
    private static IEnumerator WaitForSceneLoadlost()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("LOST");

    }
    private static IEnumerator fncall(int x, int y)
    {
        yield return new WaitForSeconds(1);
        uncover(x, y);


    }
    private IEnumerator WaitForSceneLoadwon()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("WON");

    }
    public void load(int str)
    {
        if(str==0)
            StartCoroutine(WaitForSceneLoadlost());
        else
            StartCoroutine(WaitForSceneLoadwon());
    }
    public static void uncover(int x, int y)
    {
        if (playfield16.elements[x, y].mine && playfield16.visit[x, y] == false)
        {
            playfield16.uncoverMines();
            playfield16.visit[x, y] = true;
            //playfield16.minecount = 0;
            //playfield16.minesleft = 0;
            print("You Lost");
            done = 1;
            //playfield16.elements[x, y].load(0);
        }
        if (!playfield16.elements[x, y].mine && playfield16.visit[x, y] == false)
        {
            int p = x;
            int q = y;
            playfield16.visit[x, y] = true;
            int n = playfield16.adjacentMines(p, q);
            playfield16.elements[p, q].GetComponent<SpriteRenderer>().sprite = playfield16.elements[p, q].emptyTextures[n];
            playfield16.FFuncover(x, y, new bool[playfield16.w, playfield16.h]);
            markflag();
            if (playfield16.isFinished())
            {
                //  yield return new WaitForSeconds(4);
                //StartCoroutine(WaitForSceneLoadwon());
                done = 1;
                //playfield16.minecount = 0;
                //playfield16.minesleft = 0;
                //playfield16.elements[x, y].load(1);
                print("YOU WON");
            }
        }
    }
    public int Countflags()
    {
        int x = (int)this.transform.position.x;
        int y = (int)this.transform.position.y;
        int count = 0;
        if (x >= 1 && x <= 14 && y >= 1 && y <= 14)
        {
            if (playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                count++;
            if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                count++;
            if (playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                count++;
            if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                count++;
            if (playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                count++;
            if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                count++;
            if (playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                count++;
            if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                count++;
        }
        else if ((x == 0 && y == 0) || (x == 0 && y == 15) || (x == 15 && y == 0) || (x == 15 && y == 15))
        {
            if (x == 0 && y == 0)
            {
                if (playfield16.elements[0, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[1, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[1, 0].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
            }
            if (x == 0 && y == 15)
            {
                if (playfield16.elements[1, 15].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[1, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[0, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
            }
            if (x == 15 && y == 0)
            {
                if (playfield16.elements[15, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[14, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[14, 0].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
            }
            if (x == 15 && y == 15)
            {
                if (playfield16.elements[15, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[14, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[14, 15].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
            }

        }
        else
        {
            if (x == 0 && y > 0 && y < 15)
            {
                if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
            }
            if (x == 15 && y > 0 && y < 15)
            {
                if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
            }
            if (y == 0 && x > 0 && x < 15)
            {
                if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
            }
            if (y == 15  && x > 0 && x < 15)
            {
                if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
                if (playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].flagTexture)
                    count++;
            }
        }
        return count;
    }
    public int countdef()
    {
        int x = (int)this.transform.position.x;
        int y = (int)this.transform.position.y;
        int count = 0;
        if (x >= 1 && x <= 14 && y >= 1 && y <= 14)
        {
            if (playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                count++;
            if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                count++;
            if (playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                count++;
            if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                count++;
            if (playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                count++;
            if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                count++;
            if (playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                count++;
            if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                count++;
        }
        else if ((x == 0 && y == 0) || (x == 0 && y == 15) || (x == 15 && y == 0) || (x == 15 && y == 15))
        {
            if (x == 0 && y == 0)
            {
                if (playfield16.elements[0, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[1, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[1, 0].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
            }
            if (x == 0 && y == 15)
            {
                if (playfield16.elements[1, 15].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[1, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[0, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
            }
            if (x == 15 && y == 0)
            {
                if (playfield16.elements[15, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[14, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[14, 0].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
            }
            if (x == 15 && y == 15)
            {
                if (playfield16.elements[15, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[14, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[14, 15].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
            }

        }
        else
        {
            if (x == 0 && y > 0 && y < 15)
            {
                if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
            }
            if (x == 15 && y > 0 && y < 15)
            {
                if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
            }
            if (y == 0 && x > 0 && x < 15)
            {
                if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
            }
            if (y == 15 && x > 0 && x < 15)
            {
                if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
                if (playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                    count++;
            }
        }
        return count;
    }
    public static void markflag()
    {
        foreach (Element16 elem in playfield16.elements)
        {
            int x = (int)elem.transform.position.x;
            int y = (int)elem.transform.position.y;
            if (elem.GetComponent<SpriteRenderer>().sprite == elem.emptyTexture)
            {
                playfield16.visit[x, y] = true;

            }
            if (elem.GetComponent<SpriteRenderer>().sprite != elem.defaultTexture && elem.GetComponent<SpriteRenderer>().sprite != elem.emptyTexture && elem.GetComponent<SpriteRenderer>().sprite != elem.flagTexture)
            {
                int n = playfield16.adjacentMines(x, y);
                int count = 0;
                playfield16.visit[x, y] = true;
                int nfls = elem.Countflags();
                if (x >= 1 && x <= 14 && y >= 1 && y <= 14)
                {
                    if (playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        count++;
                    if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        count++;
                    if (playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        count++;
                    if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        count++;
                    if (playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        count++;
                    if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        count++;
                    if (playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        count++;
                    if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        count++;
                    if (n - nfls == count)
                    {
                        if (playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        {
                            playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                            playfield16.elements[x - 1, y + 1].flag = true;
                            playfield16.visit[x - 1, y + 1] = true;
                            playfield16.minesleft--;
                        }
                        if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        {
                            playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                            playfield16.elements[x, y + 1].flag = true;
                            playfield16.visit[x, y + 1] = true;
                            playfield16.minesleft--;
                        }
                        if (playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        {
                            playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                            playfield16.elements[x + 1, y + 1].flag = true;
                            playfield16.visit[x + 1, y + 1] = true;
                            playfield16.minesleft--;
                        }
                        if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        {
                            playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                            playfield16.elements[x + 1, y].flag = true;
                            playfield16.visit[x + 1, y] = true;
                            playfield16.minesleft--;
                        }
                        if (playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        {
                            playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                            playfield16.elements[x + 1, y - 1].flag = true;
                            playfield16.visit[x + 1, y - 1] = true;
                            playfield16.minesleft--;
                        }
                        if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        {
                            playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                            playfield16.elements[x, y - 1].flag = true;
                            playfield16.visit[x, y - 1] = true;
                            playfield16.minesleft--;
                        }
                        if (playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        {
                            playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                            playfield16.elements[x - 1, y - 1].flag = true;
                            playfield16.visit[x - 1, y - 1] = true;
                            playfield16.minesleft--;
                        }
                        if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                        {
                            playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                            playfield16.elements[x - 1, y].flag = true;
                            playfield16.visit[x - 1, y] = true;
                            playfield16.minesleft--;
                        }
                    }
                }
                else if ((x == 0 && y == 0) || (x == 0 && y == 15) || (x == 15 && y == 0) || (x == 15 && y == 15))
                {
                    count = 0;
                    if (x == 0 && y == 0)
                    {
                        if (playfield16.elements[1, 0].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[1, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[0, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (playfield16.elements[0, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[0, 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[0, 1].flag = true;
                                playfield16.visit[0, 1] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[1, 0].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[1, 0].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[1, 0].flag = true;
                                playfield16.visit[1, 0] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[1, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[1, 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[1, 1].flag = true;
                                playfield16.visit[1, 1] = true;
                                playfield16.minesleft--;
                            }
                        }
                    }
                    if (x == 0 && y == 15)
                    {
                        if (playfield16.elements[1, 15].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[1, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[0, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (playfield16.elements[1, 15].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[1, 15].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[1, 15].flag = true;
                                playfield16.visit[1, 15] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[1, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[1, 14].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[1, 14].flag = true;
                                playfield16.visit[1, 14] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[0, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[0, 14].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[0, 14].flag = true;
                                playfield16.visit[0, 14] = true;
                                playfield16.minesleft--;
                            }
                        }
                    }
                    if (x == 15 && y == 0)
                    {
                        if (playfield16.elements[15, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[14, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[14, 0].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (playfield16.elements[15, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[15, 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[15, 1].flag = true;
                                playfield16.visit[15, 1] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[14, 0].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[14, 0].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[14, 0].flag = true;
                                playfield16.visit[14, 0] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[14, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[14, 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[14, 1].flag = true;
                                playfield16.visit[14, 1] = true;
                                playfield16.minesleft--;
                            }
                        }
                    }
                    if (x == 15 && y == 15)
                    {
                        if (playfield16.elements[15, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[14, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[14, 15].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (playfield16.elements[15, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[15 ,14].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[15, 14].flag = true;
                                playfield16.minesleft--;
                                playfield16.visit[15, 14] = true;
                            }
                            if (playfield16.elements[14, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[14, 14].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[14, 14].flag = true;
                                playfield16.visit[14, 14] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[14, 15].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[14, 15].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[14, 15].flag = true;
                                playfield16.visit[14, 15] = true;
                                playfield16.minesleft--;
                            }
                        }
                    }

                }
                else
                {
                    count = 0;
                    if (x == 0 && y > 0 && y < 15)
                    {
                        if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[0, 1].defaultTexture)
                            {
                                playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[0, 1].flagTexture;
                                playfield16.elements[x, y - 1].flag = true;
                                playfield16.visit[x, y - 1] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[0, 1].defaultTexture)
                            {
                                playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[0, 1].flagTexture;
                                playfield16.elements[x, y + 1].flag = true;
                                playfield16.visit[x, y + 1] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[0, 1].defaultTexture)
                            {
                                playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[0, 1].flagTexture;
                                playfield16.elements[x + 1, y + 1].flag = true;
                                playfield16.visit[x + 1, y + 1] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[0, 1].defaultTexture)
                            {
                                playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite = playfield16.elements[0, 1].flagTexture;
                                playfield16.elements[x + 1, y].flag = true;
                                playfield16.minesleft--;
                                playfield16.visit[x + 1, y] = true;
                            }
                            if (playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[0, 1].defaultTexture)
                            {
                                playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[0, 1].flagTexture;
                                playfield16.elements[x + 1, y - 1].flag = true;
                                playfield16.visit[x + 1, y - 1] = true;
                                playfield16.minesleft--;
                            }
                        }
                    }
                    if (x == 15 && y > 0 && y < 15)
                    {
                        if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[x, y - 1].flag = true;
                                playfield16.visit[x, y - 1] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[x, y + 1].flag = true;
                                playfield16.visit[x, y + 1] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[x - 1, y + 1].flag = true;
                                playfield16.visit[x - 1, y + 1] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[x - 1, y].flag = true;
                                playfield16.visit[x - 1, y] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[x - 1, y - 1].flag = true;
                                playfield16.visit[x - 1, y - 1] = true;
                                playfield16.minesleft--;
                            }
                        }
                    }
                    if (y == 0 && x > 0 && x < 15)
                    {
                        if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite = playfield16.elements[0, 1].flagTexture;
                                playfield16.elements[x - 1, y].flag = true;
                                playfield16.visit[x - 1, y] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[x + 1, y].flag = true;
                                playfield16.visit[x + 1, y] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[x - 1, y + 1].flag = true;
                                playfield16.visit[x - 1, y + 1] = true;
                                playfield16.minesleft--;

                            }
                            if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[x, y + 1].flag = true;
                                playfield16.visit[x, y + 1] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[x + 1, y + 1].flag = true;
                                playfield16.visit[x + 1, y + 1] = true;
                                playfield16.minesleft--;
                            }
                        }

                    }
                    if (y == 15 && x > 0 && x < 15)
                    {
                        if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[x - 1, y].flag = true;
                                playfield16.visit[x - 1, y] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[x + 1, y].flag = true;
                                playfield16.visit[x + 1, y] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[x - 1, y - 1].flag = true;
                                playfield16.visit[x - 1, y - 1] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[x, y - 1].flag = true;
                                playfield16.visit[x, y - 1] = true;
                                playfield16.minesleft--;
                            }
                            if (playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                            {
                                playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite = playfield16.elements[x, y].flagTexture;
                                playfield16.elements[x + 1, y - 1].flag = true;
                                playfield16.visit[x + 1, y - 1] = true;
                                playfield16.minesleft--;
                            }
                        }
                    }

                }

            }
        }
        if (!playfield16.isFinished())
        {
            int used = 0;
            if (playfield16.s >= 2)
            {
                foreach (Element16 ele in playfield16.elements)
                {
                    if (playfield16.set == 0)
                    {
                        playfield16.set = 1;
                        print(playfield16.minecount);
                    }
                    if (ele.GetComponent<SpriteRenderer>().sprite != ele.emptyTexture && ele.GetComponent<SpriteRenderer>().sprite != ele.defaultTexture && ele.GetComponent<SpriteRenderer>().sprite != ele.flagTexture)
                    {
                        int x = (int)ele.transform.position.x;
                        int y = (int)ele.transform.position.y;
                        int nflgs = ele.Countflags();
                        int n = playfield16.adjacentMines(x, y);
                        int def = ele.countdef();
                        if (nflgs == n && def == 0)
                            continue;
                        else if (nflgs == n && def != 0)
                        {
                            used = 1;
                            if (x >= 1 && x <= 14 && y >= 1 && y <= 14)
                            {
                                if (playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x - 1, y + 1));
                                if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x, y + 1));
                                if (playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x + 1, y + 1));
                                if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x + 1, y));
                                if (playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x + 1, y - 1));
                                if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x, y - 1));
                                if (playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x - 1, y - 1));
                                if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x - 1, y));
                            }
                            else if ((x == 0 && y == 0) || (x == 0 && y == 15) || (x == 15 && y == 0) || (x == 15 && y == 15))
                            {
                                if (x == 0 && y == 0)
                                {
                                    if (playfield16.elements[0, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(0, 1));
                                    if (playfield16.elements[1, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(1, 1));
                                    if (playfield16.elements[1, 0].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(1, 0));
                                }
                                if (x == 0 && y == 15)
                                {
                                    if (playfield16.elements[1, 15].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(1, 15)); ;
                                    if (playfield16.elements[1, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(1, 14));
                                    if (playfield16.elements[0, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(0, 14));
                                }
                                if (x == 15 && y == 0)
                                {
                                    if (playfield16.elements[15, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(15, 1)); ;
                                    if (playfield16.elements[14, 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(14, 1));
                                    if (playfield16.elements[14, 0].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(14, 0));
                                }
                                if (x == 15 && y == 15)
                                {
                                    if (playfield16.elements[15, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(15, 14)); ;
                                    if (playfield16.elements[14, 14].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(14, 14));
                                    if (playfield16.elements[14, 15].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(14, 15));
                                }
                            }
                            else
                            {
                                if (x == 0 && y > 0 && y < 15)
                                {
                                    if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x, y - 1));
                                    if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x, y + 1));
                                    if (playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x + 1, y + 1));
                                    if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x + 1, y));
                                    if (playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x + 1, y - 1));

                                }
                                if (x == 15 && y > 0 && y < 15)
                                {
                                    if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x, y - 1));
                                    if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x, y + 1));
                                    if (playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x - 1, y + 1));
                                    if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x - 1, y));
                                    if (playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x - 1, y - 1));
                                }
                                if (y == 0 && x > 0 && x < 15)
                                {
                                    if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x - 1, y));
                                    if (playfield16.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x - 1, y + 1));
                                    if (playfield16.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x, y + 1));
                                    if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x + 1, y));
                                    if (playfield16.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x + 1, y + 1));
                                }
                                if (y == 15 && x > 0 && x < 15)
                                {
                                    if (playfield16.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x - 1, y));
                                    if (playfield16.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x + 1, y));
                                    if (playfield16.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x - 1, y - 1));
                                    if (playfield16.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x, y - 1));
                                    if (playfield16.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == playfield16.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x + 1, y - 1));
                                }
                            }
                        }
                    }
                }
                if (used == 0)
                {
                    playfield16.setprob();
                    long min = 99999999;
                    int minX, minY;
                    System.Random r = new System.Random();
                    minX = r.Next(playfield16.w);
                    minY = r.Next(playfield16.h);
                    ele = playfield16.elements[minX, minY];
                    foreach (Element16 elem in playfield16.elements)
                    {
                        if (elem.prob < min && elem.GetComponent<SpriteRenderer>().sprite == ele.defaultTexture)
                        {
                            used = 1;
                            minX = (int)ele.transform.position.x;
                            minY = (int)ele.transform.position.y;
                            min = elem.prob;
                        }
                    }
                    int cnt = 0;
                    foreach (Element16 elm in playfield16.elements)
                    {
                        if (elm.prob == min)
                            cnt++;
                    }
                    if (cnt == 1)
                        ele.StartCoroutine(fncall(minX, minY));
                    else
                        used = 0;
                }
                if (used == 0)
                {
                    System.Random r = new System.Random();
                again: int newx = r.Next(playfield16.w);
                    int newy = r.Next(playfield16.h);
                    if (playfield16.visit[newx, newy])
                    {
                        goto again;
                    }
                    else if (playfield16.elements[newx, newy].mine)
                        goto again;
                    else
                    {
                        ele = playfield16.elements[newx, newy];
                        ele.StartCoroutine(fncall(newx, newy));
                    }
                }
            }
        }

    }
    public bool isCovered(Element16 elem)
    {
        return ((elem.GetComponent<SpriteRenderer>().sprite != defaultTexture));// && (GetComponent<SpriteRenderer>().sprite.texture.name != "mine"));
    }
}