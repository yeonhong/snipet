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
		private static readonly Vector2 emptyValue = -Vector2.one;
		private Vector2 startPosition = emptyValue;
		public float sensitivity { get; set; } = 1f;

		public InputController_Mobile(IUnityService service) : base(service) { }

		public override void MoveController(out int horizontal, out int vertical) {
			horizontal = 0;
			vertical = 0;

			if (_unityService.IsMouseButtonDown()) {
				startPosition = _unityService.GetMousePosition();
			}
			else if (_unityService.IsMouseButtonUp()) {
				Vector2 endPosition = _unityService.GetMousePosition();

				if ((endPosition - startPosition).magnitude > sensitivity) {
					float x = endPosition.x - startPosition.x;
					float y = endPosition.y - startPosition.y;
					if (Mathf.Abs(x) >= Mathf.Abs(y)) {
						horizontal = x > 0 ? 1 : -1;
					}
					else {
						vertical = y > 0 ? 1 : -1;
					}
				}

				startPosition = emptyValue;
			}
		}
	}
}