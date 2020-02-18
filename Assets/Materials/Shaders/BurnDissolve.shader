// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "AM/BurnDissolve"
{
	Properties
	{
		_Mask("Mask", 2D) = "white" {}
		_DistortionMap("DistortionMap", 2D) = "bump" {}
		_DistortionAmount("DistortionAmount", Range( 0 , 3)) = 1
		_ScrollSpeed("ScrollSpeed", Range( 0 , 1)) = 0
		_Burn("Burn", Range( 0 , 1.1)) = 0
		_Dissolve("Dissolve", Range( 0 , 1.1)) = 0
		_TextureSample3("Texture Sample 3", 2D) = "white" {}
		_PolygonPrototype_Texture_Metallic("PolygonPrototype_Texture_Metallic", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		AlphaToMask On
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _TextureSample3;
		uniform float4 _TextureSample3_ST;
		uniform sampler2D _Mask;
		uniform sampler2D _DistortionMap;
		uniform float4 _DistortionMap_ST;
		uniform float _DistortionAmount;
		uniform float _ScrollSpeed;
		uniform float _Burn;
		uniform float _Dissolve;
		uniform sampler2D _PolygonPrototype_Texture_Metallic;
		uniform float4 _PolygonPrototype_Texture_Metallic_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TextureSample3 = i.uv_texcoord * _TextureSample3_ST.xy + _TextureSample3_ST.zw;
			o.Albedo = tex2D( _TextureSample3, uv_TextureSample3 ).rgb;
			float4 color15 = IsGammaSpace() ? float4(0.6132076,0.320483,0,0) : float4(0.3341808,0.08379358,0,0);
			float4 color16 = IsGammaSpace() ? float4(1,0.740669,0,0) : float4(1,0.5081033,0,0);
			float2 uv_DistortionMap = i.uv_texcoord * _DistortionMap_ST.xy + _DistortionMap_ST.zw;
			float temp_output_13_0 = ( _Time.y * _ScrollSpeed );
			float2 panner11 = ( temp_output_13_0 * float2( -1,-1 ) + float2( 0,0 ));
			float2 uv_TexCoord9 = i.uv_texcoord + panner11;
			float4 lerpResult17 = lerp( color15 , color16 , tex2D( _Mask, ( ( (UnpackNormal( tex2D( _DistortionMap, uv_DistortionMap ) )).xy * _DistortionAmount ) + uv_TexCoord9 ) ).r);
			float4 temp_cast_1 = (2.0).xxxx;
			float2 panner39 = ( temp_output_13_0 * float2( -1,-1 ) + i.uv_texcoord);
			float4 tex2DNode23 = tex2D( _Mask, ( ( (UnpackNormal( tex2D( _DistortionMap, panner39 ) )).xy * 0.1 ) + i.uv_texcoord ) );
			float temp_output_24_0 = step( tex2DNode23.r , _Burn );
			float temp_output_50_0 = step( tex2DNode23.r , ( 1.0 - ( _Dissolve / 1.1 ) ) );
			float temp_output_52_0 = ( temp_output_50_0 - step( tex2DNode23.r , ( 1.0 - _Dissolve ) ) );
			float4 temp_cast_2 = (temp_output_52_0).xxxx;
			float4 temp_cast_3 = (temp_output_52_0).xxxx;
			o.Emission = ( ( ( ( pow( lerpResult17 , temp_cast_1 ) * 2.0 ) * ( temp_output_24_0 + ( temp_output_24_0 - step( tex2DNode23.r , ( _Burn / 1.1 ) ) ) ) ) - temp_cast_2 ) - temp_cast_3 ).rgb;
			float2 uv_PolygonPrototype_Texture_Metallic = i.uv_texcoord * _PolygonPrototype_Texture_Metallic_ST.xy + _PolygonPrototype_Texture_Metallic_ST.zw;
			o.Metallic = tex2D( _PolygonPrototype_Texture_Metallic, uv_PolygonPrototype_Texture_Metallic ).r;
			o.Alpha = temp_output_50_0;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			AlphaToMask Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=17700
3236.8;-308.8;1190;1078;-479.9106;1289.923;2.363219;True;True
Node;AmplifyShaderEditor.SimpleTimeNode;12;-1055.69,96.84344;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-1228.59,316.5441;Inherit;False;Property;_ScrollSpeed;ScrollSpeed;4;0;Create;True;0;0;False;0;0;0.088;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;40;-2084.984,848.1888;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-750.1894,289.2439;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;39;-1720.469,896.53;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-1,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;3;-1000.665,-409.5916;Inherit;True;Property;_DistortionMap;DistortionMap;2;0;Create;True;0;0;False;0;None;a1cd639c996f7ae49a9ff31b9496ca4c;True;bump;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SamplerNode;38;-1427.68,886.295;Inherit;True;Property;_TextureSample4;Texture Sample 4;3;0;Create;True;0;0;False;0;-1;None;a1cd639c996f7ae49a9ff31b9496ca4c;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;4;-725.0649,-392.7916;Inherit;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;8;-471.6159,-164.8058;Inherit;False;Property;_DistortionAmount;DistortionAmount;3;0;Create;True;0;0;False;0;1;0.87;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;11;-512.2892,216.4441;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;-1,-1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1071.938,1103.054;Inherit;False;Constant;_Heatwave;Heatwave;9;0;Create;True;0;0;False;0;0.1;0.02;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;42;-1042.579,812.2931;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;6;-420.9157,-394.9059;Inherit;False;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-150.5158,-287.006;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;45;-598.5881,1017.74;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-646.5278,781.5341;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;9;-139.1889,109.8438;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;33;754.4133,430.9601;Inherit;False;Constant;_DivideAmount;DivideAmount;8;0;Create;True;0;0;False;0;1.1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;1;62.13724,-590.6638;Inherit;True;Property;_Mask;Mask;1;0;Create;True;0;0;False;0;c32efdffcbd28e942bfb521dcc5704b2;c32efdffcbd28e942bfb521dcc5704b2;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.SimpleAddOpNode;10;261.3911,-147.4896;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;26;645.3782,171.4606;Inherit;False;Property;_Burn;Burn;5;0;Create;True;0;0;False;0;0;0;0;1.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;46;-298.2581,714.276;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;32;1066.33,304.2955;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;23;622.5621,-59.95823;Inherit;True;Property;_TextureSample2;Texture Sample 2;5;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;16;668.0825,-712.0576;Inherit;False;Constant;_Hot;Hot;5;0;Create;True;0;0;False;0;1,0.740669,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;54;697.7382,814.4648;Inherit;False;Property;_Dissolve;Dissolve;6;0;Create;True;0;0;False;0;0;0;0;1.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;613.2479,-353.6257;Inherit;True;Property;_TextureSample0;Texture Sample 0;2;0;Create;True;0;0;False;0;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;15;677.2476,-926.052;Inherit;False;Constant;_Warm;Warm;5;0;Create;True;0;0;False;0;0.6132076,0.320483,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;31;1376.424,217.246;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;24;1047.087,-30.67904;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;1201.941,-1036.023;Inherit;False;Constant;_Float1;Float 1;5;0;Create;True;0;0;False;0;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;17;1187.454,-735.9169;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;55;1111.531,733.0098;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;48;1427.249,761.5312;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;34;1673.426,164.7534;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;47;1417.651,1022.908;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;20;1466.581,-756.7426;Inherit;False;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;50;1741.059,736.1925;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;51;1749.785,1003.367;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;1783.264,-722.0643;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;35;2027.056,-25.7563;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;52;2051.704,820.4044;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;2444.41,-381.6922;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;58;2635.235,-68.5186;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;30;3530.411,-365.8378;Inherit;True;Property;_PolygonPrototype_Texture_Metallic;PolygonPrototype_Texture_Metallic;8;0;Create;True;0;0;False;0;-1;9dec2143ea353f54797f8eb01cf46827;bea7fa376f932ba419f3d1fc95bd1a2b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;29;3268.331,-1112.437;Inherit;True;Property;_TextureSample3;Texture Sample 3;7;0;Create;True;0;0;False;0;-1;None;962dfb6e8875c2c449032428eab5feff;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;59;2849.935,289.3158;Inherit;False;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;57;-750.1902,561.5811;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3119.031,-176.6416;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;AM/BurnDissolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Transparent;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;True;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;13;0;12;0
WireConnection;13;1;14;0
WireConnection;39;0;40;0
WireConnection;39;1;13;0
WireConnection;38;0;3;0
WireConnection;38;1;39;0
WireConnection;4;0;3;0
WireConnection;11;1;13;0
WireConnection;42;0;38;0
WireConnection;6;0;4;0
WireConnection;7;0;6;0
WireConnection;7;1;8;0
WireConnection;43;0;42;0
WireConnection;43;1;44;0
WireConnection;9;1;11;0
WireConnection;10;0;7;0
WireConnection;10;1;9;0
WireConnection;46;0;43;0
WireConnection;46;1;45;0
WireConnection;32;0;26;0
WireConnection;32;1;33;0
WireConnection;23;0;1;0
WireConnection;23;1;46;0
WireConnection;2;0;1;0
WireConnection;2;1;10;0
WireConnection;31;0;23;1
WireConnection;31;1;32;0
WireConnection;24;0;23;1
WireConnection;24;1;26;0
WireConnection;17;0;15;0
WireConnection;17;1;16;0
WireConnection;17;2;2;1
WireConnection;55;0;54;0
WireConnection;55;1;33;0
WireConnection;48;0;55;0
WireConnection;34;0;24;0
WireConnection;34;1;31;0
WireConnection;47;0;54;0
WireConnection;20;0;17;0
WireConnection;20;1;22;0
WireConnection;50;0;23;1
WireConnection;50;1;48;0
WireConnection;51;0;23;1
WireConnection;51;1;47;0
WireConnection;21;0;20;0
WireConnection;21;1;22;0
WireConnection;35;0;24;0
WireConnection;35;1;34;0
WireConnection;52;0;50;0
WireConnection;52;1;51;0
WireConnection;27;0;21;0
WireConnection;27;1;35;0
WireConnection;58;0;27;0
WireConnection;58;1;52;0
WireConnection;59;0;58;0
WireConnection;59;1;52;0
WireConnection;0;0;29;0
WireConnection;0;2;59;0
WireConnection;0;3;30;0
WireConnection;0;9;50;0
ASEEND*/
//CHKSM=182388F6B2BC49AF17120BBB76453C002B910E50