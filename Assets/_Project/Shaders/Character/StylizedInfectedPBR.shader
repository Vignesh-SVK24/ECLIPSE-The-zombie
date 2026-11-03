Shader "Eclipse/Character/StylizedInfectedPBR"
{
    Properties
    {
        [Header(Flesh Shading)]
        _BaseColor("Infected Flesh Tint", Color) = (0.33, 0.42, 0.28, 1.0) // Sickly olive-grey
        _BaseMap("Albedo / Hand-Painted Texture", 2D) = "white" {}
        _BumpMap("Normal / Vein Structure", 2D) = "bump" {}
        _BumpScale("Normal Strength", Range(0.0, 2.0)) = 1.2

        [Header(Stylized Infection Effects)]
        _VeinColor("Bruised Vein Tint", Color) = (0.24, 0.15, 0.27, 1.0) // Deep necrotic violet
        _ShadowColor("Shadow Tint", Color) = (0.16, 0.20, 0.15, 1.0)
        _RampThreshold("Shadow Threshold", Range(0.0, 1.0)) = 0.50
        _RampSmoothness("Shadow Feathering", Range(0.01, 0.5)) = 0.12

        [Header(Neon Green Eye Glow)]
        _EyeGlowColor("Infected Eye Glow", Color) = (0.22, 1.0, 0.08, 1.0) // Vivid neon lime green
        _EyeGlowIntensity("Glow Multiplier", Range(0.0, 8.0)) = 4.0
        _EyePulseRate("Glow Pulse Speed", Range(0.0, 10.0)) = 3.0
        _EmissionMap("Emission / Eye Mask", 2D) = "black" {}

        [Header(Stylized Rim Light)]
        _RimColor("Sunset Rim Tint", Color) = (1.0, 0.60, 0.25, 1.0)
        _RimPower("Rim Falloff", Range(0.5, 8.0)) = 3.5
        _RimIntensity("Rim Intensity", Range(0.0, 4.0)) = 1.2
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
            Name "InfectedForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex InfectedPassVertex
            #pragma fragment InfectedPassFragment

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE

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
            TEXTURE2D(_EmissionMap);
            SAMPLER(sampler_EmissionMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _VeinColor;
                float4 _ShadowColor;
                float4 _EyeGlowColor;
                float4 _RimColor;
                float4 _BaseMap_ST;
                float _BumpScale;
                float _RampThreshold;
                float _RampSmoothness;
                float _EyeGlowIntensity;
                float _EyePulseRate;
                float _RimPower;
                float _RimIntensity;
            CBUFFER_END

            Varyings InfectedPassVertex(Attributes input)
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

            half4 InfectedPassFragment(Varyings input) : SV_Target
            {
                float3 N = normalize(input.normalWS);
                float3 V = normalize(input.viewDirWS);

                // Sample base albedo
                half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;

                // Main light
                Light mainLight = GetMainLight();
                float3 L = normalize(mainLight.direction);

                float NdotL = dot(N, L);
                float halfLambert = NdotL * 0.5 + 0.5;
                float ramp = smoothstep(_RampThreshold - _RampSmoothness, _RampThreshold + _RampSmoothness, halfLambert);

                // Blend necrotic shadow with sunlight
                half3 diffuseLight = lerp(_ShadowColor.rgb, mainLight.color.rgb, ramp);

                // Stylized Sunset Rim Light
                float NdotV = saturate(dot(N, V));
                float rimTerm = pow(1.0 - NdotV, _RimPower);
                half3 rimGlow = _RimColor.rgb * (rimTerm * _RimIntensity);

                // Eye Emission Glow with subtle biological pulse
                half4 emissionMask = SAMPLE_TEXTURE2D(_EmissionMap, sampler_EmissionMap, input.uv);
                float pulse = 0.85 + 0.15 * sin(_Time.y * _EyePulseRate);
                half3 eyeGlow = _EyeGlowColor.rgb * (_EyeGlowIntensity * pulse * emissionMask.r);

                half3 finalColor = albedo.rgb * diffuseLight + rimGlow + eyeGlow;

                return half4(finalColor, albedo.a);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}

