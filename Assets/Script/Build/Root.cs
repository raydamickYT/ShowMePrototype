using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    public List<Mesh> meshList = new List<Mesh>();
    public float growSpeed = 0.4f;

    public Material[] mats;
    public Renderer[] r;

    private void Update()
    {
        for (int i = 0; i < mats.Length; i++)
        {
            if (mats[i].GetFloat("_grow") < 1)
            {
                mats[i].SetFloat("_grow", mats[i].GetFloat("_grow") + growSpeed * Time.deltaTime);
            }
        }
    }
    public void StartRoot()
    {
        r = GetComponentsInChildren<Renderer>();
        //mats = new Material[r.Length];
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = r[i].material;
            Debug.Log(mats[i].GetFloat("_grow"));
            mats[i].SetFloat("_grow", 0);
        }
        //mats=null;
    }
    private void OnDestroy()
    {
        meshList = null;
        mats = new Material[0];
        r = new Renderer[0];
    }
}
