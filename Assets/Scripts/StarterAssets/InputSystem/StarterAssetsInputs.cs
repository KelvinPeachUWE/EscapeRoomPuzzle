using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		// These can't be changed in the inspector anymore because they are changed using KeypadUI events
		//[Header("Mouse Cursor Settings")]
		bool cursorLocked = true;
		bool cursorInputForLook = true;

		// My code
		void Awake()
		{
			// Subscribe to events
			KeypadUI.onKeypadUiEnabled += OnKeypadUiEnabled;
			KeypadUI.onKeypadUiDisabled += OnKeypadUiDisabled;
		}

		void OnKeypadUiEnabled()
		{
			// Let the player move the cursor away from the center of the screen to reach a keypad button
			cursorLocked = false;
			cursorInputForLook = false;
			SetCursorState(false);

			// Prevent player moving
			GetComponent<FirstPersonController>().enabled = false;
		}

		void OnKeypadUiDisabled()
		{
			// Let the player movement script take control of the cursor
			cursorLocked = true;
			cursorInputForLook = true;
			SetCursorState(true);

			// Allow the player to move
			GetComponent<FirstPersonController>().enabled = true;
		}
		// End my custom code

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}