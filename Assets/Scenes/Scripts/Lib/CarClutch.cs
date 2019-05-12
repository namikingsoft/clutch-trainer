namespace Lib
{
    public class CarClutch
    {
        //---- Constants
        // The torque capacity (max transmitted torque) of the clutch is:
        // TC = sliding * radius * area * max-pressure.
        // Should be one-two times the max engine torque, typically 1.25
        private float clutchMaxTorque = 336.53f;

        // Clutch pretends to be fully engaged when (engine speed - transmission speed)
        // is less than (threshold * normal-force)
        private float threshold = 0.001f;

        //---- Variables
        private float clutchPosition = 0.0f;
        private bool locked = false;

        //---- For info only
        private float lastTorque = 0.0f;
        private float engineSpeed = 0.0f;
        private float driveSpeed = 0.0f;

        public void SetMaxTorque(float cmt) { clutchMaxTorque = cmt; }

        // 1.0 is fully engaged
        public void SetClutch(float cp) { clutchPosition = cp; }
        public float GetClutch() { return clutchPosition; }

        // Clutch is modeled as a limited highly-viscous coupling
        public float GetTorque(float newEngineSpeed, float newDriveSpeed)
        {
            engineSpeed = newEngineSpeed;
            driveSpeed = newDriveSpeed;

            float newSpeedDiff = engineSpeed - driveSpeed;
            locked = true;

            float torqueCapacity = clutchMaxTorque; // Constant
            float maxTorque = clutchPosition * torqueCapacity;
            float frictionTorque = maxTorque * newSpeedDiff; // Viscous coupling (locked clutch)

            if (frictionTorque > maxTorque)
            {
                frictionTorque = maxTorque;
                locked = false;
            }
            else if (frictionTorque < -maxTorque)
            {
                frictionTorque = -maxTorque;
                locked = false;
            }

            float torque = frictionTorque;
            lastTorque = torque;
            return torque;
        }

        public bool IsLocked() { return locked; }
        public float GetLastTorque() { return lastTorque; }
    }
}
