Shader "Custom/UIDissolve"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        [Space]
        [Space]
        _MaskTex ("Mask Tex", 2D) = "white" {}
        _DissolveTex ("Dissolve Tex", 2D) = "white" {}
        _DistortionTex ("Distortion Tex", 2D) = "white" {}
        _CenterPoint ("Center Point", Vector) = (0.5, 0.5, 0, 0)

        [Space]
        _FPS("FPS", Range(1, 30)) = 5
        _DistortionPower ("Distortion Power", Range(0, 3)) = 0.5
        _DistortionSpeed ("Distortion Speed", Vector) = (0.5, 0.5, 0, 0)
        _GradientAdjust ("Gradient Adjust", Range(0, 1)) = 0.5
        _DissolveValue ("Dissolve Value", Range(0, 1)) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            sampler2D _MaskTex;
            sampler2D _DissolveTex;
            sampler2D _DistortionTex;
            fixed4 _CenterPoint;

            fixed _FPS;
            fixed4 _DistortionSpeed;
            fixed _DistortionPower;
            fixed _GradientAdjust;
            fixed _DissolveValue;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 func1(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                half steppedTime = round(_FPS * _Time.y) / _FPS;
                fixed2 distortion = tex2D(_DistortionTex, IN.texcoord + steppedTime * _DistortionSpeed.xy);
                half dissolveValue = tex2D(_DissolveTex, IN.texcoord + distortion * _DistortionPower).r;
                //clip(dissolveValue - _DissolveValue);


                fixed toPoint = length(IN.texcoord - _CenterPoint.xy) / ((1.0001 - _DissolveValue) * 0.5);
                fixed d = ((2.0 * _DissolveValue + dissolveValue) * toPoint) - 1.0;

                fixed overOne = saturate(d * _GradientAdjust);

                fixed burn = lerp(0.0, 1, overOne);
                color *= burn;

                float luminance = dissolveValue - _DissolveValue;
                color.a *= lerp(luminance, color.a, _DissolveValue);

                return color;
            }

            fixed4 func2(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif

                half steppedTime = round(_FPS * _Time.y) / _FPS;
                fixed2 distortion = tex2D(_DistortionTex, IN.texcoord + steppedTime * _DistortionSpeed.xy);
                half dissolveValue = tex2D(_DissolveTex, IN.texcoord + distortion * _DistortionPower).r;
                half maskValue = tex2D(_MaskTex, IN.texcoord + 0.5 - _CenterPoint.xy).r;

                fixed stepVal = lerp(0, _DissolveValue + 1 - maskValue, _DissolveValue);
                color *= stepVal;
                color.a *= lerp(dissolveValue, 1, _DissolveValue);
                return color;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                //return func1(IN);
                return func2(IN);
            }
        ENDCG
        }
    }
}