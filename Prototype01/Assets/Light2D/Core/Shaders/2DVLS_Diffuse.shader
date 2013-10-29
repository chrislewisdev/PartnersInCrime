Shader "2DVLS/Diffuse" {
    Properties
    {
        _MainTex ("Base (RGB) Trans. (Alpha)", 2D) = "white" { }
        _Color("Main Color", Color) = (.5,.5,.5,0)
    }

    Category
    {
        //ZWrite On
        Cull Off
        Lighting On

        SubShader
        {
		    Pass 
			{
				ColorMask 0
			}

            Pass
            {    		
				blend SrcColor DstColor				
				SetTexture [_MainTex]
				{
					combine texture + primary
				}
                                       
            }
        } 
    }

	Fallback "Diffuse"
}