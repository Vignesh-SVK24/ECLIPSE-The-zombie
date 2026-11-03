using UnityEngine;
using UnityEngine.Rendering;

namespace Eclipse.Settings
{
    public static class MobileDeviceProfile
    {
        public struct DeviceCapabilities
        {
            public string DeviceModel;
            public string GraphicsDeviceName;
            public GraphicsDeviceType GraphicsDeviceType;
            public int SystemMemoryMB;
            public int GraphicsMemoryMB;
            public int ProcessorCount;
            public bool SupportsVulkanOrMetal;
            public QualityLevelTier RecommendedTier;
        }

        public static DeviceCapabilities Detect()
        {
            var caps = new DeviceCapabilities
            {
                DeviceModel = SystemInfo.deviceModel,
                GraphicsDeviceName = SystemInfo.graphicsDeviceName,
                GraphicsDeviceType = SystemInfo.graphicsDeviceType,
                SystemMemoryMB = SystemInfo.systemMemorySize,
                GraphicsMemoryMB = SystemInfo.graphicsMemorySize,
                ProcessorCount = SystemInfo.processorCount
            };

            caps.SupportsVulkanOrMetal =
                caps.GraphicsDeviceType == GraphicsDeviceType.Vulkan ||
                caps.GraphicsDeviceType == GraphicsDeviceType.Metal;

            caps.RecommendedTier = EvaluateRecommendedTier(caps);
            return caps;
        }

        private static QualityLevelTier EvaluateRecommendedTier(DeviceCapabilities caps)
        {
#if UNITY_ANDROID || UNITY_IOS
            // Mobile memory thresholds
            if (caps.SystemMemoryMB < 3500)
            {
                return QualityLevelTier.Low;
            }
            if (caps.SystemMemoryMB < 5500)
            {
                return QualityLevelTier.Medium;
            }
            return QualityLevelTier.High;
#else
            // Desktop / Editor default to High or Medium based on RAM
            if (caps.SystemMemoryMB < 6000)
            {
                return QualityLevelTier.Medium;
            }
            return QualityLevelTier.High;
#endif
        }

        public static void LogCapabilitiesSummary()
        {
            var caps = Detect();
            Debug.Log($"[ECLIPSE DeviceProfile] Model: {caps.DeviceModel} | RAM: {caps.SystemMemoryMB}MB | " +
                      $"GPU: {caps.GraphicsDeviceName} ({caps.GraphicsDeviceType}) | " +
                      $"Recommended Quality Tier: {caps.RecommendedTier}");
        }
    }
}
