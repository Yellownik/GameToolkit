// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Toolkit/SpriteDefault"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
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

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 uv  : TEXCOORD0;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t i)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(i.vertex);
				o.uv = i.uv;
				o.color = i.color * _Color;
				#ifdef PIXELSNAP_ON
				o.vertex = UnityPixelSnap (o.vertex);
				#endif

				return o;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (i.uv) * i.color;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}