using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam : MonoBehaviour
{
    public GameObject player;
    private TreesFader _treefader;
    private RockFader _rockfader;

    private void Update()
    {
        // GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            //Debug.Log("CAM : PLAYER ATTIVO !!!");
            Vector3 dir = player.transform.position - transform.position;
            Ray ray = new Ray(transform.position, dir);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log("CAM : RAYCAST !!!");
                if (hit.collider == null)
                    return;

                if (hit.collider.gameObject == player)
                {
                    //Debug.Log("CAM : è un PLAYER non fare fader !!!");
                    if (_treefader != null)
                    {
                        _treefader.Dofade = false;
                    }
                    if (_rockfader != null)
                    {
                        _rockfader.Dofade = false;
                    }
                }
                else
                {
                    //Debug.Log("CAM : NON è un PLAYER ATTIVA fader !!!");
                    if (hit.collider.CompareTag("Tree"))
                    {
                        //Debug.Log("ATTIVA TREES FADER : NON è un PLAYER ATTIVA fader !!!");
                        _treefader = hit.collider.gameObject.GetComponent<TreesFader>();
                        if (_treefader != null)
                        {
                            _treefader.Dofade = true;
                        }
                    }

                    if (hit.collider.CompareTag("Rock"))
                    {
                        //Debug.Log("ATTIVA TREES FADER : NON è un PLAYER ATTIVA fader !!!");
                        _rockfader = hit.collider.gameObject.GetComponent<RockFader>();
                        if (_rockfader != null)
                        {
                            _rockfader.Dofade = true;
                        }
                    }


                }
            }

        }


    }

}
