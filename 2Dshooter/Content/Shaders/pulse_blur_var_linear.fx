sampler s;



float foo;
float var;


float4 PixelShaderFunction(float2 uv : TEXCOORD0, float4 c: COLOR0) : COLOR0
{
    // TODO: add your pixel shader code here.
 
        
    //blur
    float4 color;
    float4 color1;
    float4 color2;
    
    float2 oriuv;
    
    float pulse_amplitude = 0.5;
    float pulse_frequency = var;
    
    oriuv = uv.xy;
    
    
    
    color = tex2D(s, uv); //normal
    color1 = tex2D(s, uv.xy);
	color2 = tex2D(s, uv.xy);
	
	
	// Begin y = -x diagonal
	
			// To get y= -x diagonal
					uv.x = (  + uv.x + pulse_amplitude*(pulse_frequency) );
					uv.y = (  + uv.y + pulse_amplitude*(pulse_frequency) );
		    		
        
    color1 = tex2D(s, uv.xy);
    
								//Reset uv
								uv.xy = oriuv;
							    
    
			// To get y= -x diagonal
					uv.x = (  + uv.x - pulse_amplitude*(pulse_frequency) );
					uv.y = (  + uv.y - pulse_amplitude*(pulse_frequency) );
		    
    color1 += tex2D(s, uv.xy);
          
    // End of y = -x  diagonal 
    
    
         
								//Reset uv
								uv.xy = oriuv;
							     
        
    // Begin y = x -b diagonal
        
       
    
			// To get y= x -b diagonal
					uv.x = (  + uv.x + pulse_amplitude*(pulse_frequency) );
					uv.y = (  + uv.y - pulse_amplitude*(pulse_frequency) );
		    
    color2 = tex2D(s, uv.xy);
	
	
	
								//Reset uv
								uv.xy = oriuv;
						
			// To get y= x -b diagonal
					uv.x = (  + uv.x - pulse_amplitude*(pulse_frequency) );
					uv.y = (  + uv.y + pulse_amplitude*(pulse_frequency) );
		    
	color2 += tex2D(s,uv.xy);
	
	// End of y = x -b diagonal
	
	
	
	color = color + color1 + color2;
	
	
	
      
    return color ;

}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.


        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
