using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateCanvas : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI tmText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.magnitude != 0)
        {
            // Debug.Log("we're moving");
            var Text = Mathf.Floor(gameManager.pRootPoints).ToString() + " - " + Mathf.Floor(gameManager.magnitude);
            tmText.text = Text;

        }
        else
        {
            //Debug.Log("we're not moving");
            tmText.text = Mathf.Floor(gameManager.pRootPoints).ToString();
        }
    }
}
