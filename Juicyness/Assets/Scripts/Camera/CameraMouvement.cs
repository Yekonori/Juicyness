﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraMouvement : MonoBehaviour
{
    public Vector3 cameraBasePosition;
    [SerializeField] private Vector3 cameraBaseRotation;
    public Vector3 cameraTiltedPosition;
    [SerializeField] private Vector3 cameraTiltedRotation;

    private bool canCenterOnPlayer;
    private Vector3 basePosition;
    private Vector3 goalPosition;
    private float positionLerpTimer = 0;
    [SerializeField] private float timeForCameraTotravel = 3;

    private PostProcessLayer postProcessLayer;

    [SerializeField] private Color whenTiltedColor;

    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1152, 864, true);

        postProcessLayer = GetComponent<PostProcessLayer>();

        FeatureManager.instance.onCameraTiltedToggle += () =>
        {
            if (FeatureManager.instance.isCameraTilted)
            {
                transform.position = cameraTiltedPosition;
                basePosition = cameraTiltedPosition;
                transform.rotation = Quaternion.Euler(cameraTiltedRotation);
            }
            else
            {
                transform.position = cameraBasePosition;
                basePosition = cameraBasePosition;
                transform.rotation = Quaternion.Euler(cameraBaseRotation);
            }
        };
        FeatureManager.instance.onCameraEffectToggle += () =>
        {
            if (FeatureManager.instance.isCameraEffectsOn)
            {
                postProcessLayer.enabled = true;
            }
            else
            {
                postProcessLayer.enabled = false;
            }
        };

        FeatureManager.instance.onUIEffectsToggle += () =>
        {
            if (FeatureManager.instance.isUIEffecstOn)
            {
                GetComponent<Camera>().backgroundColor = whenTiltedColor;
            }
            else
            {
                GetComponent<Camera>().backgroundColor = Color.black;
            }
        };

        basePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (canCenterOnPlayer)
        {
            positionLerpTimer += Time.deltaTime / timeForCameraTotravel;
            transform.position = Vector3.Lerp(basePosition, goalPosition, positionLerpTimer);
            if (positionLerpTimer >= 1)
            {
                canCenterOnPlayer = false;
                GameManager.instance.ChangeState(State.WIN);
            }
        }
    }

    public void SetUpVictoryPosition()
    {
        canCenterOnPlayer = true;
        if (!FeatureManager.instance.isCameraTilted)
        {
            goalPosition = new Vector3(GameManager.instance.player.transform.position.x, GameManager.instance.player.transform.position.y, GameManager.instance.player.transform.position.z - 2);
        }
        else
        {
            goalPosition = new Vector3(GameManager.instance.player.transform.position.x, -5, GameManager.instance.player.transform.position.z - 2);
        }
    }
}
