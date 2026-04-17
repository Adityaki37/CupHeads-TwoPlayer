using CupheadOnline.Net;

namespace CupheadOnline.Sync
{
    /// <summary>
    /// Stores the most recently received InputFramePacket for the remote player.
    ///
    /// PlayerMotorPatch intercepts PlayerInput.GetAxis / GetButton for the remote
    /// player's ID and returns values from this driver, so the motor processes
    /// network inputs instead of local controller inputs.
    ///
    /// Thread-safety: all writes come from PacketDispatcher (main thread),
    /// all reads from PlayerMotorPatch (main thread via FixedUpdate).
    /// No locks needed.
    /// </summary>
    public static class RemoteInputDriver
    {
        public static InputFramePacket Current { get; private set; }
        public static bool HasData            { get; private set; }

        // Age in FixedUpdate ticks since last packet — used to zero inputs on starvation
        private static int _stallFrames;
        private const  int MAX_STALL = 6; // ~100 ms at 60 Hz before inputs zeroed

        public static void Apply(InputFramePacket pkt)
        {
            Current     = pkt;
            HasData     = true;
            _stallFrames = 0;
        }

        /// <summary>Called each FixedUpdate by PlayerMotorPatch to age the input.</summary>
        public static void Tick()
        {
            if (!HasData) return;
            _stallFrames++;
            if (_stallFrames > MAX_STALL)
            {
                // Starvation: zero all inputs so the remote player stops moving
                Current  = default;
                HasData  = false;
            }
        }
    }
}
