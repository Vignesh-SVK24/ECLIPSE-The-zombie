# COLLISION AND OBSTACLE REPORT — ECLIPSE: THE LAST CIVILIZATION

## 1. Collision Architecture
- **Obstacle Registry**: Unified `mapObstacles` array registering 3D bounding boxes (`minX, maxX, minZ, maxZ`).
- **Entity Detection**: Circle-to-box collision resolution algorithm with penetration depth and normal calculation.
- **Player Collision Radius**: `0.45m`. Prevents penetration into vehicles, barriers, closed doors, and walls.
- **Zombie Collision Radius**: `0.42m`. Prevents zombie clipping through vehicles, barriers, and closed doors.

## 2. Tangential Sliding & Anti-Stuck Mechanics
- When an entity moves diagonally against an obstacle, movement is projected along the obstacle's surface tangent.
- Eliminates sticky collision artifacts and permits smooth navigation around vehicle hoods, fenders, and concrete corners.

## 3. Automated Validation Results
- **Inside Sedan Test**: Detected `collided: true`, obstacle `"Civilian Evacuation Sedan"`.
- **Open Street Test**: Detected `collided: false` (free movement).
- **Closed Gate Test**: Detected `collided: true`, obstacle `"Security Quarantine Gate"`.
- **Open Gate Test**: Detected `collided: false` (free passage through doorway).
