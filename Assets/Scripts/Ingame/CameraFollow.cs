using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Vector3 start;
    private float alpha = 3f;

    public float minZoom = 7;
    public float maxZoom = 12;
    public float maxDistanceX = 12;
    public float maxDistanceY = 12;

    public float buffer = 3;

    void Start()
    {
        start = this.transform.position;
        this.GetComponent<Camera>().orthographicSize = maxZoom;
    }

    // Update is called once per frame
    void Update()
    {
        //target = Mathf.Lerp(target, getMid(), alpha * Time.deltaTime);
        Vector3 newTarget = getMid();
        Vector3 dir = newTarget - transform.position;

        this.transform.position += dir * Time.deltaTime;
        if (Math.Abs((this.transform.position - start).x) >= maxDistanceX)
        {
            Vector3 vec = (this.transform.position - start).normalized;
            vec *= maxDistanceX;
            this.transform.position = vec + start;
        }

        if (Math.Abs((this.transform.position - start).y) >= maxDistanceY)
        {
            Vector3 vec = (this.transform.position - start).normalized;
            vec *= maxDistanceY;
            this.transform.position = vec + start;
        }

        this.GetComponent<Camera>().orthographicSize = Mathf.Lerp(this.GetComponent<Camera>().orthographicSize, nextRatio(), alpha * Time.deltaTime);
        //this.GetComponent<Camera>().transform.Translate(getMid().x, getMid().y, 0);
    }

    private Vector3 getMid()
    {
        ArrayList a = new ArrayList();
        a = getPlayerLocs();
        Vector3 final = new Vector3();
        foreach(Vector3 pos in a)
        {
            final += pos;
        }
        final /= 4;
        final.z = transform.position.z;
        return final;
    }

    private ArrayList getPlayerLocs()
    {
        ArrayList a = new ArrayList();

        for (int i = 1; i < 5; i++)
        {
            GameObject geo = GameObject.Find("Player" + i);
            if (geo != null)
            {
                a.Add(geo.transform.position);
            }
        }

        return a;
    }

    public float highestY()
    {
        float longest = 0;
        foreach (Vector3 vec in getPlayerLocs())
        {
            float dist = Math.Abs(vec.y - transform.position.y);

            if (dist > longest)
                longest = dist;
        }
        return longest;
    }

    public float highestX()
    {
        float longest = 0;
        foreach (Vector3 vec in getPlayerLocs())
        {
            float dist = Math.Abs(vec.x - transform.position.x);

            if (dist > longest)
                longest = dist;
        }
        return longest;
    }

    public float nextRatio() {
        float lastY = highestY()+ buffer;
        float lastX = highestX()+buffer;

        float ratio = 0;

        //TODO: Magic
        float yRatio = 1 * lastY;
        float xRatio = 0.5678699f * lastX;

        ratio = yRatio > xRatio ? yRatio : xRatio;

        if (ratio < minZoom)
            ratio = minZoom;

        if (ratio > maxZoom)
            ratio = maxZoom;

        return ratio;
    }
}
