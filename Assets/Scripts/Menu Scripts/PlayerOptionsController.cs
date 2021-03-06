﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlayerOptionsController : MonoBehaviour
{
    [SerializeField]
    private GameObject _textureOption;

    [SerializeField]
    private GameObject _trailOption;

    [SerializeField]
    private GameObject _hatOption;

    private SceneController _sceneControllerScript;

    private CycleOptionController _textureOptionController;
    private CycleOptionController _trailOptionController;
    private CycleOptionController _hatOptionController;

    // Start is called before the first frame update
    void Start()
    {
        //Gain access to the scene controller
        GameObject[] objects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        foreach(GameObject obj in objects)
        {
            SceneController ctrl = obj.GetComponentInChildren<SceneController>();

            if(ctrl != null)
            {
                _sceneControllerScript = ctrl;
                break;
            }
        }

       

        //_sceneControllerScript = GetComponentInParent<SceneController>();
        _textureOptionController = _textureOption.GetComponent<CycleOptionController>();
        _trailOptionController = _trailOption.GetComponent<CycleOptionController>();
        _hatOptionController = _hatOption.GetComponent<CycleOptionController>();
    }

    public void UpdateOptions()
    {
        int textureIndex = _textureOptionController.GetCurrentIndex();
        _sceneControllerScript.SetMarbleTexture(textureIndex);

        int trailIndex = _trailOptionController.GetCurrentIndex();
        _sceneControllerScript.SetMarbleTrail(trailIndex);

        int hatIndex = _hatOptionController.GetCurrentIndex();
        _sceneControllerScript.SetMarbleHat(hatIndex);
    }

    public void OptionsConfirmed()
    {
        _sceneControllerScript.MenuClosed();
    }
}