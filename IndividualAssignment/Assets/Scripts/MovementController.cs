using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    public Transform indicator;
    public float maxDistance = 100f;


    BlinkEffect blinkEffect;
    Camera mainCamera;
    Vector3 indicatorOffset;

    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main;
        indicatorOffset = new Vector3(0, 0.1f, 0);
        blinkEffect = GetComponent<BlinkEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Walkable"))
            {
                if (!IndicatorActive())
                {
                    EnableIndicator();
                }
                MoveIndicator(hit.point);
            }
            else
            {
                if (IndicatorActive())
                {
                    DisableIndicator();
                }
            }
        }
        else
        {
            if (IndicatorActive())
            {
                DisableIndicator();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(MoveCharacter());
        }
    }

    IEnumerator MoveCharacter()
    {
        if (IndicatorActive())
        {
            StartCoroutine(blinkEffect.Blink());
            
            while (!blinkEffect.EyesClosed)
            {
                yield return null;
            }

            transform.position = indicator.position - indicatorOffset;
        }
    }

    void MoveIndicator(Vector3 pos)
    {
        indicator.position = pos + indicatorOffset;
    }

    void EnableIndicator()
    {
        indicator.gameObject.SetActive(true);
    }

    void DisableIndicator()
    {
        indicator.gameObject.SetActive(false);
    }

    bool IndicatorActive()
    {
        return indicator.gameObject.activeSelf;
    }
}
