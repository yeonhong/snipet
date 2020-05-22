using System.Collections;
using UnityEngine;

namespace Roguelike2D
{
	// todo : 이동할 수 있는 오브젝트의 부모클래스.
	public abstract class MovingObject : MonoBehaviour
	{
		public float moveTime = 0.1f;
		public LayerMask blockingLayer;
		private BoxCollider2D boxCollider;
		private Rigidbody2D rb2D;
		private float inverseMoveTime;
		private bool _isMoving;

		public IUnityService _unityService { protected get; set; }

		protected virtual void Start() {
			boxCollider = GetComponent<BoxCollider2D>();
			rb2D = GetComponent<Rigidbody2D>();
			inverseMoveTime = 1f / moveTime;
		}

		protected bool Move(int xDir, int yDir, out RaycastHit2D hit) {
			Vector2 start = transform.position;
			Vector2 end = start + new Vector2(xDir, yDir);
			boxCollider.enabled = false;
			hit = Physics2D.Linecast(start, end, blockingLayer);
			boxCollider.enabled = true;

			if (hit.transform == null && !_isMoving) {
				StartCoroutine(SmoothMovement(end));
				return true;
			}
			return false;
		}

		protected IEnumerator SmoothMovement(Vector3 end) {
			_isMoving = true;

			float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

			while (sqrRemainingDistance > Mathf.Epsilon) {
				Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end,
					inverseMoveTime * _unityService.GetDeltaTime());

				rb2D.MovePosition(newPostion);

				sqrRemainingDistance = (transform.position - end).sqrMagnitude;
				yield return null;
			}

			rb2D.MovePosition(end);
			_isMoving = false;
		}
		
		protected virtual void AttemptMove<T>(int xDir, int yDir) where T : Component {
			RaycastHit2D hit;
			bool canMove = Move(xDir, yDir, out hit);

			if (hit.transform == null) {
				return;
			}

			T hitComponent = hit.transform.GetComponent<T>();
			if (!canMove && hitComponent != null) {
				OnCantMove(hitComponent);
			}
		}

		protected abstract void OnCantMove<T>(T component) where T : Component;
	}
}
