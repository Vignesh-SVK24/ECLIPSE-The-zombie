Shader "Eclipse/Character/StylizedCharacterPBR"
{
    Properties
    {
        [Header(Base Styling)]
        _BaseColor("Base Color Tint", Color) = (1.0, 1.0, 1.0, 1.0)
        _BaseMap("Albedo / Hand-Painted Texture", 2D) = "white" {}
        _BumpMap("Normal / Surface Detail", 2D) = "bump" {}
        _BumpScale("Normal Strength", Range(0.0, 2.0)) = 1.0

        [Header(Stylized Shading Ramp)]
        _ShadowColor("Shadow Tint / Ambient Tone", Color) = (0.28, 0.22, 0.32, 1.0) // Deep dusk twilight
        _RampThreshold("Shadow Threshold", Range(0.0, 1.0)) = 0.45
        _RampSmoothness("Shadow Feathering", Range(0.01, 0.5)) = 0.15
        _SubsurfaceColor("Subsurface Scatter Glow", Color) = (0.95, 0.35, 0.18, 1.0) // Warm sunset flush
        _SubsurfaceIntensity("Subsurface Intensity", Range(0.0, 1.0)) = 0.35

        [Header(Specular and Rim Lighting)]
        _SpecColor("Stylized Specular Tint", Color) = (1.0, 0.95, 0.85, 1.0)
        _Smoothness("Surface Smoothness", Range(0.0, 1.0)) = 0.40
        _Metallic("Metallic Sheen", Range(0.0, 1.0)) = 0.05
        _RimColor("Sunset Rim Light Color", Color) = (1.0, 0.65, 0.25, 1.0) // Golden hour rim
        _RimPower("Rim Light Falloff Power", Range(0.5, 8.0)) = 3.0
        _RimIntensity("Rim Light Multiplier", Range(0.0, 4.0)) = 1.6
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }
        LOD 300

        Pass
        {
            Name "StylizedForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex StylizedPassVertex
            #pragma fragment StylizedPassFragment

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 positionWS   : TEXCOORD0;
                float3 normalWS     : TEXCOORD1;
                float2 uv           : TEXCOORD2;
                float3 viewDirWS    : TEXCOORD3;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_BumpMap);
            SAMPLER(sampler_BumpMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _ShadowColor;
                float4 _SubsurfaceColor;
                float4 _SpecColor;
                float4 _RimColor;
                float4 _BaseMap_ST;
                float _BumpScale;
                float _RampThreshold;
                float _RampSmoothness;
                float _SubsurfaceIntensity;
                float _Smoothness;
                float _Metallic;
                float _RimPower;
                float _RimIntensity;
            CBUFFER_END

            Varyings StylizedPassVertex(Attributes input)
            {
                Varyings output;
                VertexPositionInputs posInputs = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normInputs = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                output.positionCS = posInputs.positionCS;
                output.positionWS = posInputs.positionWS;
                output.normalWS = normInputs.normalWS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.viewDirWS = GetCameraPositionWS() - posInputs.positionWS;

                return output;
            }

            half4 StylizedPassFragment(Varyings input) : SV_Target
            {
                float3 N = normalize(input.normalWS);
                float3 V = normalize(input.viewDirWS);

                // Sample Base Albedo & Apply Color Tint
                half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;

                // Main Directional Light (Sun)
                Light mainLight = GetMainLight();
                float3 L = normalize(mainLight.direction);

                // Half-Lambert Dot Product for Soft Stylized Lighting
                float NdotL = dot(N, L);
                float halfLambert = NdotL * 0.5 + 0.5;

                // Smoothstep Stylized Ramp Transition
                float ramp = smoothstep(_RampThreshold - _RampSmoothness, _RampThreshold + _RampSmoothness, halfLambert);

                // Main shadow / light blending
                half3 diffuseLight = lerp(_ShadowColor.rgb, mainLight.color.rgb, ramp);

                // Subsurface Scattering Term (Warm light bleed along terminator)
                float sssTerm = (1.0 - abs(NdotL)) * (1.0 - ramp) * _SubsurfaceIntensity;
                half3 sssGlow = _SubsurfaceColor.rgb * sssTerm * mainLight.color.rgb;

                // Stylized Blinn-Phong Specular
                float3 H = normalize(L + V);
                float NdotH = saturate(dot(N, H));
                float specPower = exp2(10.0 * _Smoothness + 1.0);
                float rawSpec = pow(NdotH, specPower);
                float stylizedSpec = smoothstep(0.4, 0.6, rawSpec) * _Smoothness;
                half3 specColor = _SpecColor.rgb * stylizedSpec * mainLight.color.rgb;

                // Stylized Golden-Hour Rim Light
                float NdotV = saturate(dot(N, V));
                float rimTerm = pow(1.0 - NdotV, _RimPower);
                // Mask rim light slightly so backlights pop harder
                float rimLightMask = saturate(dot(-L, V) * 0.5 + 0.5);
                half3 rimGlow = _RimColor.rgb * (rimTerm * _RimIntensity * rimLightMask);

                // Final Combined Shading
                half3 finalColor = albedo.rgb * (diffuseLight + sssGlow) + specColor + rimGlow;

                return half4(finalColor, albedo.a);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}

