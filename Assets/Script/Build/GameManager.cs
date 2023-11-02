using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//voegt automaitsch een component toe dat het game object nodig heeft. in dit geval de line renderer omdat de line render laat zien waar je de roots plaatst
[RequireComponent(typeof(LineRenderer))]
public class GameManager : MonoBehaviour
{

    [HideInInspector] public LineRenderer lineRenderer;
    [HideInInspector] public GameObject currentNode;
    public Material[] lineMaterials;
    public LayerMask rootSpawnArea;
    public LayerMask Tower;
    public LayerMask playingField;
    public LayerMask obstacle;
    public GameObject rootPrefab;
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
    [Range(0, 25)]
    private float pointsToRootLengthDivider = 10f;

    [HideInInspector] public float maxLength;

    FSM<GameManager> fsm;

    public List<GameObject> rootList = new List<GameObject>();
    public List<GameObject> buildingsList = new List<GameObject>();
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetMaxLength();

        fsm = new FSM<GameManager>();
        fsm.Initialize(this);
        fsm.AddState(new RootEmptyState(fsm));
        fsm.AddState(new RootEditState(fsm));
        fsm.AddState(new RootFightState(fsm));
        fsm.AddState(new BuildDefenses(fsm));
        fsm.AddState(new NoFunds(fsm));


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
        //dit zorgt ervoor dat de root zolang is als de distance tussen de muis en het groeipunt.
        Vector3 scale = newRoot.transform.localScale;
        scale.y = delta.magnitude / 2f;
        newRoot.transform.localScale = scale;

        return newRoot;
    }

    public GameObject PlaceBuilding(float BuildingType, Vector3 mousePos)
    {
        GameObject newBuilding = Instantiate(BuildingsList[0]);

        //offset toegevoegd omdat anders de building door de root heen spawned. dit zorgt voor problemen met de raycast.
        newBuilding.transform.position = mousePos + new Vector3(0, 1, 0);
//        print(mousePos);
        return newBuilding;
    }

}