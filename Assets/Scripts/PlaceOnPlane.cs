﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Moves the ARSessionOrigin in such a way that it makes the given content appear to be
/// at a given location acquired via a raycast.
/// </summary>
[RequireComponent(typeof(ARSessionOrigin))]
[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    private GameObject m_spawnedObject;

    private bool isPlaced = false;

    private static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private ARSessionOrigin m_SessionOrigin;
    private ARPlaneManager m_ARPlaneManager;
    private ARRaycastManager m_RaycastManager;
    private ARPointCloudManager m_ARPointCloudManager;
    private SceneController m_sceneControllerScript;


    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_ARPointCloudManager = GetComponent<ARPointCloudManager>();

        //m_SessionOrigin.transform.localScale = new Vector3(10, 10, 10);
        //Scale();
    }

    private void StopPlaneDetection()
    {
        m_ARPointCloudManager.enabled = false;
        m_ARPlaneManager.enabled = false;

        foreach (ARPlane plane in m_ARPlaneManager.trackables)
        {
            plane.gameObject.SetActive(m_ARPlaneManager.enabled);
        }

        foreach (ARPointCloud pointCloud in m_ARPointCloudManager.trackables)
        {
            pointCloud.gameObject.SetActive(m_ARPointCloudManager.enabled);
        }
    }

    public GameObject Place(GameObject spawnObject)
    {
        if (Input.touchCount == 0 || spawnObject == null)
        {
            return null;
        }
        else
        {
            var touch = Input.GetTouch(0);

            if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon) && !isPlaced)
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = s_Hits[0].pose;

                if (m_spawnedObject == null)
                {
                    Destroy(m_spawnedObject);
                }

                m_spawnedObject = Instantiate(spawnObject, hitPose.position, Quaternion.identity);
                //m_SessionOrigin.MakeContentAppearAt(spawnedObject.transform, hitPose.position*10.0f);
                isPlaced = true;

                StopPlaneDetection();
                return m_spawnedObject;
            }
            else
            {
                // this probably fucked it up
                return null;
            }
        }
    }

    public void Scale()
    {
        m_SessionOrigin.transform.localScale = new Vector3(10, 10, 10);
    }


    // Update is called once per frame
    void Update()
    {
        //Place();
    }


}