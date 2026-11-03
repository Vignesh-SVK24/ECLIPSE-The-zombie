# ECLIPSE: THE LAST CIVILIZATION
## Mobile Known Issues & Optimization Notes

1. **Unity Gaming Services Relay Configuration:**  
   When connecting two physical devices across cellular 5G networks, the project must be linked to a free Unity Gaming Services Project ID under `Project Settings > Services`. Local Wi-Fi loopback does not require cloud authentication.

2. **iOS Compilation Platform Boundary:**  
   Windows development environments do not support direct compilation of `.ipa` binaries due to Apple's native Xcode requirement. The Unity project exports the complete, self-contained Xcode project ready to build on any Mac.

3. **Android USB Driver Authorization:**  
   When connecting an Android phone for the first time via USB, the user must ensure "Always allow from this computer" is checked on the phone's RSA key popup so ADB can install the APK.
