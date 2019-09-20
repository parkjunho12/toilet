using UnityEngine;
using System.Collections;

namespace TurnTheGameOn.NPCChat {
	public class SmoothFollow : MonoBehaviour {

		// The target we are following
		[SerializeField]
		private Transform target;
		// The distance in the x-z plane to the target
		[SerializeField]
		private float distance = 10.0f;
		// the height we want the camera to be above the target
		[SerializeField]
		private float height = 5.0f;

		[SerializeField]
		private float rotationDamping;
		[SerializeField]
		private float heightDamping;

		// Update is called once per frame
		void LateUpdate()
		{
            if(LobbyManager._uniqueInstance.NOWGAMESTATE == LobbyManager.eGameState.STARTFIND)
            {
			    // Early out if we don't have a target
			    if (!target)
				    return;

			    // Calculate the current rotation angles
			    var wantedRotationAngle = target.eulerAngles.y;
			    //var wantedHeight = target.position.y ;

			    var currentRotationAngle = transform.eulerAngles.y;
			    //var currentHeight = transform.position.y;

			    // Damp the rotation around the y-axis
			    currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

			    // Damp the height
			    //currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

			    // Convert the angle into a rotation
			    var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

			    // Set the position of the camera on the x-z plane to:
			    // distance meters behind the target
			    transform.position = target.position;
			    transform.position += currentRotation * Vector3.forward * distance;

			    // Set the height of the camera
			    transform.position = new Vector3(transform.position.x - 1.0f , transform.position.y, transform.position.z);

            }

		}

	}
}