
Shader "IndieMarc/Darkness2D" {
	Properties{
		//Toggled in editor to disabled effect while working
		[Toggle] _Editor("Disable in Editor", Float) = 0

		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		
		//Filters FX
		_Bit8Mode1Freq("8-bit FX1 Freq", Range (0, 1)) = 0
		_Bit8Mode1Floor("8-bit FX1 Range", Range (0, 0.99)) = 0
		_Bit8Mode2Floor("8-bit FX2 Range", Range (0, 0.99)) = 0

		//Darkness settings
		[HideInInspector] _Bounds("_Bounds", Vector) = (0,0,1,1)
		[HideInInspector] _EdgesRadius("_EdgesRadius", Float) = 0.5
		[HideInInspector] _Opacity("_Opacity", Float) = 1.0

		//12 lights max
		[HideInInspector]_Light1_Pos("_Light1_Pos", Vector) = (0,0,0,1)
		[HideInInspector]_Light2_Pos("_Light2_Pos", Vector) = (0,0,0,1)
		[HideInInspector]_Light3_Pos("_Light3_Pos", Vector) = (0,0,0,1)
		[HideInInspector]_Light4_Pos("_Light4_Pos", Vector) = (0,0,0,1)
		[HideInInspector]_Light5_Pos("_Light5_Pos", Vector) = (0,0,0,1)
		[HideInInspector]_Light6_Pos("_Light6_Pos", Vector) = (0,0,0,1)
		[HideInInspector]_Light7_Pos("_Light7_Pos", Vector) = (0,0,0,1)
		[HideInInspector]_Light8_Pos("_Light8_Pos", Vector) = (0,0,0,1)
		[HideInInspector]_Light9_Pos("_Light9_Pos", Vector) = (0,0,0,1)
		[HideInInspector]_Light10_Pos("_Light10_Pos", Vector) = (0,0,0,1)
		[HideInInspector]_Light11_Pos("_Light11_Pos", Vector) = (0,0,0,1)
		[HideInInspector]_Light12_Pos("_Light12_Pos", Vector) = (0,0,0,1)
	}

	SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off

		CGPROGRAM

#pragma surface surf Lambert alpha vertex:vert

		sampler2D _MainTex;
		fixed4     _Color;
		float4	   _Bounds;
		float	   _EdgesRadius;
		float	   _Opacity;
		float	   _Editor;
		float	   _Bit8Mode1Freq;
		float	   _Bit8Mode1Floor;
		float	   _Bit8Mode2Floor;

		float4     _Light1_Pos;
		float4     _Light2_Pos;
		float4     _Light3_Pos;
		float4     _Light4_Pos;
		float4     _Light5_Pos;
		float4     _Light6_Pos;
		float4     _Light7_Pos;
		float4     _Light8_Pos;
		float4     _Light9_Pos;
		float4     _Light10_Pos;
		float4     _Light11_Pos;
		float4     _Light12_Pos;

		struct Input {
			float2 uv_MainTex;
			float2 location;
		};

		float powerForPos(float4 pos, float2 nearVertex);
		float boundsAtten(float4 bounds, float2 nearVertex);

		//Vertex shader
		void vert(inout appdata_full vertexData, out Input outData) {
			float4 pos = UnityObjectToClipPos(vertexData.vertex);
			float4 posWorld = mul(unity_ObjectToWorld, vertexData.vertex);
			outData.uv_MainTex = vertexData.texcoord;
			outData.location = posWorld.xy;
		}

		//Surface shader
		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 baseColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			//Sum all alpha
			float alpha = (baseColor.a - (
				+ powerForPos(_Light1_Pos, IN.location)
				+ powerForPos(_Light2_Pos, IN.location)
				+ powerForPos(_Light3_Pos, IN.location)
				+ powerForPos(_Light4_Pos, IN.location)
				+ powerForPos(_Light5_Pos, IN.location)
				+ powerForPos(_Light6_Pos, IN.location)
				+ powerForPos(_Light7_Pos, IN.location)
				+ powerForPos(_Light8_Pos, IN.location)
				+ powerForPos(_Light9_Pos, IN.location)
				+ powerForPos(_Light10_Pos, IN.location)
				+ powerForPos(_Light11_Pos, IN.location)
				+ powerForPos(_Light12_Pos, IN.location)
			));

			if (_EdgesRadius > 0.001)
				alpha -= boundsAtten(_Bounds, IN.location);

			//If editor, skip calculations
			if(_Editor > 0.5){
				alpha = 0.2;
			}

			o.Albedo = baseColor.rgb;
			o.Alpha = clamp(alpha, 0.0, 1.0) * _Opacity;
		}

		//Compute alpha near light emit
		//lightPos.xy is position .z is radius .w is attenuation
		float powerForPos(float4 lightPos, float2 pixPos) {
			float radius = lightPos.z + 0.001; //Avoid divide by 0
			float attenuation_radius = fmod(lightPos.w, 100.0) + 0.001;
            float opacity = clamp(lightPos.w / 10000.0, 0.0, 1.0);

			//8bit FX1
			if(_Bit8Mode1Freq > 0.01){
				float bit8floor1 = 1.0 - _Bit8Mode1Floor;
				float manhat_dist_x = floor(abs(lightPos.x-pixPos.x) * bit8floor1 * 100.0)/(bit8floor1 * 100.0);
				float manhat_dist_y = floor(abs(lightPos.y- pixPos.y) * bit8floor1 * 100.0)/(bit8floor1 * 100.0);
				float radius_sub = fmod(manhat_dist_x+manhat_dist_y, _Bit8Mode1Freq/10.0);
				radius -= radius_sub;
			}

			//8bit FX2
			float bit8floor2 = 1.0 - _Bit8Mode2Floor;
			float lx = floor((lightPos.x-pixPos.x)* bit8floor2 * 100.0)/(bit8floor2 * 100.0);
			float ly = floor((lightPos.y-pixPos.y)* bit8floor2 * 100.0)/(bit8floor2 * 100.0);
			float leng = sqrt(lx*lx+ly*ly);

			//Attenuation
			//float atten = clamp(radius - length(lightPos.xy - pixPos.xy), 0.0, radius) / radius; //This line is without 8bit fx
			float atten = clamp(radius - leng, 0.0, radius) / radius;
			return clamp((radius/attenuation_radius)*atten*atten, 0.0, 1.0)*opacity;
		}

		//Compute alpha near bounds
		float boundsAtten(float4 bounds, float2 nearVertex) {
			float radius = _EdgesRadius;
			float atten1 = 1.0 - clamp(radius - abs(bounds.x - nearVertex.x), 0.0, radius) / radius;
			float atten2 = 1.0 - clamp(radius - abs(bounds.y - nearVertex.y), 0.0, radius) / radius;
			float atten3 = 1.0 - clamp(radius - abs(bounds.z - nearVertex.x), 0.0, radius) / radius;
			float atten4 = 1.0 - clamp(radius - abs(bounds.w - nearVertex.y), 0.0, radius) / radius;
			//return (atten1 + atten2 + atten3 + atten4) * (atten1 + atten2 + atten3 + atten4);
			return 1.0 - (atten1) * (atten2) * (atten3) * (atten4) * (atten1) * (atten2) * (atten3) * (atten4);
		}

		ENDCG
	}

	Fallback "Transparent/VertexLit"
}