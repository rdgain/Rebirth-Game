using System.Text;
using UnityEngine;
using System.Collections;

public class LightCheck : MonoBehaviour {
	public GameObject previousMirror = null;
	public GameObject nextMirror = null;
	public double angleVariance = -1;
	
	private double minAngle, maxAngle, normal;
	private double defaultAngleVariance = 5;

	void Start()
	{
		// if not setup by map engine.
		if (nextMirror != null || previousMirror != null)
			setup ();
	}

	public void setup()
	{
		if (angleVariance < 0) angleVariance = defaultAngleVariance;

		if (nextMirror == null)
		{
			// if this object is the target/light sink.
			transform.rotation = getRotation(transform.position, previousMirror.transform.position);

			minAngle = transform.rotation.eulerAngles.z - angleVariance;
			maxAngle = transform.rotation.eulerAngles.z + angleVariance;
		}
		else if (previousMirror == null)
		{
			// if this object is the light source.
			transform.rotation = getRotation(transform.position, nextMirror.transform.position);

			minAngle = transform.rotation.eulerAngles.z - angleVariance;
			maxAngle = transform.rotation.eulerAngles.z + angleVariance;
		}
		else
		{
			// if this object is not the light source or sink.
			Vector2 toPrev = previousMirror.transform.position - transform.position;
			Vector2 toNext = nextMirror.transform.position - transform.position;
			double angle = Vector2.Angle(toPrev, toNext);
			double angleToPrev = (getVectorAngle(toPrev) + 360) % 360;
			double angleToNext = (getVectorAngle(toNext) + 360) % 360;
			if (angleToPrev <= angleToNext) normal = angleToPrev + angle / 2 - 90;
			else normal = angleToPrev - angle / 2 - 90;

			minAngle = normal - angleVariance;
			maxAngle = normal + angleVariance;
			if (minAngle < 0) minAngle += 360;
			if (maxAngle < 0) maxAngle += 360;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	/*
	 * check that this mirror is within tolerance.
	 */
	public bool inTolerance()
	{
		if(normal >= angleVariance && normal <= 360 - angleVariance)
			return transform.rotation.eulerAngles.z >= minAngle && transform.rotation.eulerAngles.z <= maxAngle &&
					transform.rotation.eulerAngles.z >= 0 && transform.rotation.eulerAngles.z <= 360;
		else
			return transform.rotation.eulerAngles.z >= minAngle || transform.rotation.eulerAngles.z <= maxAngle &&
				transform.rotation.eulerAngles.z >= 0 && transform.rotation.eulerAngles.z <= 360;
	}

	/*
	 * check that this and all prier mirrors are within tolerance angle to reflect light from source.
	 */
	public bool inToleranceFromSource() {
		if (previousMirror != null)
			return inTolerance () && previousMirror.GetComponent<LightCheck> ().inToleranceFromSource();
		else return inTolerance();
	}

	private float getAngle(Vector2 sourcePos, Vector2 targetPos) {
		Vector2 dir = targetPos - sourcePos;
		return getVectorAngle(dir);
	}

	private Quaternion getRotation(Vector2 sourcePos, Vector2 targetPos) {
		return Quaternion.Euler (0f, 0f, getAngle (sourcePos, targetPos) - 90);
	}

	private float getVectorAngle(Vector2 target) { return Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg; }
}
