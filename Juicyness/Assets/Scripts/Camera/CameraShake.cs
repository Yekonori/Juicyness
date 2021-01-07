using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	private Transform camTransform;

	private float currentShakeDuration = 0.2f;

	// larger value = shakes harder.
	private float shakeAmount = 0.7f;
	[SerializeField] private float decreaseFactor = 1.0f;

	private bool canShake;

	Vector3 originalPos;
	private CameraMouvement cameraMouvement;

	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent<Transform>();
		}
	}

    private void Start()
    {
		cameraMouvement = GetComponent<CameraMouvement>();
		GameManager.instance.onStateChange += () =>
		{
			if(GameManager.instance.state != State.INGAME)
            {
				canShake = false;
            }
		};
		FeatureManager.instance.onCameraTiltedToggle += () =>
		{
			if (FeatureManager.instance.isCameraTilted)
			{
				originalPos = cameraMouvement.cameraTiltedPosition;
			}
			else
			{
				originalPos = cameraMouvement.cameraBasePosition;
			}
		};

		originalPos = camTransform.localPosition;
	}

	void Update()
	{
		if (canShake)
		{
			if (currentShakeDuration > 0)
			{
				camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

				currentShakeDuration -= Time.deltaTime * decreaseFactor;
			}
			else
			{
				canShake = false;
				currentShakeDuration = 0f;
				camTransform.localPosition = originalPos;
				if (!GameManager.instance.canPlay)
				{
					GameManager.instance.ChangeState(State.LOOSE);
				}
			}
		}
	}

	public void ShakeCamera(float thisShakeDuration, float thisShakeAmount)
	{
		canShake = true;
		currentShakeDuration = thisShakeDuration;
		shakeAmount = thisShakeAmount;
	}

}
