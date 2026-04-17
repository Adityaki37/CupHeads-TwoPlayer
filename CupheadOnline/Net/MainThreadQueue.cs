using System;
using System.Collections.Generic;

namespace CupheadOnline.Net
{
    /// <summary>
    /// Thread-safe trampoline from the receive thread back onto Unity's main thread.
    /// Uses a plain Queue + lock (compatible with .NET 2.0 / Mono mscorlib 2.0.0.0).
    ///
    /// Usage:
    ///   (recv thread)  MainThreadQueue.Enqueue(() => DoSomethingWithUnity());
    ///   (main thread)  MainThreadQueue.Drain();   // called from Plugin.Update()
    /// </summary>
    public static class MainThreadQueue
    {
        private static readonly Queue<Action> _queue = new Queue<Action>();
        private static readonly object        _lock  = new object();

        /// <summary>Enqueue an action to run on the main thread. Safe to call from any thread.</summary>
        public static void Enqueue(Action action)
        {
            if (action == null) return;
            lock (_lock)
                _queue.Enqueue(action);
        }

        /// <summary>
        /// Drain and execute all pending actions.
        /// Must be called from Unity's main thread (Plugin.Update).
        /// Capped to 128 actions per frame to prevent stalls under burst load.
        /// </summary>
        public static void Drain()
        {
            const int MAX_PER_FRAME = 128;
            int count = 0;
            while (count < MAX_PER_FRAME)
            {
                Action action;
                lock (_lock)
                {
                    if (_queue.Count == 0) break;
                    action = _queue.Dequeue();
                }
                count++;
                try   { action(); }
                catch (Exception ex) { Plugin.Log.LogError("[MainThreadQueue] " + ex); }
            }
        }

        public static int Pending { get { lock (_lock) return _queue.Count; } }
    }
}
