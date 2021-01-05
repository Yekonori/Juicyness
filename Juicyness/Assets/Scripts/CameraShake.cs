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

	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent<Transform>();
		}
	}

    private void Start()
    {
		GameManager.instance.onStateChange += () =>
		{
			if(GameManager.instance.state != State.INGAME)
            {
				canShake = false;
            }
		};
    }

    void OnEnable()
	{
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
