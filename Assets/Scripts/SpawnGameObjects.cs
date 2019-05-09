using UnityEngine;
using System.Collections;

public class SpawnGameObjects : MonoBehaviour {

	public GameObject spawnPrefab;

    public Transform topLeftCorner;
    public Transform bottomRightCorner;
	public float minSecondsBetweenSpawning = 3.0f;
	public float maxSecondsBetweenSpawning = 6.0f;
	
	public Transform chaseTarget;
	
	float secondsBetweenSpawning,xMin,xMax,zMin,zMax;

	// Use this for initialization
	void Start () {
        zMin =topLeftCorner.position.z;
        zMax =bottomRightCorner.position.z;
        xMin =topLeftCorner.position.x;
        xMax =bottomRightCorner.position.y;
		secondsBetweenSpawning = Random.Range (minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
        Invoke("Spawn", secondsBetweenSpawning);
	}

	void Spawn()
	{
        float x = Random.Range(xMin, xMax);
        float z = Random.Range(zMin, zMax);
        float y = transform.position.y;
        Vector3 clonePos = new Vector3(x, y, z);
		// create a new gameObject
		GameObject clone = Instantiate(spawnPrefab, clonePos, transform.rotation) as GameObject;
        clone.transform.parent = transform;
		// set chaseTarget if specified
		if ((chaseTarget != null) && (clone.gameObject.GetComponent<Chaser> () != null))
		{
			clone.gameObject.GetComponent<Chaser>().SetTarget(chaseTarget);
		}

        secondsBetweenSpawning = Random.Range(minSecondsBetweenSpawning, maxSecondsBetweenSpawning);
        Invoke("Spawn", secondsBetweenSpawning);
    }
}
