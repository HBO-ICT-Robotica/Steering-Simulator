using System.Collections.Generic;

namespace Steering {
	public class Car {
		private float maxSpeed = 255.0f; // Matches PWM
		private float acceleration = 40.0f; // Arbitrairy acceleration

		private Love.Body body = null;
		private Love.PolygonShape shape = null;
		private Love.Fixture fixture = null;

		public Tire frontLeft = null;
		public Tire frontRight = null;
		public Tire backLeft = null;
		public Tire backRight = null;

		public Car(Love.World world) {
			this.body = Love.Physics.NewBody(world, 0.0f, 0.0f, Love.BodyType.Dynamic);
			this.shape = Love.Physics.NewRectangleShape(2.0f, 5.0f);
			this.fixture = Love.Physics.NewFixture(this.body, this.shape, 1.0f);

			this.frontLeft = new Tire(world, maxSpeed, acceleration);
			var jointFL = Love.Physics.NewWeldJoint(this.body, this.frontLeft.GetBody(), new Love.Vector2(0.0f, 0.0f), new Love.Vector2(1.5f, 2.0f), false);

			this.frontRight = new Tire(world, maxSpeed, acceleration);
			var jointFR = Love.Physics.NewWeldJoint(this.body, this.frontRight.GetBody(), new Love.Vector2(0.0f, 0.0f), new Love.Vector2(-1.5f, 2.0f), false);

			this.backLeft = new Tire(world, maxSpeed, acceleration);
			var jointBL = Love.Physics.NewWeldJoint(this.body, this.backLeft.GetBody(), new Love.Vector2(0.0f, 0.0f), new Love.Vector2(1.5f, -2.0f), false);

			this.backRight = new Tire(world, maxSpeed, acceleration);
			var jointBR = Love.Physics.NewWeldJoint(this.body, this.backRight.GetBody(), new Love.Vector2(0.0f, 0.0f), new Love.Vector2(-1.5f, -2.0f), false);
		}

		public void UpdateFriction() {
			this.frontLeft.UpdateFriction();
			this.frontRight.UpdateFriction();
			this.backLeft.UpdateFriction();
			this.backRight.UpdateFriction();
		}

		public void KillRotation() {
			this.frontLeft.KillRotation();
			this.frontRight.KillRotation();
			this.backLeft.KillRotation();
			this.backRight.KillRotation();
		}

		public float GetMaxSpeed() {
			return 255.0f;
		}

		public Tire GetFrontLeft() {
			return this.frontLeft;
		}

		public Tire GetFrontRight() {
			return this.frontRight;
		}

		public Tire GetBackLeft() {
			return this.backLeft;
		}

		public Tire GetBackRight() {
			return this.backRight;
		}

		public IReadOnlyList<Tire> GetFront() {
			return new List<Tire>() { this.frontLeft, this.frontRight };
		} 

		public IReadOnlyList<Tire> GetBack() {
			return new List<Tire>() { this.backLeft, this.backRight };
		} 

		public IReadOnlyList<Tire> GetLeft() {
			return new List<Tire>() { this.frontLeft, this.backLeft };
		} 

		public IReadOnlyList<Tire> GetRight() {
			return new List<Tire>() { this.frontRight, this.backRight };
		} 

		public void Draw() {
			var points = this.shape.GetPoints();

			var newPoints = new Love.Vector2[points.Length];

			for (var i = 0; i < points.Length; i++) {
				newPoints[i] = this.body.GetWorldPoint(points[i]);
			}

			Love.Graphics.Polygon(Love.DrawMode.Fill, newPoints);

			this.frontLeft.Draw();
			this.frontRight.Draw();
			this.backLeft.Draw();
			this.backRight.Draw();
		}
	}
}