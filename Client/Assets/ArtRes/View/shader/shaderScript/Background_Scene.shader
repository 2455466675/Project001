
Shader "Unlit/Background_Scene"
{
    Properties
    {
        _GridSize ("Grid Size", Range(1, 100)) = 10
        [Toggle] _PinkMode ("Pink Mode", Float) = 0
        _PinkIntensity ("Pink Intensity", Range(0, 1)) = 0.5
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque" 
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }
        
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
            };
            
            CBUFFER_START(UnityPerMaterial)
            float _GridSize;
            float _PinkMode;
            float _PinkIntensity;
            CBUFFER_END
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }
            
            half4 frag(Varyings IN) : SV_Target
            {
                // 创建棋盘格图案
                float2 scaledUV = IN.uv * _GridSize;
                int2 gridPos = floor(scaledUV);
                bool isEven = (gridPos.x % 2) == (gridPos.y % 2);
                
                // 基础棋盘格颜色
                half3 baseColor = isEven ? half3(1, 1, 1) : half3(0, 0, 0);
                
                // 粉色模式
                if (_PinkMode > 0.5)
                {
                    // 粉色棋盘格：白色变粉色，黑色变深粉色
                    half3 pink = half3(1.0, 0.41, 0.71); // 粉色RGB值
                    baseColor = isEven ? pink : lerp(baseColor, pink * 0.5, _PinkIntensity);
                }
                
                return half4(baseColor, 1);
            }
            ENDHLSL
        }
        
        // 阴影投射通道
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            
            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}