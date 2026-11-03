using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Eclipse.Damage;
using Eclipse.AI;

namespace Eclipse.Environment
{
    /// <summary>
    /// Master procedural environment builder for Greyfall City (Batch B).
    /// Constructs a photorealistic, cinematic, post-apocalyptic urban zombie survival environment
    /// featuring multi-lane roadways, weathered modular buildings, abandoned vehicles,
    /// military quarantine checkpoints, overgrown vegetation, atmospheric lighting, and particle VFX.
    /// Fully optimized for mobile (Android/iOS) with LODs and efficient collision geometry.
    /// </summary>
    public class GreyfallCityEnvironmentBuilder : MonoBehaviour
    {
        [Header("Master Scene Configuration")]
        [SerializeField] private bool _buildOnStart = false;
        [SerializeField] private AtmospherePreset _initialAtmosphere = AtmospherePreset.OvercastMorning;

        [Header("Root Group References")]
        [SerializeField] private Transform _environmentRoot;
        [SerializeField] private Transform _roadsRoot;
        [SerializeField] private Transform _buildingsRoot;
        [SerializeField] private Transform _vehiclesRoot;
        [SerializeField] private Transform _checkpointRoot;
        [SerializeField] private Transform _natureRoot;
        [SerializeField] private Transform _infrastructureRoot;
        [SerializeField] private Transform _vfxRoot;
        [SerializeField] private Transform _gameplayRoot;

        // Shared PBR Material Cache
        private Material _asphaltMat;
        private Material _asphaltWetMat;
        private Material _yellowMarkingMat;
        private Material _whiteMarkingMat;
        private Material _concreteCurbsMat;
        private Material _sidewalkMat;
        private Material _brickFacadeMat;
        private Material _concreteFacadeMat;
        private Material _plasterFacadeMat;
        private Material _windowGlassMat;
        private Material _boardedWoodMat;
        private Material _rustedMetalMat;
        private Material _policeCarMat;
        private Material _sedanCarMat;
        private Material _truckMat;
        private Material _ambulanceMat;
        private Material _tireRubberMat;
        private Material _jerseyBarrierMat;
        private Material _sandbagMat;
        private Material _woodCrateMat;
        private Material _hazardSignMat;
        private Material _foliageMat;
        private Material _treeBarkMat;
        private Material _emissiveLampMat;
        private Material _bloodDecalMat;

        private void Start()
        {
            if (_buildOnStart)
            {
                ConstructCompleteGreyfallCity();
            }
        }

        [ContextMenu("Construct Complete Greyfall City (Batch B)")]
        public void ConstructCompleteGreyfallCity()
        {
            InitializeMaterialLibrary();
            CreateRootHierarchies();

            BuildRoadNetworkAndPavements();
            BuildMunicipalInfrastructure();
            BuildModularBuildingsAndInteriors();
            BuildAbandonedVehiclesFleet();
            BuildMilitaryQuarantineCheckpoint();
            BuildStorytellingVignettes();
            BuildOvergrownNatureAndVegetation();
            BuildEnvironmentalParticleVFX();
            BuildCinematicAtmosphereAndLighting();
            BuildGameplaySpawnersAndNavBounds();

            Debug.Log("<color=#C49B58><b>[ECLIPSE] Greyfall City (Batch B) Environment successfully constructed and configured.</b></color>");
        }

        #region 1. MATERIAL LIBRARY INITIALIZATION

        private void InitializeMaterialLibrary()
        {
            Shader uLitShader = Shader.Find("Universal Render Pipeline/Lit");
            if (uLitShader == null) uLitShader = Shader.Find("Standard");

            Shader unlitShader = Shader.Find("Universal Render Pipeline/Unlit");
            if (unlitShader == null) unlitShader = Shader.Find("Unlit/Color");

            // Road & Ground PBR
            _asphaltMat = CreatePBRMaterial(uLitShader, "M_Road_Asphalt", new Color(0.12f, 0.13f, 0.12f), 0.88f, 0.04f);
            _asphaltWetMat = CreatePBRMaterial(uLitShader, "M_Road_Asphalt_Wet", new Color(0.06f, 0.07f, 0.06f), 0.12f, 0.35f);
            _yellowMarkingMat = CreatePBRMaterial(uLitShader, "M_Road_Marking_Yellow", new Color(0.72f, 0.55f, 0.16f), 0.65f, 0.0f);
            _whiteMarkingMat = CreatePBRMaterial(uLitShader, "M_Road_Marking_White", new Color(0.82f, 0.84f, 0.82f), 0.70f, 0.0f);
            _concreteCurbsMat = CreatePBRMaterial(uLitShader, "M_Concrete_Curbs", new Color(0.28f, 0.30f, 0.29f), 0.85f, 0.0f);
            _sidewalkMat = CreatePBRMaterial(uLitShader, "M_Sidewalk_Tiles", new Color(0.22f, 0.24f, 0.23f), 0.90f, 0.0f);

            // Architecture PBR
            _brickFacadeMat = CreatePBRMaterial(uLitShader, "M_Brick_Weathered", new Color(0.44f, 0.22f, 0.18f), 0.92f, 0.0f);
            _concreteFacadeMat = CreatePBRMaterial(uLitShader, "M_Concrete_Facade", new Color(0.30f, 0.32f, 0.31f), 0.88f, 0.02f);
            _plasterFacadeMat = CreatePBRMaterial(uLitShader, "M_Clinic_Plaster", new Color(0.48f, 0.46f, 0.42f), 0.85f, 0.0f);
            _windowGlassMat = CreatePBRMaterial(uLitShader, "M_Window_Glass", new Color(0.10f, 0.16f, 0.22f), 0.15f, 0.85f);
            _boardedWoodMat = CreatePBRMaterial(uLitShader, "M_Boarded_Wood", new Color(0.35f, 0.24f, 0.14f), 0.86f, 0.0f);
            _rustedMetalMat = CreatePBRMaterial(uLitShader, "M_Rusted_Metal", new Color(0.22f, 0.18f, 0.16f), 0.75f, 0.65f);

            // Vehicles PBR
            _policeCarMat = CreatePBRMaterial(uLitShader, "M_Veh_Police", new Color(0.08f, 0.10f, 0.12f), 0.35f, 0.70f);
            _sedanCarMat = CreatePBRMaterial(uLitShader, "M_Veh_Sedan", new Color(0.32f, 0.18f, 0.16f), 0.48f, 0.45f);
            _truckMat = CreatePBRMaterial(uLitShader, "M_Veh_Truck", new Color(0.18f, 0.22f, 0.18f), 0.72f, 0.30f);
            _ambulanceMat = CreatePBRMaterial(uLitShader, "M_Veh_Ambulance", new Color(0.85f, 0.86f, 0.85f), 0.40f, 0.50f);
            _tireRubberMat = CreatePBRMaterial(uLitShader, "M_Tire_Rubber", new Color(0.08f, 0.08f, 0.08f), 0.95f, 0.0f);

            // Storytelling & Props PBR
            _jerseyBarrierMat = CreatePBRMaterial(uLitShader, "M_Jersey_Barrier", new Color(0.34f, 0.36f, 0.35f), 0.90f, 0.0f);
            _sandbagMat = CreatePBRMaterial(uLitShader, "M_Sandbag_Burlap", new Color(0.46f, 0.40f, 0.30f), 0.95f, 0.0f);
            _woodCrateMat = CreatePBRMaterial(uLitShader, "M_Military_Crate", new Color(0.32f, 0.26f, 0.18f), 0.84f, 0.0f);
            _hazardSignMat = CreatePBRMaterial(uLitShader, "M_Hazard_Sign", new Color(0.85f, 0.60f, 0.10f), 0.45f, 0.40f);
            _emissiveLampMat = CreatePBRMaterial(unlitShader, "M_Emissive_Lamp", new Color(1.0f, 0.88f, 0.65f), 0.0f, 0.0f);
            _bloodDecalMat = CreatePBRMaterial(uLitShader, "M_Blood_Decal", new Color(0.25f, 0.04f, 0.04f), 0.30f, 0.10f);

            // Nature PBR
            _foliageMat = CreatePBRMaterial(uLitShader, "M_Foliage_Overgrowth", new Color(0.18f, 0.28f, 0.16f), 0.82f, 0.0f);
            _treeBarkMat = CreatePBRMaterial(uLitShader, "M_Tree_Bark", new Color(0.20f, 0.15f, 0.10f), 0.92f, 0.0f);
        }

        private Material CreatePBRMaterial(Shader shader, string name, Color color, float roughness, float metallic)
        {
            Material mat = new Material(shader) { name = name };
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
            else if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);

            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", 1.0f - roughness);
            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", metallic);
            return mat;
        }

        private void CreateRootHierarchies()
        {
            if (_environmentRoot == null)
            {
                var envGo = GameObject.Find("[ENVIRONMENT]");
                _environmentRoot = envGo != null ? envGo.transform : new GameObject("[ENVIRONMENT]").transform;
            }

            _roadsRoot = GetOrCreateSubGroup(_environmentRoot, "01_Roads_And_Sidewalks");
            _infrastructureRoot = GetOrCreateSubGroup(_environmentRoot, "02_Municipal_Infrastructure");
            _buildingsRoot = GetOrCreateSubGroup(_environmentRoot, "03_Buildings_And_Interiors");
            _vehiclesRoot = GetOrCreateSubGroup(_environmentRoot, "04_Abandoned_Vehicles");
            _checkpointRoot = GetOrCreateSubGroup(_environmentRoot, "05_Quarantine_Checkpoint");
            _natureRoot = GetOrCreateSubGroup(_environmentRoot, "06_Reclaiming_Nature");
            _vfxRoot = GetOrCreateSubGroup(_environmentRoot, "07_Environmental_VFX");
            _gameplayRoot = GetOrCreateSubGroup(_environmentRoot, "08_Gameplay_And_Navigation");
        }

        private Transform GetOrCreateSubGroup(Transform parent, string name)
        {
            var child = parent.Find(name);
            if (child != null) return child;
            var go = new GameObject(name);
            go.transform.SetParent(parent);
            return go.transform;
        }

        #endregion

        #region 2. ROADS, SIDEWALKS & MUNICIPAL INFRASTRUCTURE

        private void BuildRoadNetworkAndPavements()
        {
            // Clear existing if any
            for (int i = _roadsRoot.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(_roadsRoot.GetChild(i).gameObject);
            }

            // Main Asphalt Roadway (32m wide, 240m long)
            GameObject roadGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            roadGo.name = "SM_Road_Avenue_32x240m";
            roadGo.transform.SetParent(_roadsRoot);
            roadGo.transform.position = new Vector3(0, -0.1f, 0);
            roadGo.transform.localScale = new Vector3(32f, 0.2f, 240f);
            roadGo.GetComponent<Renderer>().sharedMaterial = _asphaltMat;
            roadGo.isStatic = true;

            // Road Material Controller
            var rmc = roadGo.AddComponent<RoadMaterialController>();

            // Double Yellow Center Lines (with realistic gaps)
            for (float side = -0.15f; side <= 0.16f; side += 0.30f)
            {
                for (float z = -115f; z <= 115f; z += 14f)
                {
                    GameObject yellowLine = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    yellowLine.name = $"YellowLine_Gap_{z:F0}";
                    yellowLine.transform.SetParent(_roadsRoot);
                    yellowLine.transform.position = new Vector3(side, 0.005f, z);
                    yellowLine.transform.localScale = new Vector3(0.14f, 0.01f, 11f);
                    yellowLine.GetComponent<Renderer>().sharedMaterial = _yellowMarkingMat;
                    DestroyImmediate(yellowLine.GetComponent<Collider>());
                    yellowLine.isStatic = true;
                }
            }

            // White Dashed Lane Dividers (Lane 1 and Lane 2)
            foreach (float xLane in new float[] { -6f, 6f })
            {
                for (float z = -110f; z <= 110f; z += 16f)
                {
                    GameObject whiteDash = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    whiteDash.name = $"WhiteDash_{xLane}_{z:F0}";
                    whiteDash.transform.SetParent(_roadsRoot);
                    whiteDash.transform.position = new Vector3(xLane, 0.005f, z);
                    whiteDash.transform.localScale = new Vector3(0.18f, 0.01f, 5.5f);
                    whiteDash.GetComponent<Renderer>().sharedMaterial = _whiteMarkingMat;
                    DestroyImmediate(whiteDash.GetComponent<Collider>());
                    whiteDash.isStatic = true;
                }
            }

            // Pedestrian Zebra Crosswalks at Intersections (z = -32 and z = 32)
            foreach (float zPos in new float[] { -32f, 32f })
            {
                for (float x = -13f; x <= 13f; x += 2.2f)
                {
                    GameObject zebra = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    zebra.name = $"ZebraStripe_{zPos}_{x:F0}";
                    zebra.transform.SetParent(_roadsRoot);
                    zebra.transform.position = new Vector3(x, 0.006f, zPos);
                    zebra.transform.localScale = new Vector3(1.2f, 0.01f, 4.2f);
                    zebra.GetComponent<Renderer>().sharedMaterial = _whiteMarkingMat;
                    DestroyImmediate(zebra.GetComponent<Collider>());
                    zebra.isStatic = true;
                }

                // Stop line
                GameObject stopLine = GameObject.CreatePrimitive(PrimitiveType.Cube);
                stopLine.name = $"StopLine_{zPos}";
                stopLine.transform.SetParent(_roadsRoot);
                stopLine.transform.position = new Vector3(0, 0.006f, zPos + (zPos < 0 ? -4f : 4f));
                stopLine.transform.localScale = new Vector3(28f, 0.01f, 0.5f);
                stopLine.GetComponent<Renderer>().sharedMaterial = _whiteMarkingMat;
                DestroyImmediate(stopLine.GetComponent<Collider>());
                stopLine.isStatic = true;
            }

            // Wet Asphalt Rain Puddles & Potholes
            var puddleConfigs = new (Vector3 pos, Vector3 scale)[]
            {
                (new Vector3(-4f, 0.008f, -18f), new Vector3(3.5f, 0.01f, 5.0f)),
                (new Vector3(6.5f, 0.008f, 12f), new Vector3(4.2f, 0.01f, 6.2f)),
                (new Vector3(-8f, 0.008f, 55f), new Vector3(4.0f, 0.01f, 4.0f)),
                (new Vector3(3f, 0.008f, -75f), new Vector3(4.5f, 0.01f, 6.5f))
            };

            foreach (var p in puddleConfigs)
            {
                GameObject puddle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                puddle.name = $"Puddle_Pothole_{p.pos.z:F0}";
                puddle.transform.SetParent(_roadsRoot);
                puddle.transform.position = p.pos;
                puddle.transform.localScale = new Vector3(p.scale.x, 0.005f, p.scale.z);
                puddle.GetComponent<Renderer>().sharedMaterial = _asphaltWetMat;
                DestroyImmediate(puddle.GetComponent<Collider>());
                puddle.isStatic = true;
            }

            // Concrete Sidewalks & Raised Curbs (Left & Right Sides)
            foreach (float side in new float[] { -1f, 1f })
            {
                float xBase = side * 16f;

                // Raised Curb
                GameObject curb = GameObject.CreatePrimitive(PrimitiveType.Cube);
                curb.name = $"Curb_Stone_{(side < 0 ? "Left" : "Right")}";
                curb.transform.SetParent(_roadsRoot);
                curb.transform.position = new Vector3(xBase, 0.175f, 0);
                curb.transform.localScale = new Vector3(0.5f, 0.35f, 240f);
                curb.GetComponent<Renderer>().sharedMaterial = _concreteCurbsMat;
                curb.isStatic = true;

                // Pedestrian Sidewalk Deck
                GameObject swDeck = GameObject.CreatePrimitive(PrimitiveType.Cube);
                swDeck.name = $"Sidewalk_Deck_{(side < 0 ? "Left" : "Right")}";
                swDeck.transform.SetParent(_roadsRoot);
                swDeck.transform.position = new Vector3(xBase + side * 4.5f, 0.15f, 0);
                swDeck.transform.localScale = new Vector3(8.5f, 0.30f, 240f);
                swDeck.GetComponent<Renderer>().sharedMaterial = _sidewalkMat;
                swDeck.isStatic = true;

                // Storm Drain Grates Inset
                foreach (float zGrate in new float[] { -45f, 0f, 45f, 90f })
                {
                    GameObject drain = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    drain.name = $"StormDrain_{side}_{zGrate}";
                    drain.transform.SetParent(_roadsRoot);
                    drain.transform.position = new Vector3(xBase - side * 0.35f, 0.02f, zGrate);
                    drain.transform.localScale = new Vector3(0.8f, 0.04f, 1.8f);
                    drain.GetComponent<Renderer>().sharedMaterial = _rustedMetalMat;
                    drain.isStatic = true;
                }
            }
        }

        private void BuildMunicipalInfrastructure()
        {
            for (int i = _infrastructureRoot.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(_infrastructureRoot.GetChild(i).gameObject);
            }

            // Cobra Head Municipal Streetlights
            var streetLightZ = new float[] { -85f, -40f, 0f, 40f, 85f };
            foreach (float z in streetLightZ)
            {
                foreach (float side in new float[] { -1f, 1f })
                {
                    CreateStreetlight(new Vector3(side * 15.6f, 0.35f, z), side);
                }
            }

            // Overhead Traffic Light Gantries at Intersections (-32 and 32)
            foreach (float zGantry in new float[] { -32f, 32f })
            {
                CreateTrafficSignalGantry(zGantry);
            }

            // Fire Hydrants & Street Signs
            CreateFireHydrant(new Vector3(-15.3f, 0.30f, -12f));
            CreateFireHydrant(new Vector3(15.3f, 0.30f, 65f));

            CreateStreetNameSign(new Vector3(-15.2f, 0.30f, -31f), "GREYFALL AVE / 4TH ST");
            CreateStreetNameSign(new Vector3(15.2f, 0.30f, 33f), "GREYFALL AVE / EVACUATION ROUTE");
        }

        private void CreateStreetlight(Vector3 position, float side)
        {
            GameObject lampRoot = new GameObject($"Streetlamp_{position.z:F0}_{(side < 0 ? "L" : "R")}");
            lampRoot.transform.SetParent(_infrastructureRoot);
            lampRoot.transform.position = position;

            // Pole
            GameObject pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pole.name = "Pole";
            pole.transform.SetParent(lampRoot.transform);
            pole.transform.localPosition = new Vector3(0, 4.0f, 0);
            pole.transform.localScale = new Vector3(0.22f, 4.0f, 0.22f);
            pole.GetComponent<Renderer>().sharedMaterial = _rustedMetalMat;

            // Overhang Arm
            GameObject arm = GameObject.CreatePrimitive(PrimitiveType.Cube);
            arm.name = "Arm";
            arm.transform.SetParent(lampRoot.transform);
            arm.transform.localPosition = new Vector3(-side * 1.2f, 7.9f, 0);
            arm.transform.localScale = new Vector3(2.4f, 0.15f, 0.20f);
            arm.GetComponent<Renderer>().sharedMaterial = _rustedMetalMat;

            // Fixture Head
            GameObject fixture = GameObject.CreatePrimitive(PrimitiveType.Cube);
            fixture.name = "FixtureHead";
            fixture.transform.SetParent(lampRoot.transform);
            fixture.transform.localPosition = new Vector3(-side * 2.2f, 7.8f, 0);
            fixture.transform.localScale = new Vector3(0.8f, 0.2f, 0.45f);
            fixture.GetComponent<Renderer>().sharedMaterial = _emissiveLampMat;

            // Point Light (warm flickering amber street lighting)
            GameObject lightGo = new GameObject("StreetLight_Source");
            lightGo.transform.SetParent(lampRoot.transform);
            lightGo.transform.localPosition = new Vector3(-side * 2.2f, 7.5f, 0);
            Light l = lightGo.AddComponent<Light>();
            l.type = LightType.Point;
            l.color = new Color(1.0f, 0.82f, 0.55f);
            l.intensity = 2.2f;
            l.range = 18f;
            l.shadows = LightShadows.None; // Mobile optimization
        }

        private void CreateTrafficSignalGantry(float zPos)
        {
            GameObject gantryRoot = new GameObject($"TrafficGantry_{zPos:F0}");
            gantryRoot.transform.SetParent(_infrastructureRoot);
            gantryRoot.transform.position = new Vector3(0, 0, zPos);

            // Left & Right Support Pillars
            foreach (float side in new float[] { -15.5f, 15.5f })
            {
                GameObject post = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                post.name = $"GantryPillar_{side}";
                post.transform.SetParent(gantryRoot.transform);
                post.transform.position = new Vector3(side, 4.2f, zPos);
                post.transform.localScale = new Vector3(0.35f, 4.2f, 0.35f);
                post.GetComponent<Renderer>().sharedMaterial = _rustedMetalMat;
            }

            // Cross Beam
            GameObject beam = GameObject.CreatePrimitive(PrimitiveType.Cube);
            beam.name = "CrossBeam";
            beam.transform.SetParent(gantryRoot.transform);
            beam.transform.position = new Vector3(0, 8.2f, zPos);
            beam.transform.localScale = new Vector3(31.5f, 0.45f, 0.45f);
            beam.GetComponent<Renderer>().sharedMaterial = _rustedMetalMat;

            // Hanging Signal Boxes
            foreach (float xSignal in new float[] { -8f, -2f, 4f, 10f })
            {
                GameObject signalBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
                signalBox.name = $"SignalHead_{xSignal}";
                signalBox.transform.SetParent(gantryRoot.transform);
                signalBox.transform.position = new Vector3(xSignal, 7.4f, zPos);
                signalBox.transform.localScale = new Vector3(0.5f, 1.2f, 0.4f);
                signalBox.GetComponent<Renderer>().sharedMaterial = _hazardSignMat;
            }
        }

        private void CreateFireHydrant(Vector3 position)
        {
            GameObject hydrant = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            hydrant.name = "SM_FireHydrant";
            hydrant.transform.SetParent(_infrastructureRoot);
            hydrant.transform.position = position + new Vector3(0, 0.45f, 0);
            hydrant.transform.localScale = new Vector3(0.35f, 0.45f, 0.35f);
            hydrant.GetComponent<Renderer>().sharedMaterial = _bloodDecalMat;
        }

        private void CreateStreetNameSign(Vector3 position, string signText)
        {
            GameObject signRoot = new GameObject($"StreetSign_{signText.Replace("/", "_")}");
            signRoot.transform.SetParent(_infrastructureRoot);
            signRoot.transform.position = position;

            GameObject post = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            post.transform.SetParent(signRoot.transform);
            post.transform.localPosition = new Vector3(0, 1.4f, 0);
            post.transform.localScale = new Vector3(0.08f, 1.4f, 0.08f);
            post.GetComponent<Renderer>().sharedMaterial = _rustedMetalMat;

            GameObject blade = GameObject.CreatePrimitive(PrimitiveType.Cube);
            blade.transform.SetParent(signRoot.transform);
            blade.transform.localPosition = new Vector3(0, 2.7f, 0);
            blade.transform.localScale = new Vector3(1.1f, 0.22f, 0.04f);
            blade.GetComponent<Renderer>().sharedMaterial = _concreteFacadeMat;
        }

        #endregion

        #region 3. MODULAR BUILDINGS & EXPLORABLE INTERIORS

        private void BuildModularBuildingsAndInteriors()
        {
            for (int i = _buildingsRoot.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(_buildingsRoot.GetChild(i).gameObject);
            }

            var buildingStyles = new (BuildingStyle style, Material mat)[]
            {
                (BuildingStyle.ResidentialApartment, _brickFacadeMat),
                (BuildingStyle.CommercialShopfront, _concreteFacadeMat),
                (BuildingStyle.MedicalClinic, _plasterFacadeMat)
            };

            for (float z = -105f; z <= 105f; z += 26f)
            {
                foreach (float side in new float[] { -1f, 1f })
                {
                    float xPos = side * 30f;
                    int styleIdx = Mathf.Abs((int)(z / 26f) + (side > 0 ? 1 : 0)) % buildingStyles.Length;
                    var style = buildingStyles[styleIdx];
                    int stories = 3 + (Mathf.Abs((int)z) % 3);

                    bool isExplorable = (Mathf.Abs(z) == 26f || Mathf.Abs(z) == 0f);

                    CreateModularBuilding(new Vector3(xPos, 0, z), side, style.style, style.mat, stories, isExplorable);
                }
            }
        }

        private void CreateModularBuilding(Vector3 position, float side, BuildingStyle style, Material facadeMat, int stories, bool isExplorable)
        {
            float bHeight = stories * 4.2f;
            GameObject bRoot = new GameObject($"Bld_{style}_{position.z:F0}_{(side < 0 ? "L" : "R")}");
            bRoot.transform.SetParent(_buildingsRoot);
            bRoot.transform.position = position;

            // Main Building Shell
            GameObject shell = GameObject.CreatePrimitive(PrimitiveType.Cube);
            shell.name = "Structural_Shell";
            shell.transform.SetParent(bRoot.transform);
            shell.transform.localPosition = new Vector3(0, bHeight * 0.5f, 0);
            shell.transform.localScale = new Vector3(16f, bHeight, 22f);
            shell.GetComponent<Renderer>().sharedMaterial = facadeMat;
            shell.isStatic = true;

            // Building Piece component
            var piece = bRoot.AddComponent<ModularBuildingPiece>();
            piece.ConfigureBuilding(style, BuildingDamageState.BrokenWindows, stories);

            // Windows & Broken Panes on Street Front Face
            float frontFaceX = -side * 8.05f;
            for (int floor = 1; floor <= stories; floor++)
            {
                float yFloor = (floor - 0.5f) * 4.0f + 1.2f;
                for (float zWin = -7f; zWin <= 7f; zWin += 4.5f)
                {
                    bool isBroken = ((int)(position.z * zWin + floor) % 5 == 0);
                    Material winMat = isBroken ? _boardedWoodMat : _windowGlassMat;

                    GameObject winPane = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    winPane.name = $"Window_F{floor}_{zWin:F0}_{(isBroken ? "Boarded" : "Glass")}";
                    winPane.transform.SetParent(bRoot.transform);
                    winPane.transform.localPosition = new Vector3(frontFaceX, yFloor, zWin);
                    winPane.transform.localScale = new Vector3(0.08f, 2.0f, 1.4f);
                    winPane.GetComponent<Renderer>().sharedMaterial = winMat;
                    DestroyImmediate(winPane.GetComponent<Collider>());
                }
            }

            // Metal Fire Escape (Residential Apartments)
            if (style == BuildingStyle.ResidentialApartment && stories >= 3)
            {
                float feX = frontFaceX - side * 0.8f;
                for (int f = 1; f < stories; f++)
                {
                    GameObject feBalcony = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    feBalcony.name = $"FireEscape_Balcony_F{f}";
                    feBalcony.transform.SetParent(bRoot.transform);
                    feBalcony.transform.localPosition = new Vector3(feX, f * 4.0f, 3.5f);
                    feBalcony.transform.localScale = new Vector3(1.4f, 0.1f, 4.2f);
                    feBalcony.GetComponent<Renderer>().sharedMaterial = _rustedMetalMat;

                    // Ladder
                    GameObject feLadder = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    feLadder.name = $"FireEscape_Ladder_F{f}";
                    feLadder.transform.SetParent(bRoot.transform);
                    feLadder.transform.localPosition = new Vector3(feX, f * 4.0f - 1.9f, 5.0f);
                    feLadder.transform.localScale = new Vector3(0.2f, 3.8f, 0.4f);
                    feLadder.GetComponent<Renderer>().sharedMaterial = _rustedMetalMat;
                }
            }

            // Rooftop Utilities (AC Condensers & Water Towers)
            GameObject acUnit = GameObject.CreatePrimitive(PrimitiveType.Cube);
            acUnit.name = "Rooftop_AC_Condenser";
            acUnit.transform.SetParent(bRoot.transform);
            acUnit.transform.localPosition = new Vector3(0, bHeight + 0.9f, -4f);
            acUnit.transform.localScale = new Vector3(2.2f, 1.8f, 2.8f);
            acUnit.GetComponent<Renderer>().sharedMaterial = _rustedMetalMat;

            if (stories >= 4)
            {
                GameObject waterTower = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                waterTower.name = "Rooftop_WaterTower";
                waterTower.transform.SetParent(bRoot.transform);
                waterTower.transform.localPosition = new Vector3(0, bHeight + 2.25f, 4f);
                waterTower.transform.localScale = new Vector3(2.2f, 2.25f, 2.2f);
                waterTower.GetComponent<Renderer>().sharedMaterial = _boardedWoodMat;
            }

            // Explorable Ground-Floor Interior
            if (isExplorable)
            {
                BuildInteriorExplorationZone(bRoot.transform, side, style, frontFaceX);
            }

            // Mobile LOD Group
            var lodGroup = bRoot.AddComponent<LODGroup>();
            var renderers = bRoot.GetComponentsInChildren<Renderer>();
            LOD[] lods = new LOD[2];
            lods[0] = new LOD(0.25f, renderers);
            lods[1] = new LOD(0.04f, new Renderer[] { shell.GetComponent<Renderer>() });
            lodGroup.SetLODs(lods);
            lodGroup.RecalculateBounds();
        }

        private void BuildInteriorExplorationZone(Transform buildingParent, float side, BuildingStyle style, float frontFaceX)
        {
            GameObject interiorRoot = new GameObject("Explorable_Interior_GroundFloor");
            interiorRoot.transform.SetParent(buildingParent);
            interiorRoot.transform.localPosition = new Vector3(-side * 2.5f, 0, 0);

            // Doorway Opening Indicator
            GameObject doorFrame = GameObject.CreatePrimitive(PrimitiveType.Cube);
            doorFrame.name = "Entrance_DoorFrame";
            doorFrame.transform.SetParent(interiorRoot.transform);
            doorFrame.transform.localPosition = new Vector3(-side * 5.4f, 1.4f, 0);
            doorFrame.transform.localScale = new Vector3(0.2f, 2.8f, 1.8f);
            doorFrame.GetComponent<Renderer>().sharedMaterial = _rustedMetalMat;

            // Interior Props based on style
            if (style == BuildingStyle.CommercialShopfront)
            {
                // Grocery Shelves & Checkout Counter
                GameObject counter = GameObject.CreatePrimitive(PrimitiveType.Cube);
                counter.name = "Shop_CheckoutCounter";
                counter.transform.SetParent(interiorRoot.transform);
                counter.transform.localPosition = new Vector3(0, 0.6f, -3f);
                counter.transform.localScale = new Vector3(1.2f, 1.2f, 3.5f);
                counter.GetComponent<Renderer>().sharedMaterial = _boardedWoodMat;

                // Loot Supply Crate
                CreateLootContainer(interiorRoot.transform, new Vector3(2f, 0.4f, -3f), "Shop_LootCrate");
            }
            else if (style == BuildingStyle.MedicalClinic)
            {
                // Triage Examination Table
                GameObject examTable = GameObject.CreatePrimitive(PrimitiveType.Cube);
                examTable.name = "Clinic_ExamTable";
                examTable.transform.SetParent(interiorRoot.transform);
                examTable.transform.localPosition = new Vector3(0, 0.5f, 2f);
                examTable.transform.localScale = new Vector3(1.4f, 1.0f, 2.8f);
                examTable.GetComponent<Renderer>().sharedMaterial = _plasterFacadeMat;

                // Medical Loot Box
                CreateLootContainer(interiorRoot.transform, new Vector3(-1.5f, 0.4f, 2f), "Clinic_MedicalSupplies");
            }
            else
            {
                // Abandoned Residential Living Area
                GameObject table = GameObject.CreatePrimitive(PrimitiveType.Cube);
                table.name = "Apt_DamagedTable";
                table.transform.SetParent(interiorRoot.transform);
                table.transform.localPosition = new Vector3(0, 0.45f, 0);
                table.transform.localScale = new Vector3(1.8f, 0.9f, 1.2f);
                table.GetComponent<Renderer>().sharedMaterial = _boardedWoodMat;

                CreateLootContainer(interiorRoot.transform, new Vector3(1.8f, 0.4f, 1.5f), "Apt_SurvivorLootStash");
            }
        }

        private void CreateLootContainer(Transform parent, Vector3 localPos, string lootName)
        {
            GameObject crate = GameObject.CreatePrimitive(PrimitiveType.Cube);
            crate.name = lootName;
            crate.tag = "Respawn"; // Interactable / Loot tag
            crate.transform.SetParent(parent);
            crate.transform.localPosition = localPos;
            crate.transform.localScale = new Vector3(0.9f, 0.8f, 0.9f);
            crate.GetComponent<Renderer>().sharedMaterial = _woodCrateMat;

            // Subtle interaction light
            GameObject lootLight = new GameObject("Loot_Glow");
            lootLight.transform.SetParent(crate.transform);
            lootLight.transform.localPosition = new Vector3(0, 0.6f, 0);
            Light l = lootLight.AddComponent<Light>();
            l.type = LightType.Point;
            l.color = new Color(0.85f, 0.70f, 0.25f);
            l.intensity = 0.8f;
            l.range = 3.5f;
            l.shadows = LightShadows.None;
        }

        #endregion

        #region 4. ABANDONED VEHICLES FLEET

        private void BuildAbandonedVehiclesFleet()
        {
            for (int i = _vehiclesRoot.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(_vehiclesRoot.GetChild(i).gameObject);
            }

            // A. Abandoned Police Cruiser (crashed diagonally with active red emergency beacon)
            CreatePoliceCruiser(new Vector3(-4.5f, 0, 16f), 22f);

            // B. Civilian Evacuation Sedan (stopped with roof luggage)
            CreateEvacuationSedan(new Vector3(5.5f, 0, -14f), -30f);

            // C. Heavy Military Transport Cargo Truck (blocking lane)
            CreateMilitaryTruck(new Vector3(-6.5f, 0, 48f), 10f);

            // D. Paramedic Emergency Ambulance (open rear doors)
            CreateAmbulance(new Vector3(6.0f, 0, -52f), -15f);

            // E. Crashed Motorcycle on Sidewalk Curb
            CreateCrashedMotorcycle(new Vector3(-14.5f, 0.25f, -2f));
        }

        private void CreatePoliceCruiser(Vector3 position, float rotY)
        {
            GameObject policeGo = new GameObject("SM_Veh_PoliceCruiser_Damaged");
            policeGo.transform.SetParent(_vehiclesRoot);
            policeGo.transform.position = position;
            policeGo.transform.rotation = Quaternion.Euler(0, rotY, 0);

            // Chassis
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "Body";
            body.transform.SetParent(policeGo.transform);
            body.transform.localPosition = new Vector3(0, 0.85f, 0);
            body.transform.localScale = new Vector3(2.2f, 0.9f, 4.8f);
            body.GetComponent<Renderer>().sharedMaterial = _policeCarMat;

            // Cabin
            GameObject cabin = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cabin.name = "Cabin";
            cabin.transform.SetParent(policeGo.transform);
            cabin.transform.localPosition = new Vector3(0, 1.65f, -0.2f);
            cabin.transform.localScale = new Vector3(1.9f, 0.85f, 2.4f);
            cabin.GetComponent<Renderer>().sharedMaterial = _windowGlassMat;

            // Open Driver Door
            GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
            door.name = "OpenDriverDoor";
            door.transform.SetParent(policeGo.transform);
            door.transform.localPosition = new Vector3(-1.45f, 0.95f, 0.2f);
            door.transform.localRotation = Quaternion.Euler(0, -35f, 0);
            door.transform.localScale = new Vector3(0.12f, 0.85f, 1.1f);
            door.GetComponent<Renderer>().sharedMaterial = _policeCarMat;

            // Emergency Light Bar & Flashing Red Siren
            GameObject lightBar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lightBar.name = "LightBar";
            lightBar.transform.SetParent(policeGo.transform);
            lightBar.transform.localPosition = new Vector3(0, 2.15f, -0.2f);
            lightBar.transform.localScale = new Vector3(1.4f, 0.18f, 0.35f);
            lightBar.GetComponent<Renderer>().sharedMaterial = _rustedMetalMat;

            GameObject siren = new GameObject("Siren_BeaconLight");
            siren.transform.SetParent(policeGo.transform);
            siren.transform.localPosition = new Vector3(0, 2.35f, -0.2f);
            Light sLight = siren.AddComponent<Light>();
            sLight.type = LightType.Point;
            sLight.color = new Color(1.0f, 0.08f, 0.05f);
            sLight.intensity = 2.5f;
            sLight.range = 16f;

            // Attach 4 Rubber Wheels
            AttachVehicleWheels(policeGo.transform, 1.05f, 0.45f, 1.45f, true);

            // Vehicle Prop & NavMesh Obstacle
            var boxCol = policeGo.AddComponent<BoxCollider>();
            boxCol.center = new Vector3(0, 1.1f, 0);
            boxCol.size = new Vector3(2.6f, 2.0f, 5.0f);

            var prop = policeGo.AddComponent<AbandonedVehicleProp>();
        }

        private void CreateEvacuationSedan(Vector3 position, float rotY)
        {
            GameObject sedanGo = new GameObject("SM_Veh_Sedan_Evacuation");
            sedanGo.transform.SetParent(_vehiclesRoot);
            sedanGo.transform.position = position;
            sedanGo.transform.rotation = Quaternion.Euler(0, rotY, 0);

            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            body.name = "Body";
            body.transform.SetParent(sedanGo.transform);
            body.transform.localPosition = new Vector3(0, 0.82f, 0);
            body.transform.localScale = new Vector3(2.1f, 0.85f, 4.6f);
            body.GetComponent<Renderer>().sharedMaterial = _sedanCarMat;

            GameObject cabin = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cabin.name = "Cabin";
            cabin.transform.SetParent(sedanGo.transform);
            cabin.transform.localPosition = new Vector3(0, 1.58f, -0.15f);
            cabin.transform.localScale = new Vector3(1.85f, 0.80f, 2.3f);
            cabin.GetComponent<Renderer>().sharedMaterial = _windowGlassMat;

            // Roof Luggage
            for (float l = -0.35f; l <= 0.36f; l += 0.35f)
            {
                GameObject suitcase = GameObject.CreatePrimitive(PrimitiveType.Cube);
                suitcase.name = $"RoofLuggage_{l:F2}";
                suitcase.transform.SetParent(sedanGo.transform);
                suitcase.transform.localPosition = new Vector3(l, 2.22f, -0.15f);
                suitcase.transform.localScale = new Vector3(0.55f, 0.28f, 0.85f);
                suitcase.GetComponent<Renderer>().sharedMaterial = _woodCrateMat;
            }

            AttachVehicleWheels(sedanGo.transform, 1.02f, 0.44f, 1.40f, false);

            var boxCol = sedanGo.AddComponent<BoxCollider>();
            boxCol.center = new Vector3(0, 1.1f, 0);
            boxCol.size = new Vector3(2.4f, 2.2f, 4.8f);

            sedanGo.AddComponent<AbandonedVehicleProp>();
        }

        private void CreateMilitaryTruck(Vector3 position, float rotY)
        {
            GameObject truckGo = new GameObject("SM_Veh_MilitaryTruck_Blocking");
            truckGo.transform.SetParent(_vehiclesRoot);
            truckGo.transform.position = position;
            truckGo.transform.rotation = Quaternion.Euler(0, rotY, 0);

            // Cab
            GameObject cab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cab.name = "TruckCab";
            cab.transform.SetParent(truckGo.transform);
            cab.transform.localPosition = new Vector3(0, 1.8f, 2.8f);
            cab.transform.localScale = new Vector3(2.6f, 2.2f, 2.6f);
            cab.GetComponent<Renderer>().sharedMaterial = _truckMat;

            // Bed
            GameObject bed = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bed.name = "TruckBed";
            bed.transform.SetParent(truckGo.transform);
            bed.transform.localPosition = new Vector3(0, 2.2f, -1.4f);
            bed.transform.localScale = new Vector3(2.7f, 2.8f, 5.8f);
            bed.GetComponent<Renderer>().sharedMaterial = _rustedMetalMat;

            AttachVehicleWheels(truckGo.transform, 1.25f, 0.65f, 2.2f, false);
            AttachVehicleWheels(truckGo.transform, 1.25f, 0.65f, -2.2f, false);

            var boxCol = truckGo.AddComponent<BoxCollider>();
            boxCol.center = new Vector3(0, 2.0f, 0.5f);
            boxCol.size = new Vector3(2.9f, 3.8f, 8.5f);

            truckGo.AddComponent<AbandonedVehicleProp>();
        }

        private void CreateAmbulance(Vector3 position, float rotY)
        {
            GameObject ambGo = new GameObject("SM_Veh_Ambulance_Emergency");
            ambGo.transform.SetParent(_vehiclesRoot);
            ambGo.transform.position = position;
            ambGo.transform.rotation = Quaternion.Euler(0, rotY, 0);

            GameObject boxBody = GameObject.CreatePrimitive(PrimitiveType.Cube);
            boxBody.name = "AmbulanceBody";
            boxBody.transform.SetParent(ambGo.transform);
            boxBody.transform.localPosition = new Vector3(0, 1.6f, 0);
            boxBody.transform.localScale = new Vector3(2.4f, 2.2f, 5.6f);
            boxBody.GetComponent<Renderer>().sharedMaterial = _ambulanceMat;

            AttachVehicleWheels(ambGo.transform, 1.15f, 0.55f, 1.8f, false);

            var boxCol = ambGo.AddComponent<BoxCollider>();
            boxCol.center = new Vector3(0, 1.6f, 0);
            boxCol.size = new Vector3(2.6f, 2.4f, 5.8f);

            ambGo.AddComponent<AbandonedVehicleProp>();
        }

        private void CreateCrashedMotorcycle(Vector3 position)
        {
            GameObject moto = GameObject.CreatePrimitive(PrimitiveType.Cube);
            moto.name = "SM_Veh_Motorcycle_Crashed";
            moto.transform.SetParent(_vehiclesRoot);
            moto.transform.position = position;
            moto.transform.rotation = Quaternion.Euler(75f, 25f, 0);
            moto.transform.localScale = new Vector3(0.4f, 0.6f, 1.8f);
            moto.GetComponent<Renderer>().sharedMaterial = _rustedMetalMat;
        }

        private void AttachVehicleWheels(Transform parent, float wX, float wY, float wZ, bool isFlatFrontLeft)
        {
            foreach (float side in new float[] { -1f, 1f })
            {
                foreach (float end in new float[] { -1f, 1f })
                {
                    bool isFlat = (isFlatFrontLeft && side < 0 && end > 0);
                    float tireR = isFlat ? 0.35f : 0.45f;

                    GameObject tire = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    tire.name = $"Tire_{(side < 0 ? "L" : "R")}_{(end < 0 ? "R" : "F")}";
                    tire.transform.SetParent(parent);
                    tire.transform.localRotation = Quaternion.Euler(0, 0, 90f);
                    tire.transform.localPosition = new Vector3(side * wX, isFlat ? wY - 0.1f : wY, end * wZ);
                    tire.transform.localScale = new Vector3(tireR * 2f, 0.26f, tireR * 2f);
                    tire.GetComponent<Renderer>().sharedMaterial = _tireRubberMat;
                    DestroyImmediate(tire.GetComponent<Collider>());
                }
            }
        }

        #endregion

        #region 5. MILITARY CHECKPOINT & STORYTELLING VIGNETTES

        private void BuildMilitaryQuarantineCheckpoint()
        {
            for (int i = _checkpointRoot.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(_checkpointRoot.GetChild(i).gameObject);
            }

            GameObject cpGo = new GameObject("Military_Checkpoint_Quarantine");
            cpGo.transform.SetParent(_checkpointRoot);
            cpGo.transform.position = new Vector3(0, 0, 26f);

            // Concrete Jersey Highway Barriers arranged in chicane
            var barrierPos = new (float x, float z, float rotY)[]
            {
                (-8f, 0f, 0f),
                (-2f, 1.8f, 15f),
                (4f, -1.8f, -15f),
                (10f, 0f, 0f)
            };

            foreach (var bp in barrierPos)
            {
                GameObject barrier = GameObject.CreatePrimitive(PrimitiveType.Cube);
                barrier.name = $"JerseyBarrier_{bp.x:F0}";
                barrier.transform.SetParent(cpGo.transform);
                barrier.transform.localPosition = new Vector3(bp.x, 0.55f, bp.z);
                barrier.transform.localRotation = Quaternion.Euler(0, bp.rotY, 0);
                barrier.transform.localScale = new Vector3(3.6f, 1.1f, 0.8f);
                barrier.GetComponent<Renderer>().sharedMaterial = _jerseyBarrierMat;
            }

            // Sandbag Emplacements
            for (int sb = -1; sb <= 1; sb++)
            {
                GameObject sandbags = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sandbags.name = $"SandbagNest_{sb}";
                sandbags.transform.SetParent(cpGo.transform);
                sandbags.transform.localPosition = new Vector3(-7f + sb * 2.5f, 0.42f, 3.2f);
                sandbags.transform.localScale = new Vector3(2.4f, 0.85f, 1.2f);
                sandbags.GetComponent<Renderer>().sharedMaterial = _sandbagMat;
            }

            // Tripod Military Searchlight
            GameObject searchlight = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            searchlight.name = "Military_Searchlight";
            searchlight.transform.SetParent(cpGo.transform);
            searchlight.transform.localPosition = new Vector3(-8f, 2.2f, 1.2f);
            searchlight.transform.localRotation = Quaternion.Euler(45f, 25f, 0);
            searchlight.transform.localScale = new Vector3(0.5f, 0.4f, 0.5f);
            searchlight.GetComponent<Renderer>().sharedMaterial = _rustedMetalMat;

            GameObject slSource = new GameObject("Searchlight_SpotBeam");
            slSource.transform.SetParent(searchlight.transform);
            slSource.transform.localPosition = Vector3.forward * 0.5f;
            Light sLight = slSource.AddComponent<Light>();
            sLight.type = LightType.Spot;
            sLight.color = new Color(0.95f, 0.95f, 1.0f);
            sLight.intensity = 4.0f;
            sLight.range = 35f;
            sLight.spotAngle = 45f;

            // Biohazard Warning Sign
            GameObject signPost = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            signPost.name = "BiohazardSign_Post";
            signPost.transform.SetParent(cpGo.transform);
            signPost.transform.localPosition = new Vector3(-6f, 1.3f, -1.8f);
            signPost.transform.localScale = new Vector3(0.06f, 1.3f, 0.06f);
            signPost.GetComponent<Renderer>().sharedMaterial = _rustedMetalMat;

            GameObject signBoard = GameObject.CreatePrimitive(PrimitiveType.Cube);
            signBoard.name = "BiohazardSign_Board";
            signBoard.transform.SetParent(cpGo.transform);
            signBoard.transform.localPosition = new Vector3(-6f, 2.2f, -1.8f);
            signBoard.transform.localScale = new Vector3(1.4f, 1.0f, 0.06f);
            signBoard.GetComponent<Renderer>().sharedMaterial = _hazardSignMat;

            // Military Crates Stack
            var cratePos = new Vector3[] { new Vector3(7f, 0.35f, 2f), new Vector3(7.8f, 0.35f, 1.6f), new Vector3(7.4f, 0.95f, 1.8f) };
            foreach (var cp in cratePos)
            {
                GameObject crate = GameObject.CreatePrimitive(PrimitiveType.Cube);
                crate.name = "Military_AmmoCrate";
                crate.transform.SetParent(cpGo.transform);
                crate.transform.localPosition = cp;
                crate.transform.localScale = new Vector3(0.9f, 0.7f, 0.9f);
                crate.GetComponent<Renderer>().sharedMaterial = _woodCrateMat;
            }

            // Attach Environmental Storytelling Controller
            var storyComp = cpGo.AddComponent<EnvironmentalStorytellingScene>();
        }

        private void BuildStorytellingVignettes()
        {
            // Bloodstained area near breached checkpoint barricade
            GameObject bloodStain = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            bloodStain.name = "StoryDecal_BloodStain_Breach";
            bloodStain.transform.SetParent(_checkpointRoot);
            bloodStain.transform.position = new Vector3(0.5f, 0.007f, 24f);
            bloodStain.transform.localScale = new Vector3(2.5f, 0.005f, 3.2f);
            bloodStain.GetComponent<Renderer>().sharedMaterial = _bloodDecalMat;
            DestroyImmediate(bloodStain.GetComponent<Collider>());

            // Scattered luggage & personal belongings
            var luggagePositions = new Vector3[]
            {
                new Vector3(4.5f, 0.15f, -11f),
                new Vector3(5.2f, 0.15f, -9.5f),
                new Vector3(3.8f, 0.15f, -12.5f)
            };
            foreach (var lp in luggagePositions)
            {
                GameObject suitcase = GameObject.CreatePrimitive(PrimitiveType.Cube);
                suitcase.name = "Scattered_Luggage";
                suitcase.transform.SetParent(_checkpointRoot);
                suitcase.transform.position = lp;
                suitcase.transform.rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
                suitcase.transform.localScale = new Vector3(0.6f, 0.25f, 0.45f);
                suitcase.GetComponent<Renderer>().sharedMaterial = _boardedWoodMat;
            }
        }

        #endregion

        #region 6. NATURE & OVERGROWN VEGETATION

        private void BuildOvergrownNatureAndVegetation()
        {
            for (int i = _natureRoot.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(_natureRoot.GetChild(i).gameObject);
            }

            // Mature Broadleaf Oak Trees along Sidewalks
            var treePositions = new Vector3[]
            {
                new Vector3(-18.5f, 0.3f, -70f),
                new Vector3(-18.5f, 0.3f, -20f),
                new Vector3(-18.5f, 0.3f, 35f),
                new Vector3(-18.5f, 0.3f, 80f),
                new Vector3(18.5f, 0.3f, -80f),
                new Vector3(18.5f, 0.3f, -35f),
                new Vector3(18.5f, 0.3f, 15f),
                new Vector3(18.5f, 0.3f, 70f)
            };

            foreach (var tp in treePositions)
            {
                CreateOakTree(tp);
            }

            // Wild Shrubs & Bushes in Concrete Sidewalk Seams
            for (float z = -90f; z <= 90f; z += 18f)
            {
                foreach (float side in new float[] { -15.2f, 15.2f })
                {
                    GameObject bush = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    bush.name = $"Wild_Bush_{side:F0}_{z:F0}";
                    bush.transform.SetParent(_natureRoot);
                    bush.transform.position = new Vector3(side, 0.5f, z + UnityEngine.Random.Range(-2f, 2f));
                    bush.transform.localScale = new Vector3(1.2f, 0.8f, 1.2f);
                    bush.GetComponent<Renderer>().sharedMaterial = _foliageMat;
                    DestroyImmediate(bush.GetComponent<Collider>());
                }
            }

            // Weeds & Grass pushing through road cracks & potholes
            for (int g = 0; g < 16; g++)
            {
                float rx = UnityEngine.Random.Range(-12f, 12f);
                float rz = UnityEngine.Random.Range(-100f, 100f);
                GameObject weed = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                weed.name = $"RoadCrack_WeedCluster_{g}";
                weed.transform.SetParent(_natureRoot);
                weed.transform.position = new Vector3(rx, 0.15f, rz);
                weed.transform.localScale = new Vector3(0.6f, 0.15f, 0.6f);
                weed.GetComponent<Renderer>().sharedMaterial = _foliageMat;
                DestroyImmediate(weed.GetComponent<Collider>());
            }
        }

        private void CreateOakTree(Vector3 position)
        {
            GameObject treeRoot = new GameObject($"SM_Tree_Oak_{position.z:F0}");
            treeRoot.transform.SetParent(_natureRoot);
            treeRoot.transform.position = position;

            // Trunk
            GameObject trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            trunk.name = "Trunk";
            trunk.transform.SetParent(treeRoot.transform);
            trunk.transform.localPosition = new Vector3(0, 3.5f, 0);
            trunk.transform.localScale = new Vector3(0.7f, 3.5f, 0.7f);
            trunk.GetComponent<Renderer>().sharedMaterial = _treeBarkMat;

            // Foliage Canopy (Multi-tier broadleaf canopy)
            var canopySpheres = new (Vector3 offset, Vector3 scale)[]
            {
                (new Vector3(0, 7.5f, 0), new Vector3(5.5f, 4.0f, 5.5f)),
                (new Vector3(-1.2f, 8.5f, 1.0f), new Vector3(3.8f, 3.2f, 3.8f)),
                (new Vector3(1.4f, 8.2f, -1.2f), new Vector3(4.2f, 3.5f, 4.2f))
            };

            foreach (var cs in canopySpheres)
            {
                GameObject leafCluster = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                leafCluster.name = "Foliage_Cluster";
                leafCluster.transform.SetParent(treeRoot.transform);
                leafCluster.transform.localPosition = cs.offset;
                leafCluster.transform.localScale = cs.scale;
                leafCluster.GetComponent<Renderer>().sharedMaterial = _foliageMat;
                DestroyImmediate(leafCluster.GetComponent<Collider>());
            }
        }

        #endregion

        #region 7. ENVIRONMENTAL VFX & PARTICLES

        private void BuildEnvironmentalParticleVFX()
        {
            for (int i = _vfxRoot.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(_vfxRoot.GetChild(i).gameObject);
            }

            // Ground Mist / Road Vapor (z = 0)
            CreateParticleEmitter("VFX_GroundMist_Avenue", new Vector3(0, 0.2f, 0), ParticlePreset.Mist);

            // Smoke from burned debris / engine bay (z = 48)
            CreateParticleEmitter("VFX_Vehicle_SmokePlume", new Vector3(-6.5f, 1.8f, 49f), ParticlePreset.Smoke);

            // Flickering Electrical Sparks near damaged transformer (z = -32)
            CreateParticleEmitter("VFX_Electrical_Sparks", new Vector3(-15.5f, 4.2f, -32f), ParticlePreset.Sparks);
        }

        private enum ParticlePreset { Mist, Smoke, Sparks }

        private void CreateParticleEmitter(string name, Vector3 position, ParticlePreset preset)
        {
            GameObject pGo = new GameObject(name);
            pGo.transform.SetParent(_vfxRoot);
            pGo.transform.position = position;

            ParticleSystem ps = pGo.AddComponent<ParticleSystem>();
            var main = ps.main;
            var emission = ps.emission;
            var shape = ps.shape;

            main.playOnAwake = true;
            main.loop = true;

            switch (preset)
            {
                case ParticlePreset.Mist:
                    main.startLifetime = 6.0f;
                    main.startSpeed = 0.2f;
                    main.startSize = 3.5f;
                    main.startColor = new Color(0.85f, 0.90f, 0.92f, 0.15f);
                    main.maxParticles = 60;
                    emission.rateOverTime = 8f;
                    shape.shapeType = ParticleSystemShapeType.Box;
                    shape.scale = new Vector3(25f, 0.5f, 80f);
                    break;

                case ParticlePreset.Smoke:
                    main.startLifetime = 4.5f;
                    main.startSpeed = 1.2f;
                    main.startSize = 1.8f;
                    main.startColor = new Color(0.18f, 0.18f, 0.18f, 0.35f);
                    main.maxParticles = 40;
                    emission.rateOverTime = 6f;
                    shape.shapeType = ParticleSystemShapeType.Cone;
                    shape.angle = 15f;
                    break;

                case ParticlePreset.Sparks:
                    main.startLifetime = 0.4f;
                    main.startSpeed = 4.5f;
                    main.startSize = 0.08f;
                    main.startColor = new Color(1.0f, 0.9f, 0.4f, 1.0f);
                    main.maxParticles = 25;
                    emission.rateOverTime = 3f;
                    shape.shapeType = ParticleSystemShapeType.Sphere;
                    shape.radius = 0.2f;
                    break;
            }
        }

        #endregion

        #region 8. CINEMATIC ATMOSPHERE & LIGHTING

        private void BuildCinematicAtmosphereAndLighting()
        {
            // Directional Sun
            Light sun = null;
            var sunGo = GameObject.Find("Directional Light");
            if (sunGo == null)
            {
                sunGo = new GameObject("Directional Light");
                sun = sunGo.AddComponent<Light>();
                sun.type = LightType.Directional;
            }
            else
            {
                sun = sunGo.GetComponent<Light>();
            }

            sun.transform.rotation = Quaternion.Euler(48f, -35f, 0);
            sun.shadows = LightShadows.Soft;

            // Atmosphere Controller
            var atmoController = gameObject.GetComponent<EnvironmentAtmosphereController>();
            if (atmoController == null) atmoController = gameObject.AddComponent<EnvironmentAtmosphereController>();
            atmoController.ApplyAtmospherePreset(_initialAtmosphere);
        }

        #endregion

        #region 9. GAMEPLAY SPAWNERS & NAVIGATION BOUNDS

        private void BuildGameplaySpawnersAndNavBounds()
        {
            for (int i = _gameplayRoot.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(_gameplayRoot.GetChild(i).gameObject);
            }

            // Player Spawn Marker (Safe avenue start)
            GameObject playerSpawn = new GameObject("PlayerSpawn_EthanCross");
            playerSpawn.transform.SetParent(_gameplayRoot);
            playerSpawn.transform.position = new Vector3(0, 0.2f, -95f);
            playerSpawn.transform.rotation = Quaternion.identity;

            // 5 Distinct Zombie Tactical Spawn Points
            var zombieSpawns = new (string name, Vector3 pos, ZombieType zType)[]
            {
                ("Spawn_Walker_Alley", new Vector3(-22f, 0.2f, -40f), ZombieType.Walker),
                ("Spawn_Runner_Crossroads", new Vector3(0, 0.2f, -25f), ZombieType.Runner),
                ("Spawn_Stalker_Rooftop", new Vector3(25f, 14.5f, 10f), ZombieType.Stalker),
                ("Spawn_Brute_Barricade", new Vector3(0, 0.2f, 30f), ZombieType.Brute),
                ("Spawn_AlphaBoss_Depot", new Vector3(0, 0.2f, 85f), ZombieType.AlphaBoss)
            };

            foreach (var zs in zombieSpawns)
            {
                GameObject spGo = new GameObject(zs.name);
                spGo.transform.SetParent(_gameplayRoot);
                spGo.transform.position = zs.pos;

                // Visual marker gizmo / indicator in editor
                var col = spGo.AddComponent<BoxCollider>();
                col.isTrigger = true;
                col.size = new Vector3(1.2f, 2.0f, 1.2f);
                col.center = new Vector3(0, 1.0f, 0);
            }

            // Map Boundary Invisible Colliders
            CreateMapBoundaryCollider("Boundary_North", new Vector3(0, 10f, 122f), new Vector3(80f, 20f, 4f));
            CreateMapBoundaryCollider("Boundary_South", new Vector3(0, 10f, -122f), new Vector3(80f, 20f, 4f));
            CreateMapBoundaryCollider("Boundary_East", new Vector3(40f, 10f, 0), new Vector3(4f, 20f, 250f));
            CreateMapBoundaryCollider("Boundary_West", new Vector3(-40f, 10f, 0), new Vector3(4f, 20f, 250f));
        }

        private void CreateMapBoundaryCollider(string name, Vector3 pos, Vector3 size)
        {
            GameObject bound = new GameObject(name);
            bound.transform.SetParent(_gameplayRoot);
            bound.transform.position = pos;
            var box = bound.AddComponent<BoxCollider>();
            box.size = size;
        }

        #endregion
    }
}

