using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Roguelike2D
{
	public abstract class MovingObject : MonoBehaviour
	{
		public float moveTime = 0.1f;
		public LayerMask blockingLayer;

		public bool IsMoving { get; protected set; }
		public IUnityService _unityService { protected get; set; }

		private BoxCollider2D _boxCollider;
		private Rigidbody2D _rb2D;
		private float _inverseMoveTime;

		public event EventHandler OnMoveEnd;

		public static bool AlmostClosed(Vector2 v1, Vector2 v2) {
			return Vector2.SqrMagnitude(v1 - v2) < 0.001f;
		}

		protected virtual void Start() {
			if (_unityService == null) {
				_unityService = new UnityService();
			}

			_boxCollider = GetComponent<BoxCollider2D>();
			_rb2D = GetComponent<Rigidbody2D>();
			_inverseMoveTime = 1f / moveTime;
		}

		protected bool Move(int xDir, int yDir, out RaycastHit2D hit) {
			Vector2 start = transform.position;
			Vector2 end = start + new Vector2(xDir, yDir);
			_boxCollider.enabled = false;
			hit = Physics2D.Linecast(start, end, blockingLayer);
			_boxCollider.enabled = true;

			if (hit.transform == null && !IsMoving) {
				StartCoroutine(SmoothMovement(end));
				return true;
			}
			return false;
		}

		protected IEnumerator SmoothMovement(Vector3 end) {
			IsMoving = true;
			{
				float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
				while (sqrRemainingDistance > Mathf.Epsilon) {
					Vector3 newPostion = Vector3.MoveTowards(_rb2D.position, end,
						_inverseMoveTime * _unityService.GetDeltaTime());

					_rb2D.MovePosition(newPostion);
					sqrRemainingDistance = (transform.position - end).sqrMagnitude;
					yield return null;
				}

				_rb2D.MovePosition(end);
			}
			IsMoving = false;
			OnMoveEnd?.Invoke(this, null);
		}

		protected virtual bool AttemptMove<T>(int xDir, int yDir) where T : Component {
			RaycastHit2D hit;
			bool canMove = Move(xDir, yDir, out hit);

			if (hit.transform != null) {
				T hitComponent = hit.transform.GetComponent<T>();
				if (!canMove && hitComponent != null) {
					OnBumped(hitComponent);
				}
			}

			return canMove;
		}

		protected abstract void OnBumped<T>(T component) where T : Component;
	}
}
