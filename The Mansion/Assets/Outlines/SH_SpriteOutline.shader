Shader "Custom/SH_SpriteOutline"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_OutlineWidth("Outline Width", Range(0.0,0.02)) = 0
		[HideInInspector] _Opacity("Opacity", Range(0.0,1.0)) = 0
		
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

		Cull Off
		Lighting Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float3 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float3 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST, _OutlineColor;
			float _OutlineWidth, _Opacity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 col = tex2D(_MainTex, i.uv);
				col.rgb *= i.color;

				#define DIV_SQRT_2 0.70710678118
				float2 directions[8] = { float2(1, 0), float2(0, 1), float2(-1, 0), float2(0, -1),
				float2(DIV_SQRT_2, DIV_SQRT_2), float2(-DIV_SQRT_2, DIV_SQRT_2),
				float2(-DIV_SQRT_2, -DIV_SQRT_2), float2(DIV_SQRT_2, -DIV_SQRT_2) };
				
				//generate outline
				float maxAlpha = 0;
				for (uint index = 0; index < 8; index++) {
					float2 sampleUV = i.uv + directions[index] * _OutlineWidth;
					maxAlpha = max(maxAlpha, tex2D(_MainTex, sampleUV).a);
				}
				//remove core
				float border = max(0, maxAlpha - tex2D(_MainTex, i.uv).a) * _Opacity;
				
				col.a = max(col.a, border);
				//col.a = 1;
				//col.rgb = lerp(col.rgb, _OutlineColor.rgb, border);
				col.rgb = lerp(col.rgb, _OutlineColor.rgb, border);

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
