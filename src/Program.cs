using System;

namespace Steering {
	public class Program : Love.Scene {
		private Love.World world = null;

		private Car car = null;

		private Love.Joystick joystick = null;

		private float bound = 0.1f;
		private float safetyFactor = 0.4f;

		private Steering.JoystickSteering joystickSteering = null;

		public Program() {
			this.world = Love.Physics.NewWorld(0.0f, 0.0f, false);

			this.car = new Car(this.world);
		}

		public override void JoystickAdded(Love.Joystick joystick) {
			this.joystickSteering = new JoystickSteering(joystick, car.GetMaxSpeed());
		}

		public override void Update(float dt) {
			if (this.joystickSteering == null) {
				return;
			}

			this.car.UpdateFriction();

			if (Love.Keyboard.IsDown(Love.KeyConstant.Space)) {
				this.car.SetPosition(new Love.Vector2(0.0f, 0.0f));
			}

			joystickSteering.UpdateSpeed();
				
			foreach (var tire in this.car.GetLeft())
				tire.UpdateDrive(joystickSteering.GetLeftSpeed(), dt);

			foreach (var tire in this.car.GetRight())
				tire.UpdateDrive(joystickSteering.GetRightSpeed(), dt);

			this.car.frontLeft.KillRotation();

			this.world.Update(dt);
		}

		public override void Draw() {
			Love.Graphics.Push(Love.StackType.All);

			Love.Graphics.Translate(640, 360);
			Love.Graphics.Scale(8, 8);

			this.car.Draw();

			Love.Graphics.Pop();
		}
	}
}