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
		private Vector2 touchOrigin = -Vector2.one;

		public InputController_Mobile(IUnityService service) : base(service) { }

		public override void MoveController(out int horizontal, out int vertical) {
			horizontal = 0;
			vertical = 0;

			if (Input.touchCount > 0) {
				Touch myTouch = Input.touches[0];

				if (myTouch.phase == TouchPhase.Began) {
					touchOrigin = myTouch.position;
				}
				else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0) {
					Vector2 touchEnd = myTouch.position;
					float x = touchEnd.x - touchOrigin.x;
					float y = touchEnd.y - touchOrigin.y;

					touchOrigin.x = -1;
					if (Mathf.Abs(x) > Mathf.Abs(y)) {
						horizontal = x > 0 ? 1 : -1;
					}
					else {
						vertical = y > 0 ? 1 : -1;
					}
				}
			}
		}
	}
}