Shader "Custom/Hologram"
{
	Properties
	{
		_Color("Color", Color) = (0, 0, 0, 1)
		[NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
		_Density("Scanline Density", Float) = 0
		_Speed("Scanline Speed", Float) = 0
		_Bias("Scanline Intensity", Range(-1, 1)) = 0
    }
    SubShader
    {
        Tags { "Queue" =  "Transparent" "RenderType"="Transparent" }
        LOD 100
		ZWrite Off
		Blend SrcAlpha One

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
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(2)
                float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float4 objVertex : TEXTCOORD1;
            };

			fixed4 _Color;
			float _Density, _Speed, _Bias;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
				o.objVertex = mul(unity_ObjectToWorld, v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = v.normal;
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			float noise (float2 uv)
			{
				return frac(sin(dot(uv, float2(12.9898, 78.233 * _Time.x))) * 43758.5453 * _Time);
			}

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				// hologram effect
				col *= _Color * max(0, noise(i.uv) * cos(i.objVertex.y * _Density + (-1 * _Time.x * _Speed)) + _Bias);
				// color noise
				fixed4 noiseCol = fixed4(noise(i.uv), noise(i.uv), noise(i.uv), 1);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col * noiseCol;
            }
            ENDCG
        }
    }
	SubShader
	{
		Tags { "RenderType" = "Opaque"}

		Pass
		{
			CGPROGRAM
			#pragma vertex jitter
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(2)
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
			};			

			sampler2D _MainTex;
			float4 _MainTex_ST;

			v2f jitter(appdata v)
			{
				v2f o;
				v.vertex.xyz += v.normal * 100;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = v.normal;
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			float noise(float2 uv)
			{
				return frac(sin(dot(uv, float2(12.9898, 78.233 * _Time.x))) * 43758.5453 * _Time);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
