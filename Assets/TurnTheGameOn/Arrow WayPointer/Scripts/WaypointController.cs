﻿using UnityEngine;
using UnityEngine.Events;

namespace TurnTheGameOn.ArrowWaypointer{
	[ExecuteInEditMode]
	public class WaypointController : MonoBehaviour {

		public enum Switch { Off, On }

		[System.Serializable]
		public class WaypointComponents
		{
			public string waypointName = "Waypoint Name";
			public Waypoint waypoint;
			public UnityEvent waypointEvent;
		}


		public Transform player;
		//[Range(0,5)]public float arrowRotationSpeed;
		public Switch configureMode;
		//Float used to determine how fast the arrow should smoothly target the next waypoint
		[Range(0.0001f,20)]public float arrowTargetSmooth;
		//Int used to determine how many Waypoints should be used
		[Range(1,100)]public int TotalWaypoints;
		public WaypointComponents[] waypointList;
		private GameObject newWaypoint;
		private string newWaypointName;
		private int nextWP;
		//Transform used to reference the Waypoint Arrow
		private Transform waypointArrow;
		//Transforms used to identify the Waypoint Arrow's target
		private Transform currentWaypoint;
		private Transform arrowTarget;
        int _rnd_nm;
		void Start () {
			if(Application.isPlaying){
				GameObject newObject = new GameObject();
				newObject.name = "Arrow Target";
				newObject.transform.parent = gameObject.transform;
				arrowTarget = newObject.transform;
				newObject = null;
                _rnd_nm = PlayerControl._uniqueInstance.RNDNUM;
			}
			nextWP = _rnd_nm;
			ChangeTarget ();
		}

		void Update () {
			if (configureMode == Switch.Off) {
				TotalWaypoints = waypointList.Length;
			}
			//Check if the script is being executed in the Unity Editor
			#if UNITY_EDITOR
			if (configureMode == Switch.On) {
				CalculateWaypoints ();
			}
			#endif
			//Keep the Waypoint Arrow pointed at the Current Waypoint
			if (arrowTarget != null) {
				arrowTarget.localPosition = Vector3.Lerp (arrowTarget.localPosition, currentWaypoint.localPosition, arrowTargetSmooth * Time.deltaTime);
				arrowTarget.localRotation = Quaternion.Lerp (arrowTarget.localRotation, currentWaypoint.localRotation, arrowTargetSmooth * Time.deltaTime);
			} else {
				arrowTarget = currentWaypoint;
			}
			if (waypointArrow == null)
				FindArrow ();
			waypointArrow.LookAt(arrowTarget);
		}

		public void WaypointEvent(int waypointEvent){
			waypointList [waypointEvent - 1].waypointEvent.Invoke ();
		}

		public void ChangeTarget(){
			int check = nextWP;
			if (check < TotalWaypoints) {
				if (currentWaypoint == null)
					currentWaypoint = waypointList [0].waypoint.transform;
				currentWaypoint.gameObject.SetActive (false);
				currentWaypoint = waypointList [nextWP].waypoint.transform;
				currentWaypoint.gameObject.SetActive (true);
				nextWP += 1;
			}
			if (check == TotalWaypoints) {
				Destroy (waypointArrow.gameObject);
				Destroy (gameObject);
			}
		}

		public void CreateArrow(){
			GameObject instance = Instantiate(Resources.Load("Waypoint Arrow", typeof(GameObject))) as GameObject;
			instance.name = "Waypoint Arrow";
			instance = null;
		}

		public void FindArrow(){
			GameObject arrow = GameObject.Find ("Waypoint Arrow");
			if (arrow == null) {
				CreateArrow ();
				waypointArrow = GameObject.Find ("Waypoint Arrow").transform;
			}else{
				waypointArrow = arrow.transform;
			}
		}

		public void CalculateWaypoints(){
			if (configureMode == Switch.On) {
				System.Array.Resize (ref waypointList, TotalWaypoints);
				if (waypointArrow == null)	FindArrow ();
				for (var i = 0; i < TotalWaypoints; i++) {
					if (waypointList [i] != null && waypointList[i].waypoint == null) {
						newWaypointName = "Waypoint " + (i + 1);
						waypointList [i].waypointName = newWaypointName;
						//setup waypoint reference
						foreach (Transform child in transform) {
							if (child.name == newWaypointName) {		waypointList[i].waypoint = child.GetComponent<Waypoint> ();			}
						}
						if (waypointList [i].waypoint == null) {
							newWaypoint = Instantiate (Resources.Load ("Waypoint")) as GameObject;
							newWaypoint.name = newWaypointName;
							newWaypoint.GetComponent<Waypoint> ().waypointNumber = i + 1;
							newWaypoint.transform.parent = gameObject.transform;
							waypointList[i].waypoint = newWaypoint.GetComponent<Waypoint> ();
							waypointList [i].waypoint.waypointController = this;
							Debug.Log ("Waypoint Controller created a new Waypoint: " + newWaypointName);
						}
						currentWaypoint = waypointList [0].waypoint.transform;
					}
				}
				CleanUpWaypoints ();
			}
		}
		
		public void CleanUpWaypoints(){
			if (configureMode == Switch.On) {
				if(Application.isPlaying){
					Debug.LogWarning ("ARROW WAYPOINTER: Turn Off 'Configure Mode' on the Waypoint Controller");
				}
				if (transform.childCount > waypointList.Length) {
					foreach (Transform oldChild in transform) {
						if (oldChild.GetComponent<Waypoint> ().waypointNumber  > waypointList.Length) {
							DestroyImmediate (oldChild.gameObject);
						}
					}
				}
			}
		}

		#if UNITY_EDITOR
		//Draws a Gizmo in the scene view window to show the Waypoints
		public void OnDrawGizmosSelected(int radius) {
			for(var i = 0; i < waypointList.Length; i++){
				if (waypointList [i] != null) {
					if (waypointList [i].waypoint != null) {
						//Gizmos.DrawWireSphere (waypointList [i].waypoint.transform.position, waypointList [i].waypoint.radius);
					}
				}
			}
		}
		#endif

	}
}