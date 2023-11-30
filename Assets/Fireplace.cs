using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : MonoBehaviour
{
    [SerializeField] GenericLight trigger;
    [SerializeField] EndSceneController fearWall;

    private void Start()
    {
        if (fearWall != null)
            fearWall.gameObject.SetActive(false);
    }

    void Update()
    {
        if(trigger != null)
        {
            if(trigger.isOn == false)
            {
                if(fearWall != null)
                    fearWall.gameObject.SetActive(true);
            } else
            {
                if (fearWall != null)
                    fearWall.gameObject.SetActive(false);
            }
        }
    }
}
