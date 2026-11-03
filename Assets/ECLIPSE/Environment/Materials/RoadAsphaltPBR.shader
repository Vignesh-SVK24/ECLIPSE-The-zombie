Shader "Eclipse/Environment/RoadAsphaltPBR"
{
    Properties
    {
        _BaseColor("Asphalt Base Color", Color) = (0.13, 0.14, 0.13, 1.0)
        _BaseMap("Asphalt Albedo Texture", 2D) = "white" {}
        _BumpMap("Asphalt Normal Map", 2D) = "bump" {}
        _BumpScale("Normal Scale", Range(0.1, 2.0)) = 1.0
        _Roughness("Base Roughness", Range(0.0, 1.0)) = 0.85
        _Metallic("Base Metallic", Range(0.0, 1.0)) = 0.05
        _CrackIntensity("Crack Fissure Intensity", Range(0.0, 1.0)) = 0.65
        _MarkingFade("Road Marking Fade", Range(0.0, 1.0)) = 0.50
        _Wetness("Rain Wetness / Puddle", Range(0.0, 1.0)) = 0.35
        _PuddleThreshold("Puddle Accumulation Threshold", Range(0.0, 1.0)) = 0.60
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
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment

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
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_BumpMap);
            SAMPLER(sampler_BumpMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _BaseMap_ST;
                float _BumpScale;
                float _Roughness;
                float _Metallic;
                float _CrackIntensity;
                float _MarkingFade;
                float _Wetness;
                float _PuddleThreshold;
            CBUFFER_END

            Varyings LitPassVertex(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);

                return output;
            }

            half4 LitPassFragment(Varyings input) : SV_Target
            {
                Light mainLight = GetMainLight();
                float3 N = normalize(input.normalWS);
                float3 L = normalize(mainLight.direction);
                float3 V = normalize(GetCameraPositionWS() - input.positionWS);

                half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;

                // Wet puddle modulation (lowers roughness, darkens albedo)
                float puddleMask = saturate((_Wetness - (1.0 - _PuddleThreshold)) / _PuddleThreshold);
                float finalRoughness = lerp(_Roughness, 0.08, puddleMask * _Wetness);
                half3 finalAlbedo = lerp(albedo.rgb, albedo.rgb * 0.7, puddleMask * _Wetness);

                // Diffuse lighting
                float NdotL = saturate(dot(N, L));
                half3 diffuse = finalAlbedo * (SampleSH(N) + mainLight.color * NdotL);

                // Specular reflection (Sharp wet puddles)
                float3 H = normalize(L + V);
                float NdotH = saturate(dot(N, H));
                float specPower = lerp(8.0, 256.0, 1.0 - finalRoughness);
                float specular = pow(NdotH, specPower) * (1.0 - finalRoughness) * (puddleMask * 1.5 + 0.1);

                half3 finalColor = diffuse + (specular * mainLight.color);
                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}
