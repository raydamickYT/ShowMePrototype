using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//voegt automatisch een component toe dat het game object nodig heeft. in dit geval de line renderer omdat de line render laat zien waar je de roots plaatst
[RequireComponent(typeof(LineRenderer))]
public class GameManager : MonoBehaviour
{

    [HideInInspector] public LineRenderer lineRenderer;
    [HideInInspector] public GameObject FirstPole;
    public Material[] lineMaterials;
    public LayerMask rootSpawnArea;
    public float magnitude;
    public LayerMask playingField;
    public LayerMask obstacle;
    public LayerMask FinishLoop;
    public GameObject rootPrefab, Pole;
    public GameObject[] BuildingsList;

    public float pRootPoints
    {
        get
        {
            return rootPoints;
        }

        set
        {
            rootPoints = value;

            SetMaxLength();
        }
    }

    [SerializeField]
    private float rootPoints = 50f;

    [SerializeField]
    [Range(0, 2)]
    private float pointsToRootLengthDivider = 1f;

    [HideInInspector] public float maxLength;

    FSM<GameManager> fsm;

    public List<GameObject> rootList = new List<GameObject>();
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetMaxLength();

        fsm = new FSM<GameManager>();
        fsm.AddState(new RootEmptyState(fsm, this));
        fsm.AddState(new RootEditState(fsm, this));
        fsm.AddState(new NoFunds(fsm, this));


        fsm.SwitchState(typeof(RootEmptyState));
    }

    void Update()
    {
        fsm.Update();
    }

    private void SetMaxLength()
    {
        maxLength = Mathf.Floor(rootPoints / pointsToRootLengthDivider);
    }

    public GameObject PlaceRoot()
    {
        GameObject newRoot = Instantiate(rootPrefab);
        Vector3 p1 = lineRenderer.GetPosition(0);
        Vector3 p2 = lineRenderer.GetPosition(1);
        Vector3 delta = p2 - p1;
        newRoot.transform.rotation = Quaternion.LookRotation(delta);
        newRoot.transform.Rotate(-90, 0, 0);
        newRoot.transform.position = (p1 + p2) / 2f;
        newRoot.transform.position += new Vector3(0, 0.3f, 0);

        //dit zorgt ervoor dat de root zolang is als de distance tussen de muis en het groeipunt.
        Vector3 scale = newRoot.transform.localScale;
        scale.y = delta.magnitude;
        newRoot.transform.localScale = scale;

        return newRoot;
    }
    public GameObject PlacePole(Vector3 SpawnPos)
    {
        GameObject newPole = Instantiate(Pole);
        newPole.transform.position = SpawnPos + new Vector3(0, 0.2f, 0);
        if (FirstPole == null)
        {
            FirstPole = newPole;
            int layerIndex = (int)Mathf.Log(FinishLoop.value, 2);
            FirstPole.layer = layerIndex;
            return FirstPole;
        }
        return newPole;
    }
    
}