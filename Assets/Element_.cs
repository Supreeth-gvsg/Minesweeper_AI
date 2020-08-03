using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
public class Element_ : MonoBehaviour
{
    public bool mine;
    public bool flag;
    public static int done = 0;
    public Sprite[] emptyTextures;
    public Sprite mineTexture, flagTexture, defaultTexture, emptyTexture;
    System.Random r = new System.Random();
    public static Element_ ele;
    public int prob;
    void Start()
    {
        flag = false;
        //Playfield_.minecount = 0;
        //Playfield_.minesleft = 0;
        if (Playfield_.minecount < 10)
            mine = UnityEngine.Random.value < 0.15;
        if (mine)
        {
            Playfield_.minecount++;
            Playfield_.minesleft++;
        }
        int q = (int)transform.position.x;
        int p = (int)transform.position.y;
        Playfield_.elements[q, p] = this;
        ele = this;
        for (int i = 0; i < Playfield_.w; i++)
            for (int j = 0; j < Playfield_.h; j++)
                Playfield_.visit[i, j] = false;
        Playfield_.s = 0;
        int x, y;//
                 // Playfield_.visit[x, y] = true;//
        while (Playfield_.s < 3)
        { //
            x = r.Next(Playfield_.w);//
            y = r.Next(Playfield_.h);//
            StartCoroutine(fncall(x, y));//
            Playfield_.s++;
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
    private IEnumerator WaitForSceneLoadlost()
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
        if (str == 0)
            StartCoroutine(WaitForSceneLoadlost());
        else
            StartCoroutine(WaitForSceneLoadwon());
    }
    public static void uncover(int x, int y)
    {
        if (Playfield_.elements[x, y].mine && Playfield_.visit[x, y] == false)
        {
            done = 1;
            Playfield_.uncoverMines();
            Playfield_.visit[x, y] = true;
            print("You Lost");
            Application.Quit();
            Playfield_.minecount = 0;
            Playfield_.minesleft = 0;
           // Playfield_.elements[x, y].load(0);

        }
        if (!Playfield_.elements[x, y].mine && Playfield_.visit[x, y] == false)
        {
            int p = x;
            int q = y;
            Playfield_.visit[x, y] = true;
            int n = Playfield_.adjacentMines(p, q);
            Playfield_.elements[p, q].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[p, q].emptyTextures[n];
            Playfield_.FFuncover(x, y, new bool[Playfield_.w, Playfield_.h]);
            markflag();
            if (Playfield_.isFinished())
            {
                //  yield return new WaitForSeconds(4);
                //StartCoroutine(WaitForSceneLoadwon());
                done = 1;
                Playfield_.minecount = 0;
                Playfield_.minesleft = 0;
                //Playfield_.elements[x, y].load(1);
                print("YOU WON");
                Application.Quit();
              }
        }
    }
    public int Countflags()
    {
        int x = (int)this.transform.position.x;
        int y = (int)this.transform.position.y;
        int count = 0;
        if (x >= 1 && x <= 7 && y >= 1 && y <= 7)
        {
            if (Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                count++;
            if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                count++;
            if (Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                count++;
            if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                count++;
            if (Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                count++;
            if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                count++;
            if (Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                count++;
            if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                count++;
        }
        else if ((x == 0 && y == 0) || (x == 0 && y == 8) || (x == 8 && y == 0) || (x == 8 && y == 8))
        {
            if (x == 0 && y == 0)
            {
                if (Playfield_.elements[0, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[1, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[1, 0].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
            }
            if (x == 0 && y == 8)
            {
                if (Playfield_.elements[1, 8].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[1, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[0, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
            }
            if (x == 8 && y == 0)
            {
                if (Playfield_.elements[8, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[7, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[7, 0].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
            }
            if (x == 8 && y == 8)
            {
                if (Playfield_.elements[8, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[7, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[7, 8].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
            }

        }
        else
        {
            if (x == 0 && y > 0 && y < 8)
            {
                if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
            }
            if (x == 8 && y > 0 && y < 8)
            {
                if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
            }
            if (y == 0 && x > 0 && x < 8)
            {
                if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
            }
            if (y == 8 && x > 0 && x < 8)
            {
                if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
                    count++;
                if (Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].flagTexture)
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
        if (x >= 1 && x <= 7 && y >= 1 && y <= 7)
        {
            if (Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                count++;
            if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                count++;
            if (Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                count++;
            if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                count++;
            if (Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                count++;
            if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                count++;
            if (Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                count++;
            if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                count++;
        }
        else if ((x == 0 && y == 0) || (x == 0 && y == 8) || (x == 8 && y == 0) || (x == 8 && y == 8))
        {
            if (x == 0 && y == 0)
            {
                if (Playfield_.elements[0, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[1, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[1, 0].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
            }
            if (x == 0 && y == 8)
            {
                if (Playfield_.elements[1, 8].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[1, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[0, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
            }
            if (x == 8 && y == 0)
            {
                if (Playfield_.elements[8, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[7, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[7, 0].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
            }
            if (x == 8 && y == 8)
            {
                if (Playfield_.elements[8, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[7, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[7, 8].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
            }

        }
        else
        {
            if (x == 0 && y > 0 && y < 8)
            {
                if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
            }
            if (x == 8 && y > 0 && y < 8)
            {
                if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
            }
            if (y == 0 && x > 0 && x < 8)
            {
                if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
            }
            if (y == 8 && x > 0 && x < 8)
            {
                if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
                if (Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                    count++;
            }
        }
        return count;
    }
    public static void markflag()
    {
        foreach (Element_ elem in Playfield_.elements)
        {
            int x = (int)elem.transform.position.x;
            int y = (int)elem.transform.position.y;
            if (elem.GetComponent<SpriteRenderer>().sprite == elem.emptyTexture)
            {
                Playfield_.visit[x, y] = true;

            }
            if (elem.GetComponent<SpriteRenderer>().sprite != elem.defaultTexture && elem.GetComponent<SpriteRenderer>().sprite != elem.emptyTexture && elem.GetComponent<SpriteRenderer>().sprite != elem.flagTexture)
            {
                int n = Playfield_.adjacentMines(x, y);
                int count = 0;
                Playfield_.visit[x, y] = true;
                int nfls = elem.Countflags();
                if (x >= 1 && x <= 7 && y >= 1 && y <= 7)
                {
                    if (Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        count++;
                    if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        count++;
                    if (Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        count++;
                    if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        count++;
                    if (Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        count++;
                    if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        count++;
                    if (Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        count++;
                    if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        count++;
                    if (n - nfls == count)
                    {
                        if (Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        {
                            Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                            Playfield_.elements[x - 1, y + 1].flag = true;
                            Playfield_.visit[x - 1, y + 1] = true;
                            Playfield_.minesleft--;
                        }
                        if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        {
                            Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                            Playfield_.elements[x, y + 1].flag = true;
                            Playfield_.visit[x, y + 1] = true;
                            Playfield_.minesleft--;
                        }
                        if (Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        {
                            Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                            Playfield_.elements[x + 1, y + 1].flag = true;
                            Playfield_.visit[x + 1, y + 1] = true;
                            Playfield_.minesleft--;
                        }
                        if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        {
                            Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                            Playfield_.elements[x + 1, y].flag = true;
                            Playfield_.visit[x + 1, y] = true;
                            Playfield_.minesleft--;
                        }
                        if (Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        {
                            Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                            Playfield_.elements[x + 1, y - 1].flag = true;
                            Playfield_.visit[x + 1, y - 1] = true;
                            Playfield_.minesleft--;
                        }
                        if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        {
                            Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                            Playfield_.elements[x, y - 1].flag = true;
                            Playfield_.visit[x, y - 1] = true;
                            Playfield_.minesleft--;
                        }
                        if (Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        {
                            Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                            Playfield_.elements[x - 1, y - 1].flag = true;
                            Playfield_.visit[x - 1, y - 1] = true;
                            Playfield_.minesleft--;
                        }
                        if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                        {
                            Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                            Playfield_.elements[x - 1, y].flag = true;
                            Playfield_.visit[x - 1, y] = true;
                            Playfield_.minesleft--;
                        }
                    }
                }
                else if ((x == 0 && y == 0) || (x == 0 && y == 8) || (x == 8 && y == 0) || (x == 8 && y == 8))
                {
                    count = 0;
                    if (x == 0 && y == 0)
                    {
                        if (Playfield_.elements[1, 0].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[1, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[0, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (Playfield_.elements[0, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[0, 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[0, 1].flag = true;
                                Playfield_.visit[0, 1] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[1, 0].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[1, 0].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[1, 0].flag = true;
                                Playfield_.visit[1, 0] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[1, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[1, 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[1, 1].flag = true;
                                Playfield_.visit[1, 1] = true;
                                Playfield_.minesleft--;
                            }
                        }
                    }
                    if (x == 0 && y == 8)
                    {
                        if (Playfield_.elements[1, 8].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[1, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[0, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (Playfield_.elements[1, 8].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[1, 8].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[1, 8].flag = true;
                                Playfield_.visit[1, 8] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[1, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[1, 7].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[1, 7].flag = true;
                                Playfield_.visit[1, 7] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[0, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[0, 7].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[0, 7].flag = true;
                                Playfield_.visit[0, 7] = true;
                                Playfield_.minesleft--;
                            }
                        }
                    }
                    if (x == 8 && y == 0)
                    {
                        if (Playfield_.elements[8, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[7, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[7, 0].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (Playfield_.elements[8, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[8, 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[8, 1].flag = true;
                                Playfield_.visit[8, 1] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[7, 0].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[7, 0].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[7, 0].flag = true;
                                Playfield_.visit[7, 0] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[7, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[7, 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[7, 1].flag = true;
                                Playfield_.visit[7, 1] = true;
                                Playfield_.minesleft--;
                            }
                        }
                    }
                    if (x == 8 && y == 8)
                    {
                        if (Playfield_.elements[8, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[7, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[7, 8].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (Playfield_.elements[8, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[8, 7].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[8, 7].flag = true;
                                Playfield_.minesleft--;
                                Playfield_.visit[8, 7] = true;
                            }
                            if (Playfield_.elements[7, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[7, 7].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[7, 7].flag = true;
                                Playfield_.visit[7, 7] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[7, 8].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[7, 8].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[7, 8].flag = true;
                                Playfield_.visit[7, 8] = true;
                                Playfield_.minesleft--;
                            }
                        }
                    }

                }
                else
                {
                    count = 0;
                    if (x == 0 && y > 0 && y < 8)
                    {
                        if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[0, 1].defaultTexture)
                            {
                                Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[0, 1].flagTexture;
                                Playfield_.elements[x, y - 1].flag = true;
                                Playfield_.visit[x, y - 1] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[0, 1].defaultTexture)
                            {
                                Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[0, 1].flagTexture;
                                Playfield_.elements[x, y + 1].flag = true;
                                Playfield_.visit[x, y + 1] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[0, 1].defaultTexture)
                            {
                                Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[0, 1].flagTexture;
                                Playfield_.elements[x + 1, y + 1].flag = true;
                                Playfield_.visit[x + 1, y + 1] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[0, 1].defaultTexture)
                            {
                                Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[0, 1].flagTexture;
                                Playfield_.elements[x + 1, y].flag = true;
                                Playfield_.minesleft--;
                                Playfield_.visit[x + 1, y] = true;
                            }
                            if (Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[0, 1].defaultTexture)
                            {
                                Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[0, 1].flagTexture;
                                Playfield_.elements[x + 1, y - 1].flag = true;
                                Playfield_.visit[x + 1, y - 1] = true;
                                Playfield_.minesleft--;
                            }
                        }
                    }
                    if (x == 8 && y > 0 && y < 8)
                    {
                        if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[x, y - 1].flag = true;
                                Playfield_.visit[x, y - 1] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[x, y + 1].flag = true;
                                Playfield_.visit[x, y + 1] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[x - 1, y + 1].flag = true;
                                Playfield_.visit[x - 1, y + 1] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[x - 1, y].flag = true;
                                Playfield_.visit[x - 1, y] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[x - 1, y - 1].flag = true;
                                Playfield_.visit[x - 1, y - 1] = true;
                                Playfield_.minesleft--;
                            }
                        }
                    }
                    if (y == 0 && x > 0 && x < 8)
                    {
                        if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[0, 1].flagTexture;
                                Playfield_.elements[x - 1, y].flag = true;
                                Playfield_.visit[x - 1, y] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[x + 1, y].flag = true;
                                Playfield_.visit[x + 1, y] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[x - 1, y + 1].flag = true;
                                Playfield_.visit[x - 1, y + 1] = true;
                                Playfield_.minesleft--;

                            }
                            if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[x, y + 1].flag = true;
                                Playfield_.visit[x, y + 1] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[x + 1, y + 1].flag = true;
                                Playfield_.visit[x + 1, y + 1] = true;
                                Playfield_.minesleft--;
                            }
                        }

                    }
                    if (y == 8 && x > 0 && x < 8)
                    {
                        if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            count++;
                        if (n - nfls == count)
                        {
                            if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[x - 1, y].flag = true;
                                Playfield_.visit[x - 1, y] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[x + 1, y].flag = true;
                                Playfield_.visit[x + 1, y] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[x - 1, y - 1].flag = true;
                                Playfield_.visit[x - 1, y - 1] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[x, y - 1].flag = true;
                                Playfield_.visit[x, y - 1] = true;
                                Playfield_.minesleft--;
                            }
                            if (Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                            {
                                Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite = Playfield_.elements[x, y].flagTexture;
                                Playfield_.elements[x + 1, y - 1].flag = true;
                                Playfield_.visit[x + 1, y - 1] = true;
                                Playfield_.minesleft--;
                            }
                        }
                    }

                }

            }
        }
        if (!Playfield_.isFinished())
        {
            int used = 0;
            if (Playfield_.s >= 2 && done == 0)
            {
                foreach (Element_ ele in Playfield_.elements)
                {
                    if (Playfield_.set == 0)
                    {
                        Playfield_.set = 1;
                        print(Playfield_.minecount);
                    }
                    if (ele.GetComponent<SpriteRenderer>().sprite != ele.emptyTexture && ele.GetComponent<SpriteRenderer>().sprite != ele.defaultTexture && ele.GetComponent<SpriteRenderer>().sprite != ele.flagTexture)
                    {
                        int x = (int)ele.transform.position.x;
                        int y = (int)ele.transform.position.y;
                        int nflgs = ele.Countflags();
                        int n = Playfield_.adjacentMines(x, y);
                        int def = ele.countdef();
                        if (nflgs == n && def == 0)
                            continue;
                        else if (nflgs == n && def != 0)
                        {
                            used = 1;
                            if (x >= 1 && x <= 7 && y >= 1 && y <= 7)
                            {
                                if (Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x - 1, y + 1));
                                if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x, y + 1));
                                if (Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x + 1, y + 1));
                                if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x + 1, y));
                                if (Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x + 1, y - 1));
                                if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x, y - 1));
                                if (Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x - 1, y - 1));
                                if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                    ele.StartCoroutine(fncall(x - 1, y));
                            }
                            else if ((x == 0 && y == 0) || (x == 0 && y == 8) || (x == 8 && y == 0) || (x == 8 && y == 8))
                            {
                                if (x == 0 && y == 0)
                                {
                                    if (Playfield_.elements[0, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(0, 1));
                                    if (Playfield_.elements[1, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(1, 1));
                                    if (Playfield_.elements[1, 0].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(1, 0));
                                }
                                if (x == 0 && y == 8)
                                {
                                    if (Playfield_.elements[1, 8].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(1, 8)); ;
                                    if (Playfield_.elements[1, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(1, 7));
                                    if (Playfield_.elements[0, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(0, 7));
                                }
                                if (x == 8 && y == 0)
                                {
                                    if (Playfield_.elements[8, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(8, 1)); ;
                                    if (Playfield_.elements[7, 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(7, 1));
                                    if (Playfield_.elements[7, 0].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(7, 0));
                                }
                                if (x == 8 && y == 8)
                                {
                                    if (Playfield_.elements[8, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(8, 7)); ;
                                    if (Playfield_.elements[7, 7].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(7, 7));
                                    if (Playfield_.elements[7, 8].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(7, 8));
                                }
                            }
                            else
                            {
                                if (x == 0 && y > 0 && y < 8)
                                {
                                    if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x, y - 1));
                                    if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x, y + 1));
                                    if (Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x + 1, y + 1));
                                    if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x + 1, y));
                                    if (Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x + 1, y - 1));

                                }
                                if (x == 8 && y > 0 && y < 8)
                                {
                                    if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x, y - 1));
                                    if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x, y + 1));
                                    if (Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x - 1, y + 1));
                                    if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x - 1, y));
                                    if (Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x - 1, y - 1));
                                }
                                if (y == 0 && x > 0 && x < 8)
                                {
                                    if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x - 1, y));
                                    if (Playfield_.elements[x - 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x - 1, y + 1));
                                    if (Playfield_.elements[x, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x, y + 1));
                                    if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x + 1, y));
                                    if (Playfield_.elements[x + 1, y + 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x + 1, y + 1));
                                }
                                if (y == 8 && x > 0 && x < 8)
                                {
                                    if (Playfield_.elements[x - 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x - 1, y));
                                    if (Playfield_.elements[x + 1, y].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x + 1, y));
                                    if (Playfield_.elements[x - 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x - 1, y - 1));
                                    if (Playfield_.elements[x, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x, y - 1));
                                    if (Playfield_.elements[x + 1, y - 1].GetComponent<SpriteRenderer>().sprite == Playfield_.elements[x, y].defaultTexture)
                                        ele.StartCoroutine(fncall(x + 1, y - 1));
                                }
                            }
                        }
                    }
                }
                if (used == 0)
                {
                    Playfield_.setprob();
                    long min = 99999999;
                    int minX, minY;
                    System.Random r = new System.Random();
                    minX = r.Next(Playfield_.w);
                    minY = r.Next(Playfield_.h);
                    ele = Playfield_.elements[minX, minY];
                    foreach (Element_ elem in Playfield_.elements)
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
                    foreach (Element_ elm in Playfield_.elements)
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
                again: int newx = r.Next(Playfield_.w);
                    int newy = r.Next(Playfield_.h);
                    if (Playfield_.visit[newx, newy])
                    {
                        goto again;
                    }
                    else if (Playfield_.elements[newx, newy].mine)
                        goto again;
                    else
                    {
                        ele = Playfield_.elements[newx, newy];
                        ele.StartCoroutine(fncall(newx, newy));
                    }
                }
            }
        }

    }
    public bool isCovered(Element_ elem)
    {
        return ((elem.GetComponent<SpriteRenderer>().sprite != defaultTexture));// && (GetComponent<SpriteRenderer>().sprite.texture.name != "mine"));
    }
}