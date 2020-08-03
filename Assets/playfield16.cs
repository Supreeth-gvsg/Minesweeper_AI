using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class playfield16
{
    public static int w = 16;
    public static int h = 16;
    public static int s;
    public static int minecount = 0;
    public static int minesleft = 0;
    public static int set = 0;
    public static int totalarrangements = 0;
    public static bool[,] visit = new bool[w, h];
    //public static void start()
    //{
    //    minesleft = 10;
    //    minecount = minesleft;

    //    for (int i = 0; i < minecount; i++)
    //    {
    //        System.Random r = new System.Random();
    //        int x = r.Next(w);
    //        int y = r.Next(h);
    //        elements[x, y].mine = true;
    //    }
    //    for (int i = 0; i < playfield16.w; i++)//
    //        for (int j = 0; j < playfield16.h; j++)//
    //            playfield16.visit[i, j] = false;//


    //}

    public static Element16[,] elements = new Element16[w, h];

    public static void uncoverMines()
    {
        foreach (Element16 elem in elements)
            if (elem.mine) elem.loadTexture(0);
    }

    public static bool mineAt(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < w && y < h)
            return elements[x, y].mine;
        return false;
    }

    public static int adjacentMines(int x, int y)
    {
        int count = 0;

        if (mineAt(x, y + 1)) ++count;
        if (mineAt(x + 1, y + 1)) ++count;
        if (mineAt(x + 1, y)) ++count;
        if (mineAt(x + 1, y - 1)) ++count;
        if (mineAt(x, y - 1)) ++count;
        if (mineAt(x - 1, y - 1)) ++count;
        if (mineAt(x - 1, y)) ++count;
        if (mineAt(x - 1, y + 1)) ++count;
        return count;
    }

    public static bool isFinished()
    {
        foreach (Element16 elem in elements)
            if (elem.isCovered(elem) || elem.mine) ;
            else
                return false;
        return true;
    }

    public static void FFuncover(int x, int y, bool[,] visited)
    {
        if (x >= 0 && y >= 0 && x < w && y < h)
        {
            if (visited[x, y])
                return;
            elements[x, y].loadTexture(adjacentMines(x, y));
            if (adjacentMines(x, y) > 0)
                return;
            visited[x, y] = true;
            FFuncover(x - 1, y, visited);
            FFuncover(x + 1, y, visited);
            FFuncover(x, y - 1, visited);
            FFuncover(x, y + 1, visited);
        }
    }

    public static void setprob()
    {
        int noclues = 0;
        foreach (Element16 ele in elements)
        {
            bool noclue_element = true;
            int defs = ele.countdef();
            int i = (int)ele.transform.position.x;
            int j = (int)ele.transform.position.y;
            if (elements[i, j].GetComponent<SpriteRenderer>().sprite == ele.defaultTexture)
            {
                if (i > 0 && elements[i - 1, j].GetComponent<SpriteRenderer>().sprite != ele.defaultTexture && elements[i - 1, j].GetComponent<SpriteRenderer>().sprite != ele.flagTexture)
                    noclue_element = false;
                if (i < w - 1 && elements[i + 1, j].GetComponent<SpriteRenderer>().sprite != ele.defaultTexture && elements[i + 1, j].GetComponent<SpriteRenderer>().sprite != ele.flagTexture)
                    noclue_element = false;
                if (i > 0 && j > 0 && elements[i - 1, j - 1].GetComponent<SpriteRenderer>().sprite != ele.defaultTexture && elements[i - 1, j - 1].GetComponent<SpriteRenderer>().sprite != ele.flagTexture)
                    noclue_element = false;
                if (i < w - 1 && j > 0 && elements[i + 1, j - 1].GetComponent<SpriteRenderer>().sprite != ele.defaultTexture && elements[i + 1, j - 1].GetComponent<SpriteRenderer>().sprite != ele.flagTexture)
                    noclue_element = false;
                if (i > 0 && j < h - 1 && elements[i - 1, j + 1].GetComponent<SpriteRenderer>().sprite != ele.defaultTexture && elements[i - 1, j + 1].GetComponent<SpriteRenderer>().sprite != ele.flagTexture)
                    noclue_element = false;
                if (i < w - 1 && j < h - 1 && elements[i + 1, j + 1].GetComponent<SpriteRenderer>().sprite != ele.defaultTexture && elements[i + 1, j + 1].GetComponent<SpriteRenderer>().sprite != ele.flagTexture)
                    noclue_element = false;
                if (j < h - 1 && elements[i, j + 1].GetComponent<SpriteRenderer>().sprite != ele.defaultTexture && elements[i, j + 1].GetComponent<SpriteRenderer>().sprite != ele.flagTexture)
                    noclue_element = false;
                if (j > 0 && elements[i, j - 1].GetComponent<SpriteRenderer>().sprite != ele.defaultTexture && elements[i, j - 1].GetComponent<SpriteRenderer>().sprite != ele.flagTexture)
                    noclue_element = false;
                if (noclue_element)
                    noclues++;
            }
        }
        List<List<Element16>> sections = new List<List<Element16>>();
        foreach (Element16 ele in elements)
        {
            bool alreadythere = false;
            int j = sections.Count;
            if (adjacentMines((int)ele.transform.position.x, (int)ele.transform.position.y) - elements[(int)ele.transform.position.x, (int)ele.transform.position.y].Countflags() != 0 && ele.GetComponent<SpriteRenderer>().sprite != ele.defaultTexture && ele.GetComponent<SpriteRenderer>().sprite != ele.flagTexture && ele.GetComponent<SpriteRenderer>().sprite != ele.emptyTexture)
            {
                for (int i = 0; i < j; i++)
                    if (sections[i].Contains(ele))
                        alreadythere = true;
                if (!alreadythere)
                {
                    List<Element16> newsection = new List<Element16>();
                    addthisele(newsection, ele);
                    sections.Add(newsection);
                }
            }
        }
        List<List<List<Element16>>> secsols = new List<List<List<Element16>>>();
        for (int k = 0; k < sections.Count; k++)
        {
            secsols.Add(getsols(sections[k]));
        }
        int[] section_selection = new int[sections.Count];
        List<List<Element16>> AllPossibleBombArrangements = new List<List<Element16>>();
        bool done = false;
        while (!done)
        {
            List<Element16> temp = new List<Element16>();
            for (int i = 0; i < sections.Count; i++)
            {
                List<List<Element16>> templist = secsols[i];
                for (int j = 0; j < templist[section_selection[i]].Count; j++)
                {
                    int x = section_selection[i];
                    List<Element16> t = templist[x];
                    temp.Add(t[j]);
                }
            }
            if (temp.Count <= minesleft)
                AllPossibleBombArrangements.Add(temp);
            for (int i = sections.Count - 1; i >= 0; i--)
            {
                section_selection[i] += 1;
                if (section_selection[i] >= secsols[i].Count)
                {
                    if (i == 0)
                    {
                        done = true;
                        break;
                    }
                    section_selection[i] = 0;
                }
                else
                {
                    break;
                }
            }
        }
        foreach (Element16 ele in elements)
            ele.prob = 0;
        totalarrangements = 0;
        for (int i = 0; i < AllPossibleBombArrangements.Count; i++)
        {
            int finalbombsleft = minesleft - AllPossibleBombArrangements[i].Count;
            int combinations = ncr(noclues, finalbombsleft);
            totalarrangements = totalarrangements + combinations;
            for (int j = 0; j < AllPossibleBombArrangements[i].Count; j++)
            {
                List<Element16> temp = AllPossibleBombArrangements[i];
                temp[j].prob = temp[j].prob + combinations;
            }
        }
    }

    public static int fact(int r)
    {
        if (r <= 1)
            return 1;
        return r * fact(r - 1);
    }

    public static int ncr(int n, int r)
    {
        if (r == 0)
        {
            return 1;
        }
        int result = 1;
        for (int i = 0; i < r; i++)
        {

            result = result * (n - i);
        }
        result = result / (fact(r));
        return result;
    }

    public static List<List<Element16>> getsols(List<Element16> section)
    {
        List<Element16> allhiddens = new List<Element16>();
        for (int i = 0; i < section.Count; i++)
        {
            List<Element16> hiddens = Hidden_ones(section[i]);
            for (int j = 0; j < hiddens.Count; j++)
            {
                if (!allhiddens.Contains(hiddens[j]))
                    allhiddens.Add(hiddens[j]);
            }
        }

        List<List<Element16>> sols = new List<List<Element16>>();
        List<List<int>> big = new List<List<int>>();
        List<Element16> checking = new List<Element16>();
        List<int> checkingInt = new List<int>();
        List<int> tempList = new List<int>();

        for (int i = 0; i < allhiddens.Count; i++)
        {
            checkingInt = new List<int>();
            checkingInt.Add(i);
            big.Add(checkingInt);
        }
        while (true)
        {
            if (big.Count != 0)
            {
                checkingInt = big[0];
                big.RemoveAt(0);
            }
            else
                break;
            checking = new List<Element16>();
            for (int i = 0; i < checkingInt.Count; i++)
            {
                checking.Add(allhiddens[checkingInt[i]]);
            }
            if (solution(checking, section))
            {
                sols.Add(checking);
            }
            else
            {
                if (!violated(checking, checkingInt, allhiddens, section))
                {
                    for (int i = checkingInt[checkingInt.Count - 1] + 1; i < allhiddens.Count; i++)
                    {
                        tempList = new List<int>(checkingInt);
                        tempList.Add(i);
                        big.Add(tempList);
                    }

                }
            }
        }
        return sols;
    }

    public static bool solution(List<Element16> checking, List<Element16> section)
    {
        for (int i = 0; i < section.Count; i++)
        {
            int count = 0;
            List<Element16> hidnear = Hidden_ones(section[i]);
            for (int j = 0; j < hidnear.Count; j++)
            {
                if (checking.Contains(hidnear[j]))
                {
                    count++;
                }
            }
            if (count != adjacentMines((int)section[i].transform.position.x, (int)section[i].transform.position.y) - elements[(int)section[i].transform.position.x, (int)section[i].transform.position.y].Countflags())
            {
                return false;
            }
        }
        return true;
    }

    public static bool violated(List<Element16> checking, List<int> checkingInt, List<Element16> hiddens, List<Element16> section)
    {
        for (int i = 0; i < section.Count; i++)
        {
            int count = 0;
            int missed = 0;
            List<Element16> hidden_near = Hidden_ones(section[i]);
            for (int j = 0; j < hidden_near.Count; j++)
            {
                if (checking.Contains(hidden_near[j]))
                {
                    count++;
                }
                else
                {
                    if (indexOf(hiddens, hidden_near[j]) < checkingInt[checkingInt.Count - 1])
                    {
                        //if the tile has been missed then count it as missed
                        missed++;
                    }
                }
            }
            if (count > adjacentMines((int)section[i].transform.position.x, (int)section[i].transform.position.y) - elements[(int)section[i].transform.position.x, (int)section[i].transform.position.y].Countflags())
            {
                return true;
            }
            if (adjacentMines((int)section[i].transform.position.x, (int)section[i].transform.position.y) - elements[(int)section[i].transform.position.x, (int)section[i].transform.position.y].Countflags() > Hidden_ones(section[i]).Count - missed)
            {
                return true;
            }
        }

        return false;//still good
    }

    public static int indexOf(List<Element16> hiddens, Element16 ele)
    {
        for (int i = 0; i < hiddens.Count; i++)
        {
            if ((int)hiddens[i].transform.position.x == ele.transform.position.x)
            {
                if ((int)hiddens[i].transform.position.y == ele.transform.position.y)
                    return i;
            }
        }
        return -1;
    }

    public static void addthisele(List<Element16> section, Element16 elem)
    {
        section.Add(elem);
        foreach (Element16 ele in playfield16.elements)
        {
            if (!section.Contains(ele) && ele.GetComponent<SpriteRenderer>().sprite != ele.defaultTexture && ele.GetComponent<SpriteRenderer>().sprite != ele.flagTexture && ele.GetComponent<SpriteRenderer>().sprite != ele.emptyTexture)
            {
                List<Element16> hidd = Hidden_ones(ele);
                List<Element16> actual_hidd = Hidden_ones(elem);
                for (int k = 0; k < hidd.Count; k++)
                {
                    if (actual_hidd.Contains(hidd[k]))
                    {
                        addthisele(section, ele);
                        break;
                    }
                }
            }
        }
    }
    public static List<Element16> Hidden_ones(Element16 ele)
    {
        List<Element16> list = new List<Element16>();
        int i = (int)ele.transform.position.x;
        int j = (int)ele.transform.position.y;
        if (i > 0 && elements[i - 1, j].GetComponent<SpriteRenderer>().sprite == ele.defaultTexture)
            list.Add(elements[i - 1, j]);
        if (i < w - 1 && elements[i + 1, j].GetComponent<SpriteRenderer>().sprite == ele.defaultTexture)
            list.Add(elements[i + 1, j]);
        if (i > 0 && j > 0 && elements[i - 1, j - 1].GetComponent<SpriteRenderer>().sprite == ele.defaultTexture)
            list.Add(elements[i - 1, j - 1]);
        if (i < w - 1 && j > 0 && elements[i + 1, j - 1].GetComponent<SpriteRenderer>().sprite == ele.defaultTexture)
            list.Add(elements[i + 1, j - 1]);
        if (i > 0 && j < h - 1 && elements[i - 1, j + 1].GetComponent<SpriteRenderer>().sprite == ele.defaultTexture)
            list.Add(elements[i - 1, j + 1]);
        if (i < w - 1 && j < h - 1 && elements[i + 1, j + 1].GetComponent<SpriteRenderer>().sprite == ele.defaultTexture)
            list.Add(elements[i + 1, j + 1]);
        if (j < h - 1 && elements[i, j + 1].GetComponent<SpriteRenderer>().sprite == ele.defaultTexture)
            list.Add(elements[i, j + 1]);
        if (j > 0 && elements[i, j - 1].GetComponent<SpriteRenderer>().sprite == ele.defaultTexture)
            list.Add(elements[i, j - 1]);
        return list;
    }
}

