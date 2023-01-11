Shader "MILLDIA/TestShader"{
    Properties{
        _MainTexture("MainTexture",2D) = ""{}
        _F("_F",Range(1,20)) = 10
    }

    SubShader{
        Tags {"Queue" = "Transparent" "RenderType" = "Opaque"}
        CGPROGRAM
        #pragma surface surf Lambert

        struct Input {
            float4  color : COLOR;
        };

        void surf (Input IN,inout SurfaceOutput o){
            o.Albedo = 1;
        }

        ENDCG
    }

    Fallback "Diffuse"
}