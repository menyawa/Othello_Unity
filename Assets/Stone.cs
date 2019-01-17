using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Stone : MonoBehaviour
{
	[SerializeField] private GameObject stone;
	private Transform gridTransform;
	
	// Use this for initialization
	void Start ()
	{
		gridTransform = this.gameObject.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ClickGrid()
	{
		Debug.Log("test");
		Vector3 gridPosition = gridTransform.position;
		Instantiate(stone, new Vector3(gridPosition.x, gridPosition.y + (float)0.1, gridPosition.z), Quaternion.identity);
	}
}
