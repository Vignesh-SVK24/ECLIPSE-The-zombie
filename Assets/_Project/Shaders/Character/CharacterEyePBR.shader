Shader "Eclipse/Character/EyePBR"
{
    Properties
    {
        _IrisColor("Iris Tint Color", Color) = (0.22, 0.45, 0.65, 1.0)
        _ScleraColor("Sclera Color", Color) = (0.92, 0.90, 0.88, 1.0)
        _PupilSize("Pupil Dilation", Range(0.1, 0.8)) = 0.35
        _CorneaRoughness("Cornea Surface Roughness", Range(0.01, 0.5)) = 0.05
        _CorneaSpecular("Cornea Specular Intensity", Range(0.5, 3.0)) = 1.8
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry+1"
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
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 positionWS   : TEXCOORD0;
                float3 normalWS     : TEXCOORD1;
                float2 uv           : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _IrisColor;
                float4 _ScleraColor;
                float _PupilSize;
                float _CorneaRoughness;
                float _CorneaSpecular;
            CBUFFER_END

            Varyings LitPassVertex(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.uv = input.uv;
                return output;
            }

            half4 LitPassFragment(Varyings input) : SV_Target
            {
                Light mainLight = GetMainLight();
                float3 N = normalize(input.normalWS);
                float3 L = normalize(mainLight.direction);
                float3 V = normalize(GetCameraPositionWS() - input.positionWS);

                // Radial iris & pupil generation from UV coordinates
                float2 centerUV = input.uv - float2(0.5, 0.5);
                float dist = length(centerUV) * 2.0;

                half3 eyeColor;
                if (dist < _PupilSize)
                {
                    eyeColor = half3(0.01, 0.01, 0.01); // Pupil
                }
                else if (dist < 0.65)
                {
                    float irisT = (dist - _PupilSize) / (0.65 - _PupilSize);
                    eyeColor = lerp(_IrisColor.rgb * 1.2, _IrisColor.rgb * 0.5, irisT); // Iris striations
                }
                else
                {
                    eyeColor = _ScleraColor.rgb; // Sclera
                }

                // Diffuse lighting
                float NdotL = saturate(dot(N, L));
                half3 lighting = eyeColor * (SampleSH(N) + mainLight.color * NdotL);

                // Cornea Specular Highlight (Sharp moisture reflection)
                float3 H = normalize(L + V);
                float NdotH = saturate(dot(N, H));
                float spec = pow(NdotH, 256.0) * _CorneaSpecular;
                half3 finalColor = lighting + (spec * mainLight.color);

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}
