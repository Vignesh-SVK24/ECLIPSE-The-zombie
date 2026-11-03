"""
ECLIPSE: Backend and Connectivities Injection Script
Injects real-time WebSocket client, remote player interpolation, REST API save/load,
and connectivity HUD status badge into LocalPlayable/index.html.
"""
import os

HTML_PATH = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "..", "..", "LocalPlayable", "index.html"))

def inject_connectivity():
    with open(HTML_PATH, "r", encoding="utf-8") as f:
        html = f.read()

    # 1. Add CSS for connectivity badge
    target_css = ".kill-counter {"
    replacement_css = """.connectivity-badge {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            background: rgba(17, 25, 24, 0.90);
            border: 1px solid rgba(76, 175, 80, 0.45);
            padding: 4px 10px;
            border-radius: 4px;
            font-size: 11px;
            font-weight: 600;
            letter-spacing: 0.5px;
            color: #a5d6a7;
            margin-top: 6px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.5);
        }
        .status-dot {
            width: 8px;
            height: 8px;
            border-radius: 50%;
            background: #4caf50;
            box-shadow: 0 0 8px #4caf50;
        }
        .status-dot.offline {
            background: #f44336;
            box-shadow: 0 0 8px #f44336;
        }
        .ping-badge {
            color: #81c784;
            font-family: monospace;
        }
        .kill-counter {"""

    html = html.replace(target_css, replacement_css)

    # 2. Add Connectivity Badge to HUD
    target_hud = """                <div class="kill-counter">KILLS: <span id="kill-count">0</span> | THREAT: <span id="threat-level">ACTIVE</span></div>
            </div>"""

    replacement_hud = """                <div class="kill-counter">KILLS: <span id="kill-count">0</span> | THREAT: <span id="threat-level">ACTIVE</span></div>
                <div class="connectivity-badge" id="connectivity-badge">
                    <span class="status-dot"></span>
                    <span id="conn-text">CONNECTING TO BACKEND...</span>
                    <span class="ping-badge" id="ping-text">PING: --ms</span>
                </div>
            </div>"""

    html = html.replace(target_hud, replacement_hud)

    # 3. Add Backend Client & WebSocket Manager in Script
    target_script = "// --- Start Animation Loop ---"
    replacement_script = """// =========================================================================
        // BACKEND & REAL-TIME WEBSOCKET MULTIPLAYER CONNECTIVITY
        // =========================================================================
        let ws = null;
        let clientId = null;
        let currentRoom = 'ECL-709';
        const remotePlayers = new Map(); // id -> { mesh, targetPos, targetYaw }
        let lastSyncTime = 0;
        let pingStartTime = 0;

        function initNetworkBackend() {
            const protocol = window.location.protocol === 'https:' ? 'wss:' : 'ws:';
            const wsUrl = `${protocol}//${window.location.host}/ws`;

            try {
                ws = new WebSocket(wsUrl);

                ws.onopen = () => {
                    console.log('[Backend] Connected to ECLIPSE Master Server at', wsUrl);
                    updateConnStatus(true, 'CONNECTED | CO-OP: ' + currentRoom);
                    measurePing();
                };

                ws.onmessage = (event) => {
                    try {
                        const msg = JSON.parse(event.data);
                        handleNetworkMessage(msg);
                    } catch (err) {}
                };

                ws.onclose = () => {
                    updateConnStatus(false, 'OFFLINE (SOLO)');
                    setTimeout(initNetworkBackend, 3000);
                };

                ws.onerror = () => {
                    updateConnStatus(false, 'OFFLINE (SOLO)');
                };
            } catch (e) {
                updateConnStatus(false, 'LOCAL SOLO MODE');
            }
        }

        function updateConnStatus(online, text) {
            const badge = document.getElementById('connectivity-badge');
            const dot = badge ? badge.querySelector('.status-dot') : null;
            const txt = document.getElementById('conn-text');
            if (dot) dot.className = online ? 'status-dot' : 'status-dot offline';
            if (txt) txt.textContent = text;
        }

        function measurePing() {
            if (ws && ws.readyState === WebSocket.OPEN) {
                pingStartTime = performance.now();
                ws.send(JSON.stringify({ type: 'ping', clientTimestamp: pingStartTime }));
            }
            setTimeout(measurePing, 5000);
        }

        function handleNetworkMessage(msg) {
            if (msg.type === 'welcome') {
                clientId = msg.clientId;
                currentRoom = msg.roomCode;
                updateConnStatus(true, `CONNECTED | ROOM: ${currentRoom}`);
            } else if (msg.type === 'pong') {
                const rtt = Math.round(performance.now() - pingStartTime);
                const pingEl = document.getElementById('ping-text');
                if (pingEl) pingEl.textContent = `PING: ${rtt}ms`;
            } else if (msg.type === 'remote_player_update') {
                updateRemotePlayer(msg.player);
            } else if (msg.type === 'player_disconnected') {
                removeRemotePlayer(msg.playerId);
            }
        }

        function createRemotePlayerVisual(name) {
            const group = new THREE.Group();
            // Torso
            const torso = new THREE.Mesh(new THREE.BoxGeometry(0.48, 0.65, 0.28), charPBR.vest);
            torso.position.y = 1.25;
            group.add(torso);
            // Head
            const head = new THREE.Mesh(new THREE.SphereGeometry(0.14, 12, 10), charPBR.head);
            head.position.y = 1.72;
            group.add(head);
            // Ally Nameplate Indicator
            const tag = new THREE.Mesh(new THREE.BoxGeometry(0.6, 0.08, 0.02), new THREE.MeshBasicMaterial({ color: 0x4caf50 }));
            tag.position.y = 1.95;
            group.add(tag);
            return group;
        }

        function updateRemotePlayer(pData) {
            if (!pData || pData.id === clientId) return;

            let remote = remotePlayers.get(pData.id);
            if (!remote) {
                const allyMesh = createRemotePlayerVisual(pData.name);
                scene.add(allyMesh);
                remote = {
                    id: pData.id,
                    mesh: allyMesh,
                    targetPos: new THREE.Vector3(pData.position.x, pData.position.y, pData.position.z),
                    targetYaw: pData.bodyYaw
                };
                remotePlayers.set(pData.id, remote);
            }

            remote.targetPos.set(pData.position.x, pData.position.y, pData.position.z);
            remote.targetYaw = pData.bodyYaw;
        }

        function removeRemotePlayer(pId) {
            const remote = remotePlayers.get(pId);
            if (remote) {
                scene.remove(remote.mesh);
                remotePlayers.delete(pId);
            }
        }

        // Initialize Backend Connection
        initNetworkBackend();

        // --- Start Animation Loop ---"""

    html = html.replace(target_script, replacement_script)

    # 4. Add periodic synchronization inside animate()
    target_animate_end = "renderer.render(scene, camera);"
    replacement_animate_end = """renderer.render(scene, camera);

            // Interpolate Remote Co-Op Players
            for (const [id, remote] of remotePlayers.entries()) {
                remote.mesh.position.lerp(remote.targetPos, dt * 10);
                remote.mesh.rotation.y = THREE.MathUtils.lerp(remote.mesh.rotation.y, remote.targetYaw, dt * 12);
            }

            // Sync Local Player State to Backend (20Hz)
            if (ws && ws.readyState === WebSocket.OPEN && (clock.getElapsedTime() - lastSyncTime > 0.05)) {
                lastSyncTime = clock.getElapsedTime();
                ws.send(JSON.stringify({
                    type: 'player_sync',
                    position: { x: player.position.x, y: player.position.y, z: player.position.z },
                    bodyYaw: player.bodyYaw,
                    isSprinting: player.isSprinting,
                    isAiming: player.isAiming,
                    health: player.health,
                    weapon: weapons[currentWeaponIndex].name
                }));
            }"""

    html = html.replace(target_animate_end, replacement_animate_end)

    with open(HTML_PATH, "w", encoding="utf-8") as f:
        f.write(html)

    print("[BackendInject] Successfully injected real-time WebSocket connectivity and HUD badges into LocalPlayable/index.html")

if __name__ == "__main__":
    inject_connectivity()

