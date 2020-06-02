using UnityEngine;

namespace Roguelike2D
{
	public abstract class InputContoller
	{
		protected IUnityService _unityService;
		public InputContoller(IUnityService unityService) {
			_unityService = unityService;
		}

		public abstract void MoveController(out int horizontal, out int vertical);
	}

	public class InputController_Standalone : InputContoller
	{
		public InputController_Standalone(IUnityService unityService) : base(unityService) { }

		public override void MoveController(out int horizontal, out int vertical) {
			horizontal = (int)(_unityService.GetAxisRaw("Horizontal"));
			vertical = (int)(_unityService.GetAxisRaw("Vertical"));
			if (horizontal != 0) {
				vertical = 0;
			}
		}
	}

	public class InputController_Mobile : InputContoller
	{
		private static readonly Vector2 EMPTY_VALE = -Vector2.one;
		private Vector2 _startPosition = EMPTY_VALE;
		public float _sensitivity { get; set; } = 1f;

		public InputController_Mobile(IUnityService service) : base(service) { }

		public override void MoveController(out int horizontal, out int vertical) {
			horizontal = 0;
			vertical = 0;

			if (_unityService.IsMouseButtonDown()) {
				_startPosition = _unityService.GetMousePosition();
			}
			else if (_unityService.IsMouseButtonUp()) {
				Vector2 endPosition = _unityService.GetMousePosition();
				float magnitude = (endPosition - _startPosition).magnitude;

				if (magnitude > _sensitivity) {
					if (Mathf.Abs(GetX(endPosition)) >= Mathf.Abs(GetY(endPosition))) {
						horizontal = GetX(endPosition) > 0 ? 1 : -1;
					}
					else {
						vertical = GetY(endPosition) > 0 ? 1 : -1;
					}
				}

				_startPosition = EMPTY_VALE;
			}
		}

		private float GetY(Vector2 endPosition) {
			return endPosition.y - _startPosition.y;
		}

		private float GetX(Vector2 endPosition) {
			return endPosition.x - _startPosition.x;
		}
	}
}