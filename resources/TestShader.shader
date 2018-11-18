Shader "Custom/TestShader" {
	Properties{
		//[PreRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_SecondTex("Overlay Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1.0,1.0,1.0,1.0)
		[MaterialToggle] PixelSnap("Pixel Snap", Float) = 0
	}

	SubShader{

		//Tags for this subshader, might affect rendering layers
		Tags {
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}

		//Some aditional properties
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma shader_feature ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"

			//User defined variables
			sampler2D _MainTex;
			sampler2D _AlphaTex;
			sampler2D _SecondTex;
			float4 _Color;

			//Unity defined variables

			//Base input structs
			struct vertexInput {
				//Should contain all the information we want about an existing vertex
				float4 vertex	: POSITION;
				float4 color	: COLOR;
				float2 texcoord	: TEXCOORD0;
			};

			struct vertexOutput {
				float4 vertex		: SV_POSITION;
				fixed4 color		: COLOR;
				float2 texcoord		: TEXCOORD0;
				float4 screenPos	: TEXCOORD1;
			};

			//Additional functions
			fixed4 SampleSpriteTexture(float2 uv) {
				fixed4 color = tex2D(_MainTex, uv);

				#if ETC1_EXTERNAL_ALPHA
				//Grab the color from an external texture (usecase: Alpha support for ETC1 on android)
				color.a = tex2D(_AlphaTex, uv).r;
				#endif

				return color;
			}

			//Vertex function
			vertexOutput vert(vertexInput v) {

				vertexOutput o;

				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color * _Color;

				o.screenPos = ComputeScreenPos(o.vertex);

				//o.color = tex2D(_SecondTex, o.screenPos.xy);

				#ifdef PIXELSNAP_ON
				o.vertex = UnityPixelSnap(o.vertex);
				#endif

				return o;

			}

			//Fragment function
			fixed4 frag(vertexOutput i) : COLOR {

				//Get the color of the pixel making sure to sample the sprite texture
				fixed4 c = SampleSpriteTexture(i.texcoord) * i.color;

				float4 overColor = tex2D(_SecondTex, i.screenPos.xy);
				c.rgb = overColor.rgb * i.color.rgb;


				//Apply alpha
				c.rgb *= c.a;

				return c;
				
			}


			ENDCG
		}
	}
}
