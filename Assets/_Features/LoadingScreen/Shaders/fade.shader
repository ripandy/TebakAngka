Shader "Custom/Unlit/Fade"
{
    Properties
    {
        //_MainTex ("Texture", 2D) = "white" {}
        _col ("col",color) = (0.0,0.0,0.0,0.0)
        _pow ("pos",float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent+1500"}
        LOD 100
        ZTest always
        Cull off
        ZWrite off
			Stencil {
                Ref 28
                Comp notEqual
                Pass keep
            }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            fixed4 _col;
            float _pow;

            static float2 screenvertex[4] =	{float2(-1.0, 1.0),
								 float2( 1.0, 1.0),
								 float2(-1.0,-1.0),
								 float2( 1.0,-1.0)};
								 

			struct v2f {
				float4 screenuv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
			v2f vert () {
				v2f output = (v2f)0;
				return output;
			}
			[maxvertexcount(4)]
			void geom (point v2f IN[1],inout TriangleStream< v2f > OUT)	{
				v2f output;
                [unroll] for(uint index=0; index<4; index++){
					output.vertex = float4(screenvertex[index],1.0,1.0);
                	output.screenuv = ComputeGrabScreenPos(output.vertex);
					OUT.Append(output);
				}
				OUT.RestartStrip();
			}

            fixed4 frag (v2f i) : SV_Target
            {
                float alpha = pow(distance(i.screenuv,float2(0.5,0.5)),_pow);
                // sample the texture
                fixed4 col;// = tex2D(_MainTex, i.uv);
                col = _col*alpha;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
