# ECLIPSE: THE LAST CIVILIZATION
## Part 3 — Network & Systems Architecture Specification

### 1. Multiplayer Networking Stack
- **Framework:** Unity Netcode for GameObjects (NGO 2.2.0)
- **Transport:** Unity Transport (UTP 2.4.0)
- **Signaling & NAT Punchthrough:** Unity Relay (eliminates port-forwarding requirements for mobile cellular/Wi-Fi)
- **Session Discovery:** Unity Lobby (room creation, 2–4 players, 6-character room join codes)
- **Authentication:** Unity Authentication (anonymous guest token or linked ID)

---

### 2. Network Authority Topology

```mermaid
flowchart TD
    Host["Host Player (Authority)"] <--> Relay["Unity Relay Server"]
    Relay <--> Client1["Client Player 1"]
    Relay <--> Client2["Client Player 2"]
    Relay <--> Client3["Client Player 3"]

    subgraph HostResponsibilities [Host Validates]
        H1["Zombie AI Decisions & Pathing"]
        H2["Damage & Health Changes"]
        H3["Ammunition & Inventory Transactions"]
        H4["Mission Objective Completions"]
        H5["Settlement Resource Ledger"]
    end

    subgraph ClientResponsibilities [Clients Execute]
        C1["Local Prediction & Smooth Input"]
        C2["Camera & Touch UI Interactions"]
        C3["Audio Cues & Particle VFX"]
    end
```

---

### 3. Core Network Interfaces & Contracts
- `IPlayerIdentity`: Unique network client ID, display name, character preset, ping/latency.
- `INetworkAuthority`: Server/host authority validation contract for damage, loot, and progression.
- `IDamageable`: Networked health synchronization through `NetworkVariable<float>`.
- `IMissionParticipant`: Co-op mission participation and reward distribution.
