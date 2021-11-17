Shader "Unlit/03_Toon"
{
	Properties
	{
		_red("red",Range(0,1)) = 1
		_green("green",Range(0,1)) = 1
		_blue("blue",Range(0,1)) = 1
		//_alpha("alpha",Range(0,1))=1

		_Color("Color",Color) = (1,0,0,1)

		_specular("specular",Range(0,1000)) = 15

		_ambient("ambient",Range(0,1)) = 1

		_light("light",Range(0,1000))=1
	}

		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		//LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work

			#include "UnityCG.cginc"
			fixed _red;
			fixed _green;
			fixed _blue;
			//fixed _alpha;
			fixed4 _Color;
			fixed _specular;
			fixed _ambient;
			fixed _light;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				/*float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)*/
				float4 vertex :SV_POSITION;
				float3 worldPosition : TEXCORD1;
				float3 normal : NORMAL;
			};



			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPosition = mul(unity_ObjectToWorld, v.vertex);
				o.normal = UnityObjectToWorldNormal(v.normal);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				float3 eyeDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPosition);
				float3 halfVec = normalize(_WorldSpaceLightPos0 + eyeDir);
				float intensity = saturate(dot(normalize(i.normal),halfVec));
				float spec = pow(intensity, _specular);


				fixed4 ambient = fixed4(_Color.r * _ambient, _Color.g * _ambient, _Color.b * _ambient, _Color.a * 0.2f);//暗めの単色
				fixed4 diffuse = fixed4(intensity * _Color.r, intensity * _Color.g, intensity * _Color.b, 1);//Diffuseの計算
				fixed4 specular = fixed4(spec*_light, spec*_light, spec*_light, 1); //Specularの計算

				fixed4 up_spe = step(0.1f, intensity) * specular;
				
				fixed4 up = smoothstep(0.29f,0.3f, intensity)*_Color;

				fixed4 down = (1 - smoothstep(0.29f,0.3f, intensity))*ambient;

				fixed4 toon = up + down;

				fixed4 ads = ambient + diffuse*toon + up_spe;
				return ads;
			}

			ENDCG
		}
	}
}