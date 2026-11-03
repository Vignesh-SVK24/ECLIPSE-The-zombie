# ECLIPSE: THE LAST CIVILIZATION
## Part 3 — Quality Assurance & Testing Plan

### 1. Verification Dimensions
1. **Network Connectivity & Room Management:**
   - Room creation (2–4 players) with generated join codes.
   - Cross-network join (Device A on Wi-Fi, Device B on 5G mobile data).
   - Graceful host departure and client reconnection attempts.
2. **Co-Op Gameplay Synchronization:**
   - Remote player position/rotation interpolation with zero jitter.
   - Authoritative damage delivery: Host confirms hit registration before applying damage.
   - Zombie AI synchronization: All clients observe synchronized zombie targets and states.
   - Downed player state and cooperative revive interaction.
3. **Mobile Performance Under Load:**
   - 4 connected players + 20 active zombies on mid-range Android & iOS.
   - Target 30 FPS baseline on lower-tier mobile, 45–60 FPS on high-tier.
   - Memory footprint tracking to ensure no out-of-memory crashes on 4GB devices.

---

### 2. Multi-Device Regression Test Matrix
| Scenario ID | Test Case | Target Metric | Expected Outcome |
|---|---|---|---|
| **NET-01** | Create Private Room | Join code generated | 6-character code created on Relay |
| **NET-02** | Join via Invitation Code | Valid handshake | Client enters room within 3.5s |
| **NET-03** | Max Capacity Rejection | 5th player join attempt | Room full error message displayed |
| **NET-04** | Sudden Disconnect | Force close on Client 2 | Host cleans up player within 5s |
| **NET-05** | Co-Op Zombie Fight | 4 players attack Brute | Health synced, 0 duplicated damage |
| **NET-06** | Teammate Revive | Player downed, revive hold | Revives at 30% HP after 3s interaction |
