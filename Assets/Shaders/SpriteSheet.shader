Shader "Unlit/SpriteSheet"
{
    Properties
    {
        [Header(Texture Sheet)]
        [PerRendererData]
        _MainTex("Texture", 2D) = "white" {}

        [Header(Settings)]
        _ColumnCount("Column Count (X)", int) = 1
        _RowCount("Row Count (Y)", int) = 1
        _AnimationSpeed("Frames Per Seconds", float) = 10
    }
    SubShader
    {
        Tags 
        {
            "Queue" = "AlphaTest"
            "IgnoreProjector" = "True"
            "PreviewType" = "Plane"
            "RenderType" = "TransparentCutout"
            //"DisableBatching" = "True"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert 
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            uint _ColumnCount;
            uint _RowCount;
            float _AnimationSpeed;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // get single sprite size
                float2 size = float2(1.0f / _ColumnCount, 1.0f / _RowCount);
                uint totalFrames = _ColumnCount * _RowCount;
        
                uint totalIndex = _Time.y * _AnimationSpeed; // use timer to increment index
       
                uint columnIndex = totalIndex % _ColumnCount; // wrap x and y indexes
                uint rowIndex = floor((totalIndex % totalFrames) / _ColumnCount);

                float2 newUV = v.uv * size; // get single sprite UV
                newUV.y = newUV.y + size.y * (_RowCount - 1); // flip Y (to start 0 from top)

                float2 offset = float2(size.x * columnIndex, -size.y * rowIndex);  // get offsets to our sprite index
                o.uv = newUV + offset;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
        ENDCG
        }
    }
}
