# ECLIPSE: THE LAST CIVILIZATION
## Backend & Connectivities Architecture & Implementation Report

- **Date:** 2026-11-03
- **Lead Backend Engineer:** Antigravity Senior Engineering Assistant
- **Target Stack:** Node.js Master Backend + RFC 6455 WebSocket Engine + REST API + Unity Netcode Bridge
- **Master Server Status:** **ONLINE** on Port `5173`
- **Host Link:** [http://localhost:5173](http://localhost:5173)

---

### 1. Architectural Topology

```
+-----------------------------------------------------------------------------------------+
|                                    ECLIPSE BACKEND                                      |
|  +-------------------------+  +--------------------------+  +-------------------------+ |
|  | Native Node.js Server   |  | RFC 6455 WebSocket Engine|  | Save File Store         | |
|  | - Port 5173 (0.0.0.0)   |  | - Room ECL-709           |  | - saves/savegame.json   | |
|  | - CORS & Static Assets  |  | - 20-30Hz State Sync     |  |                         | |
|  +------------+------------+  +------------+-------------+  +------------+------------+ |
+---------------|----------------------------|-----------------------------|--------------+
                |                            |                             |
                v                            v                             v
   +------------------------+   +--------------------------+  +-------------------------+
   | Web Client (Player 1)  |   | Web Client (Player 2/LAN)|  | Unity 6 MCP Bridge      |
   | - Local Ethan Cross    |   | - Remote Ally Spawning   |  | - http://127.0.0.1:8080 |
   | - 360° Locomotion Sync |   | - Multi-Touch Controls   |  | - Unity Editor Bridge   |
   +------------------------+   +--------------------------+  +-------------------------+
```

---

### 2. Implemented Backend Endpoints

#### A. REST API Endpoints
| Endpoint | Method | Description | Sample Output |
| :--- | :---: | :--- | :--- |
| `/api/status` | `GET` | Server health check, uptime, and active player counts | `{"status":"online","connectedClients":0,"activeRooms":1,"uptimeSeconds":120}` |
| `/api/rooms` | `GET` | List active co-op lobbies and capacities | `[{"code":"ECL-709","name":"Greyfall City Central Sector","maxPlayers":4}]` |
| `/api/rooms/create` | `POST` | Create a new private co-op lobby | `{"success":true,"roomCode":"ECL-412"}` |
| `/api/save` | `POST` | Persists player health, stamina, kills, and coordinates | `{"success":true,"message":"Game progress successfully saved to backend."}` |
| `/api/load` | `GET` | Retrieves saved player state from disk | `{"health":100,"stamina":100,"kills":5,"position":{"x":0,"y":0,"z":-50}}` |
| `/api/mcp/status` | `GET` | Proxies connection status to Unity Editor MCP bridge | `{"status":"offline","message":"Unity MCP Bridge not running on port 8080."}` |

#### B. Real-Time RFC 6455 WebSocket Service (`/ws`)
- **Transport:** Native binary framing without external npm dependencies.
- **Handshake:** RFC 6455 `Sec-WebSocket-Key` with SHA1 hash and base64 digest.
- **Client Synchronization:**
  - `player_sync`: Dispatches player $(x, y, z)$ position, 360° `bodyYaw`, `isSprinting`, `isAiming`, `health`, and active weapon.
  - `remote_player_update`: Broadcasts position and animation states to all other room members.
  - `player_fire`: Broadcasts gunshot audio, muzzle flash, and ballistic tracers.
  - `zombie_damage`: Synchronizes damage dealt to infected zombies across clients.
  - `ping` / `pong`: Round-trip latency calculation and HUD telemetry display.

---

### 3. Client-Side Connectivity Integration (`LocalPlayable/index.html`)

- **HUD Connectivity Badge:** Live top-bar pill showing connection status, active room code, and real-time millisecond ping:
  `● CONNECTED | CO-OP ROOM: ECL-709 | PING: 8ms`
- **Remote Ally Spawning:** When another player connects via another browser tab, mobile device on LAN, or second machine, an Ethan Cross ally 3D model is automatically spawned with smooth position and rotation interpolation.
- **Automated Reconnection:** Exponential backoff and auto-reconnect if connection drops.
- **Persistence:** Game progress automatically saves to `/api/save` upon sector clearance.

---

### 4. Verification Evidence
1. `GET http://localhost:5173/api/status` -> Returned `200 OK`, `status: online`.
2. `GET http://localhost:5173/api/rooms` -> Returned active room `ECL-709`.
3. `POST http://localhost:5173/api/save` -> Saved test payload to disk (`saves/savegame.json`).
4. `GET http://localhost:5173/api/load` -> Returned saved payload with timestamp.
5. `GET http://localhost:5173/api/mcp/status` -> Handled offline Unity Editor gracefully without crashing.

