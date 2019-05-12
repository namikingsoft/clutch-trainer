namespace Lib
{
    public class CarBrake
    {
        // Constants
        private float fric = 0.73f; // Sliding coefficient of friction for brake pads on rotor
        private float maxPress = 4e6f; // Max allowed pressure
        private float radius = 0.14f; // Effective radius of rotor
        private float area = 0.015f; // Area of brake pads
        private float bias = 0.5f; // Fraction of pressure to be applied to brake
        private float threshold = 2e-4f; // Brake will lock when (linear brake velocity / normal force) is under this
        private float handbrake; // Friction factor applied when handbrake is pulled

        // Variables
        private float brakeFactor = 0;
        private float handbrakeFactor = 0;
        bool locked = false;

        // For info only
        private float lastTorque = 0;

        // 0.0 (no brakes applied) to 1.0 (brakes applied fully)
        public void SetBrakeFactor(float bf) { brakeFactor = bf; }
        public void SetHandbrakeFactor(float hf) { handbrakeFactor = hf; }

        // Magnitude of brake torque
        public float GetTorque()
        {
            float brake = brakeFactor > handbrake * handbrakeFactor ? brakeFactor : handbrake * handbrakeFactor;
            float pressure = brake * bias * maxPress;
            float normal = pressure * area;
            float torque = fric * normal * radius;

            lastTorque = torque;
            return torque;
        }

        // Used by autoclutch system
        public bool WillLock() { return locked; }
        public void SetWillLock(bool lck) { locked = lck; }

        public void SetFriction(float f) { fric = f; }
        public float GetFriction() { return fric; }

        public void SetMaxPressure(float mp) { maxPress = mp; }
        public void SetRadius(float r) { radius = r; }
        public void SetArea(float a) { area = a; }
        public void SetBias(float b) { bias = b; }

        public bool GetLocked() { return locked; }
        public float GetBrakeFactor() { return brakeFactor; }
        public float GetHandbrakeFactor() { return handbrakeFactor; }
        public void SetHandbrake(float h) { handbrake = h; }
    }
}
