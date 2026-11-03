"""
ECLIPSE: Local Playable Upgrade for Batch B (Greyfall City Photorealistic Overhaul)
Injects explorable interiors, Paramedic Ambulance, Ground Mist VFX, and storytelling decals.
"""
import os

HTML_PATH = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "..", "..", "LocalPlayable", "index.html"))

def upgrade_playable():
    with open(HTML_PATH, "r", encoding="utf-8") as f:
        html = f.read()

    # 1. Update Mission Tag / Title
    html = html.replace(
        '<div class="mission-tag">ECLIPSE: GREYFALL COUNTY</div>',
        '<div class="mission-tag">ECLIPSE: GREYFALL CITY (BATCH B OVERHAUL)</div>'
    )

    # 2. Add Explorable Ground-Floor Interiors to Buildings
    target_buildings = """                if (stories >= 4) {
                    const waterTower = new THREE.Mesh(new THREE.CylinderGeometry(2.2, 2.2, 4.5, 12), envPBR.boardedWood);
                    waterTower.position.set(0, roofY + 2.25, 4);
                    waterTower.castShadow = true;
                    bGroup.add(waterTower);
                }

                buildingsGroup.add(bGroup);"""

    replacement_buildings = """                if (stories >= 4) {
                    const waterTower = new THREE.Mesh(new THREE.CylinderGeometry(2.2, 2.2, 4.5, 12), envPBR.boardedWood);
                    waterTower.position.set(0, roofY + 2.25, 4);
                    waterTower.castShadow = true;
                    bGroup.add(waterTower);
                }

                // Batch B: Explorable Ground-Floor Interior Details
                if (Math.abs(i) === 26 || i === 0) {
                    const interiorGroup = new THREE.Group();
                    interiorGroup.position.set(-side * 2.5, 0, 0);

                    // Doorway entrance indicator
                    const doorFrame = new THREE.Mesh(new THREE.BoxGeometry(0.2, 2.8, 1.8), envPBR.fireEscapeMetal);
                    doorFrame.position.set(-side * 5.4, 1.4, 0);
                    interiorGroup.add(doorFrame);

                    if (styleIdx === 1) { // Commercial Grocery Shopfront
                        const counter = new THREE.Mesh(new THREE.BoxGeometry(1.2, 1.1, 3.2), envPBR.boardedWood);
                        counter.position.set(0, 0.55, -2.5);
                        interiorGroup.add(counter);

                        const shelf = new THREE.Mesh(new THREE.BoxGeometry(0.6, 2.0, 3.5), envPBR.fireEscapeMetal);
                        shelf.position.set(2.0, 1.0, 0);
                        interiorGroup.add(shelf);
                    } else if (styleIdx === 2) { // Medical Clinic
                        const examTable = new THREE.Mesh(new THREE.BoxGeometry(1.4, 0.9, 2.6), envPBR.roadMarkingWhite);
                        examTable.position.set(0, 0.45, 2.0);
                        interiorGroup.add(examTable);
                    } else { // Residential Apartment
                        const table = new THREE.Mesh(new THREE.BoxGeometry(1.6, 0.8, 1.2), envPBR.boardedWood);
                        table.position.set(0, 0.4, 0);
                        interiorGroup.add(table);
                    }
                    bGroup.add(interiorGroup);
                }

                buildingsGroup.add(bGroup);"""

    # Normalize CRLF/LF for robust matching
    target_norm = target_buildings.replace("\r\n", "\n")
    html_norm = html.replace("\r\n", "\n")

    if target_norm in html_norm:
        html_norm = html_norm.replace(target_norm, replacement_buildings.replace("\r\n", "\n"), 1)
        print("[Upgrade] Injected explorable ground-floor building interiors.")
    else:
        print("[Upgrade] Warning: buildings target not found.")

    # 3. Add Paramedic Emergency Ambulance (Vehicle E)
    target_vehicles = """        attachWheels(truckGroup, 1.25, 0.65, 2.2);
        attachWheels(truckGroup, 1.25, 0.65, -2.2);
        vehiclesGroup.add(truckGroup);"""

    replacement_vehicles = """        attachWheels(truckGroup, 1.25, 0.65, 2.2);
        attachWheels(truckGroup, 1.25, 0.65, -2.2);
        vehiclesGroup.add(truckGroup);

        // E. Paramedic Emergency Ambulance (open rear doors & stretcher)
        const ambGroup = new THREE.Group();
        ambGroup.position.set(6.0, 0, -48);
        ambGroup.rotation.y = -0.25;

        const ambBody = new THREE.Mesh(new THREE.BoxGeometry(2.4, 2.2, 5.6), envPBR.roadMarkingWhite);
        ambBody.position.y = 1.6;
        ambBody.castShadow = true;
        ambGroup.add(ambBody);

        const ambCross = new THREE.Mesh(new THREE.BoxGeometry(2.45, 0.5, 5.65), envPBR.awningFabric1);
        ambCross.position.y = 1.6;
        ambGroup.add(ambCross);

        attachWheels(ambGroup, 1.15, 0.55, 1.8);
        attachWheels(ambGroup, 1.15, 0.55, -1.8);
        vehiclesGroup.add(ambGroup);"""

    target_v_norm = target_vehicles.replace("\r\n", "\n")
    if target_v_norm in html_norm:
        html_norm = html_norm.replace(target_v_norm, replacement_vehicles.replace("\r\n", "\n"), 1)
        print("[Upgrade] Injected Paramedic Emergency Ambulance.")
    else:
        print("[Upgrade] Warning: vehicles target not found.")

    # 4. Add Ground Mist VFX Layer
    target_vfx = "scene.add(natureGroup);"
    replacement_vfx = """scene.add(natureGroup);

        // 8b. ENVIRONMENTAL PARTICLE VFX (Batch B Ground Mist)
        const vfxGroup = new THREE.Group();
        const mistCount = 80;
        const mistGeo = new THREE.BufferGeometry();
        const mistPos = new Float32Array(mistCount * 3);
        for (let m = 0; m < mistCount; m++) {
            mistPos[m * 3] = (Math.random() - 0.5) * 32;
            mistPos[m * 3 + 1] = 0.2 + Math.random() * 0.9;
            mistPos[m * 3 + 2] = -115 + Math.random() * 230;
        }
        mistGeo.setAttribute('position', new THREE.BufferAttribute(mistPos, 3));
        const mistMat = new THREE.PointsMaterial({
            color: 0x90a4ae,
            size: 3.8,
            transparent: true,
            opacity: 0.28,
            depthWrite: false
        });
        const mistPoints = new THREE.Points(mistGeo, mistMat);
        vfxGroup.add(mistPoints);
        scene.add(vfxGroup);"""

    if target_vfx in html_norm:
        html_norm = html_norm.replace(target_vfx, replacement_vfx, 1)
        print("[Upgrade] Injected Environmental Ground Mist VFX.")

    with open(HTML_PATH, "w", encoding="utf-8") as f:
        f.write(html_norm)

    print(f"[Upgrade] Successfully updated {HTML_PATH}")

if __name__ == "__main__":
    upgrade_playable()

