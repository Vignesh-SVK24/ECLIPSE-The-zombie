using System;
using UnityEngine;

namespace Eclipse.AI
{
    public enum SoundCategory
    {
        Footstep = 0,
        Sprint = 1,
        Gunshot = 2,
        Explosion = 3,
        VehicleEngine = 4,
        GlassBreak = 5,
        VoiceShout = 6
    }

    public struct SoundEvent
    {
        public Vector3 Position;
        public float Radius;
        public SoundCategory Category;
        public GameObject Source;

        public SoundEvent(Vector3 position, float radius, SoundCategory category, GameObject source = null)
        {
            Position = position;
            Radius = Mathf.Max(0.5f, radius);
            Category = category;
            Source = source;
        }
    }

    public static class SoundEventBus
    {
        public static event Action<SoundEvent> OnSoundEmitted;

        public static void EmitSound(Vector3 position, float radius, SoundCategory category, GameObject source = null)
        {
            OnSoundEmitted?.Invoke(new SoundEvent(position, radius, category, source));
        }
    }
}
