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
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    private bool isPlaced = false;


    void Awake()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
        m_ARPlaneManager = GetComponent<ARPlaneManager>();
        m_RaycastManager = GetComponent<ARRaycastManager>();

        m_SessionOrigin.transform.localScale = new Vector3(10, 10, 10);
    }

    private void StopPlaneDetection()
    {
        m_ARPlaneManager.enabled = false;

        foreach (ARPlane plane in m_ARPlaneManager.trackables)
        {
            plane.gameObject.SetActive(m_ARPlaneManager.enabled);
        }
    }

    private void Place()
    {
        if (Input.touchCount == 0 || m_PlacedPrefab == null)
            return;

        var touch = Input.GetTouch(0);


        if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon) && !isPlaced)
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;

            

            spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, Quaternion.identity);
            //m_SessionOrigin.MakeContentAppearAt(spawnedObject.transform, hitPose.position*10.0f);
            isPlaced = true;

            StopPlaneDetection();
        }
    }


    // Update is called once per frame
    void Update()
    {
        Place();
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARSessionOrigin m_SessionOrigin;
    ARPlaneManager m_ARPlaneManager;
    ARRaycastManager m_RaycastManager;
}
