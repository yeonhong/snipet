//using Pathfinding.Util; // Code from A*Pathfinding
using com.t7t.utilities;
using UnityEngine;

namespace com.t7t.formation
{
	public class FormationAnimationTrigger : MonoBehaviour
	{
		[SerializeField] private float period = 0.8F;  // Checks every period seconds if a formation is in range
		[SerializeField] private float range = 5.0F;
		[SerializeField] private bool drawGizmos = true;

		[Header("Animation")]

		[SerializeField] private Animator animator = null;
		[SerializeField] private string animationParameter = null;
		[SerializeField] private bool newState = true;

		private float time = 0.0f;
		private Toolbox toolbox;

		private void Awake() {
			toolbox = Toolbox.Instance;
		}


		// Use this for initialization
		private void Start() {

		}

		// Update is called once per frame
		private void Update() {

			time += Time.deltaTime;
			if (time >= period) {
				time = 0.0f;

				// check if a formation is now in range

				for (int i = 0; i < toolbox.allFormations.Count; i++) {
					FormationGrid fg = toolbox.allFormations[i];

					//Debug.Log(Vector3.Distance(fg.transform.position, transform.position));

					if (Vector3.Distance(fg.transform.position, transform.position) < range) {

						animator.SetBool(animationParameter, newState);

					}
				}


			}

		}

		private void OnDrawGizmosSelected() {
			if (drawGizmos) {
				// Draw.Gizmos.CircleXZ(transform.position, range, Color.yellow);

			}
		}

	}

}
