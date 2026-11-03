Shader "Eclipse/Character/SkinPBR"
{
    Properties
    {
        _BaseColor("Skin Color Tone", Color) = (0.85, 0.68, 0.58, 1.0)
        _SubsurfaceColor("Subsurface Scatter Tone", Color) = (0.9, 0.28, 0.18, 1.0)
        _BaseMap("Albedo / Skin Texture", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Normal Scale", Float) = 1.0
        _Roughness("Skin Roughness", Range(0.0, 1.0)) = 0.42
        _SubsurfaceScattering("Subsurface Scattering Intensity", Range(0.0, 1.0)) = 0.45
        _CurvatureBias("Curvature Wrap Bias", Range(0.0, 0.5)) = 0.18
        _SpecColor("Skin Specular Tint", Color) = (0.04, 0.04, 0.04, 1.0)
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
                float4 _SubsurfaceColor;
                float4 _BaseMap_ST;
                float _BumpScale;
                float _Roughness;
                float _SubsurfaceScattering;
                float _CurvatureBias;
                float4 _SpecColor;
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

                // Subsurface scattering wrap lighting
                float NdotL = dot(N, L);
                float diffuseWrap = saturate((NdotL + _CurvatureBias) / (1.0 + _CurvatureBias));

                // Direct light + Subsurface bleed into shadow terminator
                float sssTerm = saturate(1.0 - abs(NdotL)) * _SubsurfaceScattering;
                half3 sssColor = _SubsurfaceColor.rgb * sssTerm * mainLight.color;

                half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;
                half3 ambient = SampleSH(N) * albedo.rgb;
                half3 directLighting = albedo.rgb * mainLight.color * diffuseWrap + sssColor;

                // Micro-specular reflection
                float3 H = normalize(L + V);
                float NdotH = saturate(dot(N, H));
                float specPower = lerp(4.0, 128.0, 1.0 - _Roughness);
                float specular = pow(NdotH, specPower) * (1.0 - _Roughness);
                half3 finalColor = ambient + directLighting + (specular * mainLight.color * _SpecColor.rgb);

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }

        // Shadow Caster Pass
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
            };

            Varyings ShadowPassVertex(Attributes input)
            {
                Varyings output;
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _MainLightPosition.xyz));
                return output;
            }

            half4 ShadowPassFragment(Varyings input) : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}
