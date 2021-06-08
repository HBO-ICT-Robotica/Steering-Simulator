using System;

namespace Steering {
	public class Tire {
		private Love.Body body = null;
		private Love.PolygonShape shape = null;
		private Love.Fixture fixture = null;

		private float maxSpeed = 0.0f;
		private float acceleration = 0.0f;

		private float desiredSpeed = 0.0f;

		public Tire(Love.World world, float maxSpeed, float acceleration) {
			this.body = Love.Physics.NewBody(world, 0.0f, 0.0f, Love.BodyType.Dynamic);
			this.shape = Love.Physics.NewRectangleShape(0.5f, 1.25f);
			this.fixture = Love.Physics.NewFixture(this.body, this.shape, 1.0f);

			this.maxSpeed = maxSpeed;
			this.acceleration = acceleration;
		}

		public void UpdateDrive(float desiredSpeed, float dt) {
			this.desiredSpeed = desiredSpeed;

			Love.Vector2 currentForwardNormal = this.body.GetWorldVector(new Love.Vector2(0.0f, 1.0f));
			float currentSpeed = Love.Vector2.Dot(this.GetForwardVelocity(), currentForwardNormal);

			float delta = Math.Clamp(desiredSpeed - currentSpeed, -acceleration, acceleration) * dt;

			Love.Vector2 force = (delta * currentForwardNormal);
			Love.Vector2 center = this.body.GetWorldCenter();
			this.body.ApplyForce(force.X, force.Y, center.X, center.Y);
		}

		public Love.Vector2 GetForwardVelocity() {
			Love.Vector2 currentForwardNormal = this.body.GetWorldVector(new Love.Vector2(0.0f, 1.0f));
			return Love.Vector2.Dot(currentForwardNormal, this.body.GetLinearVelocity()) * currentForwardNormal;
		}

		public Love.Vector2 GetLateralVelocity() {
			Love.Vector2 currentRightNormal = this.body.GetWorldVector(new Love.Vector2(1.0f, 0.0f));

			return Love.Vector2.Dot(currentRightNormal, this.body.GetLinearVelocity()) * currentRightNormal;
		}

		public void UpdateFriction() {
			Love.Vector2 impulse = this.body.GetMass() * -this.GetLateralVelocity();
			Love.Vector2 center = this.body.GetWorldCenter();

			this.body.ApplyLinearImpulse(impulse.X, impulse.Y, center.X, center.Y);
		}

		public void KillRotation() {
			this.body.ApplyAngularImpulse(0.1f * this.body.GetInertia() * -this.body.GetAngularVelocity());
		}

		public Love.Body GetBody() {
			return this.body;
		}

		public void Draw() {
			var points = this.shape.GetPoints();

			var newPoints = new Love.Vector2[points.Length];

			for (var i = 0; i < points.Length; i++) {
				newPoints[i] = this.body.GetWorldPoint(points[i]);
			}

			Love.Graphics.Polygon(Love.DrawMode.Fill, newPoints);

			var magnitude = this.desiredSpeed / this.maxSpeed;
			var angle = this.body.GetAngle();
			var pos = this.body.GetPosition() + new Love.Vector2(MathF.Sin(-angle) * magnitude, MathF.Cos(-angle) * magnitude);
			Love.Graphics.SetColor(1.0f, 0.0f, 0.0f, 1.0f);
			Love.Graphics.Circle(Love.DrawMode.Fill, pos, 0.2f);
			Love.Graphics.SetColor(1.0f, 1.0f, 1.0f, 1.0f);
		}
	}
}