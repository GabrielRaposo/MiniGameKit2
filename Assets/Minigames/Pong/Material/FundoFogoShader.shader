Shader "Custom/FundoFogoShader"{
	Properties{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (0, 0, 0, 1)
		_FirePosition("FirePosition", float) = 0
	}

		SubShader{
			Tags{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Blend SrcAlpha OneMinusSrcAlpha

			ZWrite off
			Cull off

			Pass{

				CGPROGRAM

				#include "UnityCG.cginc"

				#pragma vertex vert
				#pragma fragment frag

				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _Color;
				float _FirePosition;

				struct appdata {
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					fixed4 color : COLOR;
				};

				struct v2f {
					float4 position : SV_POSITION;
					float2 uv : TEXCOORD0;
					fixed4 color : COLOR;
				};

				v2f vert(appdata v) {
					v2f o;
					o.position = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.color = v.color;
					return o;
				}

				fixed4 disperseEffect(float x) {

					fixed4 returnValue;

					
					if (abs(x - 0.5) * 2 < _FirePosition) {
						returnValue = _Color;
					}
					else {
						returnValue = fixed4(1, 1, 1, 0);
					}
					
					return returnValue;
				}

				fixed4 frag(v2f i) : SV_TARGET{
					fixed4 col = tex2D(_MainTex, i.uv);
					col *= _Color;
					col *= i.color;
					col *= disperseEffect(i.uv.x);
					return col;
				}

				ENDCG
			}
		}
}