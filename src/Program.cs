namespace Steering {
	public class Program : Love.Scene {
		private Love.World world = null;

		private Car car = null;

		public Program() {
			this.world = Love.Physics.NewWorld(0.0f, 0.0f, false);

			this.car = new Car(this.world);
		}

		public override void Update(float dt) {
			this.car.UpdateFriction();

			float maxSpeed = this.car.GetMaxSpeed();

			float leftSpeed = 0.0f;
			float rightSpeed = 0.0f;

			if (Love.Keyboard.IsDown(Love.KeyConstant.W)) {
				leftSpeed += maxSpeed * -0.5f; 
				rightSpeed += maxSpeed * -0.5f; 
			}

			if (Love.Keyboard.IsDown(Love.KeyConstant.S)) {
				leftSpeed -= maxSpeed * -0.5f; 
				rightSpeed -= maxSpeed * -0.5f; 
			}

			if (Love.Keyboard.IsDown(Love.KeyConstant.A)) {
				rightSpeed += maxSpeed * -0.5f; 
			}

			if (Love.Keyboard.IsDown(Love.KeyConstant.D)) {
				leftSpeed += maxSpeed * -0.5f; 
			}
				
				
			foreach (var tire in this.car.GetLeft())
				tire.UpdateDrive(leftSpeed, dt);

			foreach (var tire in this.car.GetRight())
				tire.UpdateDrive(rightSpeed, dt);

			this.car.frontLeft.KillRotation();

			this.world.Update(dt);
		}

		public override void Draw() {
			Love.Graphics.Push(Love.StackType.All);

			Love.Graphics.Translate(640, 360);
			Love.Graphics.Scale(12, 12);

			this.car.Draw();

			Love.Graphics.Pop();
		}
	}
}