# ECLIPSE: THE LAST CIVILIZATION
## iOS Xcode Project Export & Device Signing Guide

**Developer:** SVK Creationz  
**Application ID:** `com.svkcreationz.eclipse`  
**Graphics API:** Metal (Low-overhead native iOS API)  
**Output Destination:** `Builds/iOS/ECLIPSE-iOS-Xcode-Project/`

---

### 1. Workstation & Environment Requirements
> [!NOTE]
> Apple requires macOS and Xcode to sign and deploy native iOS `.ipa` applications to physical iPhones/iPads. On a Windows workstation, Unity 6 exports the complete, self-contained Xcode project folder. You can transfer this folder to any Mac running Xcode for final signing and installation.

---

### 2. Exporting the iOS Xcode Project from Unity
1. In Unity Editor, click top menu:  
   **`ECLIPSE > Build > Export iOS Xcode Project`**
2. Unity compiles IL2CPP C++ wrappers, packs URP shaders, and generates the complete Xcode workspace inside:  
   `Builds/iOS/ECLIPSE-iOS-Xcode-Project/`

---

### 3. Final Signing & Deployment on macOS (Xcode)
1. Transfer `Builds/iOS/ECLIPSE-iOS-Xcode-Project/` to your Mac.
2. Open `Unity-iPhone.xcodeproj` in **Xcode**.
3. In the project settings under **Signing & Capabilities**:
   - Check **Automatically manage signing**.
   - Select your **Apple Developer Team** or free personal Apple ID.
   - Set Bundle Identifier to `com.svkcreationz.eclipse`.
4. Connect your iPhone via USB, select your device as the run target in Xcode, and click **Run (⌘ + R)**.
5. On your iPhone: Go to `Settings > General > VPN & Device Management` and tap **Trust Developer**.
6. Launch **ECLIPSE: THE LAST CIVILIZATION**.
