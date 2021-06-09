using System;

namespace Steering
{
    public partial class JoystickSteering
    {

        private Love.Joystick joysticks = null;
        private float bound;
        private float safetyFactor;
        private float carMaxSpeed;

        private float leftSpeed = 0.0f;
        private float rightSpeed = 0.0f;

        private InputCurve inputCurve = InputCurve.CUBIC;

        public JoystickSteering(Love.Joystick joysticks, float maxSpeed, float bound = 0.1f, float safetyFactor = 0.5f, float carMaxSpeed = 255, InputCurve inputCurve = InputCurve.CUBIC)
        {
            this.joysticks = joysticks;
            this.carMaxSpeed = maxSpeed;

            this.bound = bound;
            this.safetyFactor = safetyFactor;
            this.carMaxSpeed = carMaxSpeed;
            this.inputCurve = inputCurve;
        }

        public void UpdateSpeed()
        {
            this.rightSpeed = 0.0f;
            this.leftSpeed = 0.0f;

            var yJoystick = joysticks.GetGamepadAxis(Love.GamepadAxis.LeftY);
            var xJoystick = joysticks.GetGamepadAxis(Love.GamepadAxis.RightX);

            if (yJoystick > bound || yJoystick < -bound)
            {
                leftSpeed += GetEased(yJoystick) * safetyFactor * carMaxSpeed;
                rightSpeed += GetEased(yJoystick) * safetyFactor * carMaxSpeed;
            }

            if (xJoystick > bound || xJoystick < -bound)
            {
                float winding = yJoystick < bound ? -1 : 1;

                leftSpeed += xJoystick * safetyFactor * carMaxSpeed * winding;
                rightSpeed -= xJoystick * safetyFactor * carMaxSpeed * winding;
            }
        }

        public float GetLeftSpeed()
        {
            return leftSpeed;
        }

        public float GetRightSpeed()
        {
            return rightSpeed;
        }

        private float GetEased(float x)
        {
            switch (this.inputCurve)
            {
                case InputCurve.LINEAR:
                    return Linear(x);
                case InputCurve.CUBIC:
                    return CubicIn(x);
            }
            return 0.0f;
        }

        private float CubicIn(float x)
        {
            return (float)Math.Pow(x, 3);
        }

        private float Linear(float x)
        {
            return x;
        }
    }
}