using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class RootEmptyState : State<GameManager>
{
    static public bool SwitchState = false;
    LineRenderer lineRenderer;
    public RootEmptyState(FSM<GameManager> _owner, GameManager _manager) : base(_manager, _owner)
    {
        lineRenderer = Manager.lineRenderer;
    }

    public override void OnEnter()
    {
        Manager.lineRenderer.positionCount = 0;
        // Debug.Log("emptystate");
        if (PlayerController.Instance == null)
        {
            //do nothing
        }
        else if (PlayerController.Instance.currency == 0)
        {
            fSM.SwitchState(typeof(NoFunds));
        }
    }

    public override void OnUpdate()
    {

        base.OnUpdate();

        if (SwitchState == true)
        {
            SwitchState = false;
        }

        // if (!DayCycle.Instance.isNight)
        // {
        //     owner.SwitchState(typeof(RootFightState));
        // }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        //op de roots zitten belzier curves die onder de rootspawnarea layermask vallen. bij deze raycast wordt gecheckts of de muis op die layer mask zit en zo ja dan kan je een
        // root spawnen op dat oppervlak
        if (Physics.Raycast(ray, out hit, 100f, Manager.rootSpawnArea))
        {
            //"0" staat voor primairy mouse button, 1 is je secondairy en 2 is je middle mouse button
            if (Input.GetMouseButtonDown(0))
            {
                lineRenderer.positionCount = 2;
                //dit is waar de lijn spawnt
                Vector3 spawnPoint = new Vector3(ray.GetPoint(hit.distance).x, ray.GetPoint(hit.distance).y, ray.GetPoint(hit.distance).z);
                lineRenderer.SetPosition(0, spawnPoint);
                Manager.PlacePole(spawnPoint);
                fSM.SwitchState(typeof(RootEditState));
            }

        }
    }
    static public void ChangeState()
    {
        RootEmptyState.SwitchState = true;
    }
    private void OnDestroy()
    {
        lineRenderer = null;
    }
}


public class RootEditState : State<GameManager>
{
    // protected GameManager manager;
    LineRenderer lineRenderer;

    bool canPlace, loopFinished;

    public RootEditState(FSM<GameManager> _owner, GameManager _manager) : base(_manager, _owner)
    {        
        lineRenderer = Manager.lineRenderer;
    }

    public override void OnEnter()
    {
        Debug.Log("edit");
        base.OnEnter();
    }

    public override void OnExit()
    {
        base.OnExit();
        //        PlayerController.Instance.currency -= 1;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        //logic voor mouse input
        if (Physics.Raycast(ray, out hit, 100f, Manager.playingField))
        {
            Vector3 p1 = lineRenderer.GetPosition(0);
            //Debug.Log(p1);

            Vector3 p2 = ray.GetPoint(hit.distance);
            Vector3 dir = (p2 - p1).normalized;

            Vector3 placePoint = new Vector3(ray.GetPoint(hit.distance).x, p1.y, ray.GetPoint(hit.distance).z);

            lineRenderer.sharedMaterial = Manager.lineMaterials[0];
            canPlace = true;
            Manager.magnitude = (p2 - p1).magnitude;
            if (Physics.Raycast(p1, dir, (p2 - p1).magnitude, Manager.obstacle))
            {
                lineRenderer.sharedMaterial = Manager.lineMaterials[1];
                canPlace = false;
            }
            if (Physics.Raycast(p1, dir, (p2 - p1).magnitude, Manager.FinishLoop))
            {
                //you can finish the loop here
                Debug.Log("loop finished");
                loopFinished = true;
            }
            else
            {
                loopFinished = false;
            }

            if ((p2 - p1).magnitude > Manager.maxLength)
            {

                Vector3 correctedPosition = p1 + (dir * Manager.maxLength);
                // Debug.Log("distance of lint " + (p1 - p2).magnitude);
                // Debug.Log("maxlength " + Manager.maxLength);

                lineRenderer.SetPosition(1, correctedPosition);
            }
            else
            {
                lineRenderer.SetPosition(1, placePoint);
            }
            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                GameObject root = Manager.PlaceRoot();
                Manager.pRootPoints -= (p1 - p2).magnitude;
                //root.GetComponent<Root>().StartRoot();
                Manager.rootList.Add(root);
                if (!loopFinished)
                {
                    // Debug.Log("einde van de lijn " + lineRenderer.GetPosition(1));
                    lineRenderer.SetPosition(0, lineRenderer.GetPosition(1));
                    Manager.PlacePole(lineRenderer.GetPosition(1));
                    // Debug.Log("klik positie " + placePoint);
                }
                else
                {
                    Manager.FirstPole.layer = 0;
                    Manager.FirstPole = null;
                    fSM.SwitchState(typeof(RootEmptyState));
                }
            }

        }

    }
    private void OnDestroy()
    {
        lineRenderer = null;
    }
}

public class NoFunds : State<GameManager>
{

    public NoFunds(FSM<GameManager> _owner,GameManager _manager) : base(_manager, _owner)
    {
    }
    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //maak een raycasthit var die hit heet
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, Manager.playingField))
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayerController.Instance.NoFund();
            }
        }
    }
    private void OnDestroy()
    {
    }
}