"""
ECLIPSE: Local Playable Upgrade for BATCH A (360-Degree Movement, Smooth Rotation, Advanced Locomotion)
"""
import os

HTML_PATH = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "..", "..", "LocalPlayable", "index.html"))

def upgrade_batch_a():
    with open(HTML_PATH, "r", encoding="utf-8") as f:
        html = f.read()

    # 1. Add bodyYaw, currentSpeed, bankAngle to player object definition
    target_player_def = """            breathTimer: 0,
            flinchOffset: new THREE.Vector3()
        };"""

    replacement_player_def = """            breathTimer: 0,
            flinchOffset: new THREE.Vector3(),
            bodyYaw: 0,
            currentSpeed: 0,
            bankAngle: 0,
            isCrouching: false
        };"""

    html = html.replace(target_player_def, replacement_player_def)

    # 2. Upgrade Movement, 360-degree rotation, and Locomotion in animate()
    target_movement = """            // Movement Calculation
            let moveX = 0;
            let moveZ = 0;
            if (keys['w']) moveZ += 1;
            if (keys['s']) moveZ -= 1;
            if (keys['a']) moveX -= 1;
            if (keys['d']) moveX += 1;

            if (touchJoystickActive) {
                moveX = joystickVector.x;
                moveZ = -joystickVector.y;
            }

            const isMoving = Math.abs(moveX) > 0.05 || Math.abs(moveZ) > 0.05;
            const currentSpeed = (player.isSprinting && player.stamina > 10 ? 7.2 : 3.8);

            if (player.isSprinting && isMoving) {
                player.stamina = Math.max(0, player.stamina - dt * 25);
            } else {
                player.stamina = Math.min(100, player.stamina + dt * 15);
            }
            document.getElementById('stamina-fill').style.width = `${player.stamina}%`;

            if (isMoving) {
                const forward = new THREE.Vector3(-Math.sin(player.yaw), 0, -Math.cos(player.yaw));
                const right = new THREE.Vector3(Math.cos(player.yaw), 0, -Math.sin(player.yaw));

                const moveDir = new THREE.Vector3()
                    .addScaledVector(forward, moveZ)
                    .addScaledVector(right, moveX)
                    .normalize();

                player.position.addScaledVector(moveDir, currentSpeed * dt);
                // Keep inside street bounds
                player.position.x = Math.max(-16, Math.min(16, player.position.x));
                player.position.z = Math.max(-110, Math.min(110, player.position.z));
            }

            // Update Player Model
            player.mesh.position.copy(player.position);
            player.mesh.rotation.y = player.yaw;"""

    replacement_movement = """            // 1. Normalized 360-Degree Input Vector (WASD + Mobile Joystick)
            let rawX = 0;
            let rawZ = 0;
            if (keys['w']) rawZ += 1;
            if (keys['s']) rawZ -= 1;
            if (keys['a']) rawX -= 1;
            if (keys['d']) rawX += 1;

            if (touchJoystickActive) {
                rawX = joystickVector.x;
                rawZ = -joystickVector.y;
            }

            const inputMag = Math.sqrt(rawX * rawX + rawZ * rawZ);
            const isMoving = inputMag > 0.05;

            let normX = rawX;
            let normZ = rawZ;
            if (inputMag > 1.0) {
                normX /= inputMag;
                normZ /= inputMag;
            }

            // 2. Target Speed calculation (Walk, Run, Sprint, Aim Strafe)
            let targetSpeed = 0;
            if (isMoving) {
                if (player.isAiming) {
                    targetSpeed = (normZ < -0.2) ? 2.4 : 3.0; // Backward combat retreat or strafe
                } else if (player.isCrouching) {
                    targetSpeed = 2.0;
                } else if (player.isSprinting && player.stamina > 10) {
                    targetSpeed = 7.5; // High-velocity sprint
                } else if (inputMag > 0.8) {
                    targetSpeed = 5.4; // Tactical Jog / Run
                } else {
                    targetSpeed = 3.6; // Controlled Walk
                }
            }

            // Smooth Acceleration & Deceleration
            const accelRate = isMoving ? 12 : 16;
            player.currentSpeed = THREE.MathUtils.lerp(player.currentSpeed, targetSpeed, dt * accelRate);

            if (player.isSprinting && isMoving) {
                player.stamina = Math.max(0, player.stamina - dt * 25);
            } else {
                player.stamina = Math.min(100, player.stamina + dt * 15);
            }
            document.getElementById('stamina-fill').style.width = `${player.stamina}%`;

            // 3. Camera-Relative Movement Vector
            const moveDir = new THREE.Vector3();
            if (isMoving) {
                const forward = new THREE.Vector3(-Math.sin(player.yaw), 0, -Math.cos(player.yaw));
                const right = new THREE.Vector3(Math.cos(player.yaw), 0, -Math.sin(player.yaw));

                moveDir.addScaledVector(forward, normZ)
                       .addScaledVector(right, normX)
                       .normalize();

                player.position.addScaledVector(moveDir, player.currentSpeed * dt);
                // Keep inside street bounds
                player.position.x = Math.max(-16, Math.min(16, player.position.x));
                player.position.z = Math.max(-110, Math.min(110, player.position.z));
            }

            // 4. AAA 360-DEGREE SMOOTH BODY ROTATION
            if (player.isAiming) {
                // Combat Strafe Mode: Body smoothly locks to camera aim yaw
                let diff = (player.yaw - player.bodyYaw) % (Math.PI * 2);
                if (diff < -Math.PI) diff += Math.PI * 2;
                if (diff > Math.PI) diff -= Math.PI * 2;
                player.bodyYaw += diff * Math.min(1.0, dt * 18);
                player.bankAngle = THREE.MathUtils.lerp(player.bankAngle, 0, dt * 10);
            } else if (isMoving) {
                // Free Exploration Mode: Body smoothly rotates 360 degrees to face movement direction!
                const targetAngle = Math.atan2(-moveDir.x, -moveDir.z);
                let diff = (targetAngle - player.bodyYaw) % (Math.PI * 2);
                if (diff < -Math.PI) diff += Math.PI * 2;
                if (diff > Math.PI) diff -= Math.PI * 2;

                const turnSpeed = (player.isSprinting ? 16 : 12) * (1.0 + Math.abs(diff) / Math.PI * 0.5);
                player.bodyYaw += diff * Math.min(1.0, dt * turnSpeed);

                // Body Banking into turns (subtle roll)
                const targetBank = Math.max(-0.10, Math.min(0.10, -diff * 0.22));
                player.bankAngle = THREE.MathUtils.lerp(player.bankAngle, targetBank, dt * 8);
            } else {
                player.bankAngle = THREE.MathUtils.lerp(player.bankAngle, 0, dt * 10);
            }

            // Update Player Mesh Position, 360 Rotation & Banking
            player.mesh.position.copy(player.position);
            player.mesh.rotation.y = player.bodyYaw;
            player.mesh.rotation.z = player.bankAngle;"""

    html_norm = html.replace("\r\n", "\n")
    target_norm = target_movement.replace("\r\n", "\n")
    replacement_norm = replacement_movement.replace("\r\n", "\n")

    if target_norm in html_norm:
        html_norm = html_norm.replace(target_norm, replacement_norm, 1)
        print("[BatchA] Injected AAA 360-degree movement and smooth body rotation.")
    else:
        print("[BatchA] Warning: target movement not found in html.")

    # 3. Enhance skeletal animations with forward sprint body lean and arm pumping
    target_sprint_anim = """                const walkAmp = isMoving ? ((player.isSprinting && player.stamina > 10) ? 0.72 : 0.48) : 0;"""
    replacement_sprint_anim = """                const isSprintActive = player.isSprinting && player.stamina > 10 && isMoving;
                const walkAmp = isMoving ? (isSprintActive ? 0.76 : (player.currentSpeed > 4.5 ? 0.58 : 0.42)) : 0;
                
                // Spine forward lean into sprint & running
                const targetSpineLean = isSprintActive ? 0.28 : (isMoving ? 0.12 : 0.0);
                r.spine.rotation.x = THREE.MathUtils.lerp(r.spine.rotation.x, targetSpineLean, dt * 8);"""

    target_sa_norm = target_sprint_anim.replace("\r\n", "\n")
    replacement_sa_norm = replacement_sprint_anim.replace("\r\n", "\n")
    if target_sa_norm in html_norm:
        html_norm = html_norm.replace(target_sa_norm, replacement_sa_norm, 1)
        print("[BatchA] Injected dynamic sprint spine lean and multi-tier walk/run/sprint amplitudes.")

    with open(HTML_PATH, "w", encoding="utf-8") as f:
        f.write(html_norm)

    print("[BatchA] Upgrade complete.")

if __name__ == "__main__":
    upgrade_batch_a()

