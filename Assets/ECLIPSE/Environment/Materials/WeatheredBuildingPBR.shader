Shader "Eclipse/Environment/WeatheredBuildingPBR"
{
    Properties
    {
        _BaseColor("Building Wall Tint", Color) = (0.75, 0.72, 0.68, 1.0)
        _BaseMap("Wall Albedo", 2D) = "white" {}
        _BumpMap("Wall Normal", 2D) = "bump" {}
        _GrimeColor("Grime / Dirt Color", Color) = (0.22, 0.20, 0.18, 1.0)
        _MossColor("Moss Overgrowth Color", Color) = (0.28, 0.38, 0.22, 1.0)
        _Roughness("Surface Roughness", Range(0.0, 1.0)) = 0.88
        _Metallic("Surface Metallic", Range(0.0, 1.0)) = 0.02
        _WeatheringAmount("Weathering Decay", Range(0.0, 1.0)) = 0.55
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
                float4 color        : COLOR;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 positionWS   : TEXCOORD0;
                float3 normalWS     : TEXCOORD1;
                float2 uv           : TEXCOORD2;
                float4 color        : COLOR;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            TEXTURE2D(_BumpMap);
            SAMPLER(sampler_BumpMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _GrimeColor;
                float4 _MossColor;
                float4 _BaseMap_ST;
                float _Roughness;
                float _Metallic;
                float _WeatheringAmount;
            CBUFFER_END

            Varyings LitPassVertex(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                output.color = input.color;
                return output;
            }

            half4 LitPassFragment(Varyings input) : SV_Target
            {
                Light mainLight = GetMainLight();
                float3 N = normalize(input.normalWS);
                float3 L = normalize(mainLight.direction);

                half4 albedo = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv) * _BaseColor;

                // Blend grime (vertex red) and moss (vertex green)
                half3 weatheredColor = lerp(albedo.rgb, _GrimeColor.rgb, input.color.r * _WeatheringAmount);
                weatheredColor = lerp(weatheredColor, _MossColor.rgb, input.color.g * _WeatheringAmount);

                float NdotL = saturate(dot(N, L));
                half3 lighting = weatheredColor * (SampleSH(N) + mainLight.color * NdotL);

                return half4(lighting, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}
