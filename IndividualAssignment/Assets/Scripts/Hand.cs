using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ViveController))]
public class Hand : MonoBehaviour {

    public float grabOffset = 0.1f;

    GameObject grabbedObject;
    ViveController controller;

    Rigidbody simulator;

	// Use this for initialization
	void Start () {
        controller = GetComponent<ViveController>();

        simulator = new GameObject().AddComponent<Rigidbody>();
        simulator.name = "Simulator";
        simulator.transform.parent = transform.parent;
	}
	
	// Update is called once per frame
	void Update () {
		if (grabbedObject)
        {
            simulator.velocity = (transform.position - simulator.position) * 50f;
            if (controller.Controller.GetPressUp(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
            {
                grabbedObject.transform.parent = null;
                grabbedObject.GetComponent<HeldObject>().parent = null;

                Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.isKinematic = false;
                    rb.velocity = simulator.velocity;
                }

                grabbedObject = null;
            }
        }
        else
        {
            if (controller.Controller.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger))
            {
                Collider[] cols = Physics.OverlapSphere(transform.position, grabOffset);

                foreach (Collider col in cols)
                {
                    HeldObject heldObject = col.GetComponent<HeldObject>();
                    if (heldObject && heldObject.parent == null)
                    {
                        grabbedObject = col.gameObject;
                        grabbedObject.transform.parent = transform;
                        grabbedObject.transform.localPosition = Vector3.zero;
                        grabbedObject.transform.localRotation = Quaternion.identity;

                        Rigidbody rb = grabbedObject.GetComponent<Rigidbody>();
                        if (rb) rb.isKinematic = true;

                        heldObject.parent = controller;
                    }
                }
            }
        }
	}
}
