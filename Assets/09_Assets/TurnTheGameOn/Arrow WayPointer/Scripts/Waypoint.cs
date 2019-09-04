using UnityEngine;

namespace TurnTheGameOn.ArrowWaypointer{		
	public class Waypoint : MonoBehaviour {

		public int radius;
		[HideInInspector] public WaypointController waypointController;
		[HideInInspector] public int waypointNumber;


		void OnTriggerEnter (Collider col) {
			if(col.gameObject.tag == "Player"){
				waypointController.WaypointEvent (waypointNumber);
				waypointController.ChangeTarget ();
			}
		}

		#if UNITY_EDITOR
		void OnDrawGizmosSelected(){
			//waypointController.OnDrawGizmosSelected (radius);
		}
		#endif
	}
}