using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
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
            goalPosition = new Vector3(GameManager.instance.player.transform.position.x, cameraTiltedPosition.y, GameManager.instance.player.transform.position.z - 2);
        }
    }
}
