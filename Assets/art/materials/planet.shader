Shader "Custom/planet"
{
    Properties
    {
        _sand("sand", Color) = (1, 1, 1, 1)
        _grass("grass", Color) = (1, 1, 1, 1)
        _percentage("percentage", Range(0, 1)) = 0.5
        _shadow("shadow", Range(0, 1)) = 0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float noiseValue : TEXCOORD1;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _sand;
                half4 _grass;
                float4 _BaseMap_ST;
                float _percentage;
                float _shadow;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.noiseValue = IN.uv2.x;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float grass = IN.noiseValue > _percentage;
                float sand = 1 - grass;

                half4 groundColor = grass * _grass + sand * _sand;
                half4 color =  lerp(groundColor, groundColor * IN.noiseValue, _shadow);
                
                return color;
            }
            ENDHLSL
        }
    }
}
