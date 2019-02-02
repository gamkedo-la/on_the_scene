Shader "Custom/Waves"
{
    Properties 
    {
        _Color("Color", Color) = (0,0,0,0)
        _Strength("Strength", Range(0,2)) = 1.0
        _Speed("Speed", Range(-200,200)) = 100
        _MainTex ("Texture", 2D) = "white" {}
    }
    
    SubShader
    {
        Tags
        {
        "RenderType" = "Transparent"
        }
        Pass 
        {
        
            Cull Off
            
            CGPROGRAM
            
            #pragma vertex vertexFunc
            #pragma fragment fragmentFunc
            
            #include "UnityCG.cginc"
            
            float4 _Color;
            float _Speed;
            float _Strength;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            struct vertexData
            {
                float4 position : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            struct vertexOutput 
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            vertexOutput vertexFunc(vertexData v) 
            {
                vertexOutput o;
                
                float4 worldPos = mul(unity_ObjectToWorld, v.position);
                
                float displacement = (cos(worldPos.y) + cos(worldPos.x + (_Speed * _Time)));
                worldPos.y = worldPos.y + (displacement * _Strength);
                o.pos = mul(UNITY_MATRIX_VP, worldPos); 
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            float4 fragmentFunc(vertexOutput o) : SV_Target
            {
                return tex2D(_MainTex, o.uv) * _Color;
            }
            
            ENDCG
        }
    }
}
