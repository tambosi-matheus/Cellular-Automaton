

<h1>🧫 Celular Automaton</h1>
<h3>Target platform: Unity - PC</h3>
<h6>Technologies: Unity, C#, HLSL, multithreading</h6>
I've been passioned about automation, systems and AI since my first Boid code in 2020, when I learned about compute shade and the possibility to use GPU's multithreading power for physics simulations I jumped right into it. This approach let agents go from a max of 1000 using GameObjects to more than a million. Here are some examples of results I filmed when implementing Stigmergy and Conway's Game of Life rules.

<br></br>

<h5>The Rules</h5>

Stigmergy - Agents wander around leaving a trail that decay and diffuse over time. Agents will steer towards trails

Game of Life - Cells life or die based on the quantity of live cells adjacent to them

<small>This explanations are extremely superficial, if you're interested in this topics I suggest reading related papers</small>

<br></br>

<small>Hoover the mouse to see each gif's description</small>

<h3>🦠Sitgmergy</h3>
<table> 
<tr>
<td width=25%>
<img src="https://github.com/tambosi-matheus/Cellular-Automaton/raw/main/GIFs/StigBasic.gif" 
title="The basic black and white representation of the Stigmetry rules, white represents the agent's trails">
</td>

<td width=25%>
<img src="https://github.com/tambosi-matheus/Cellular-Automaton/raw/main/GIFs/StigDiffuse.gif" 
title="With extra randomness and colors, you can create chaos quite easily">
</td>

<td width=25%>
<img src="https://github.com/tambosi-matheus/Cellular-Automaton/raw/main/GIFs/StigRainbow.gif" 
title="By setting the agent color to a random value but still adding all values to the trail, you can create this cool effect
where the edges and new trails are in a 'rainbow' pattern while established trail routes sum to white">
</td>

<td width=25%>
<img src=https://github.com/tambosi-matheus/Cellular-Automaton/raw/main/GIFs/StigRGB.gif 
title="If you change the trails colors to match the agent's, you can quickly paint the whole screen">
</td>
</tr>



<tr>
<td width=25%>
<img src="https://github.com/tambosi-matheus/Cellular-Automaton/raw/main/GIFs/StigLightning.gif" 
title="By adding some extra randomness to the agents angular speed, trails start to get less linear and in a lightstorm-like pattern">
</td>

<td width=25%>
<img src="https://github.com/tambosi-matheus/Cellular-Automaton/raw/main/GIFs/StigAtractor.gif" 
title="If you add an attractor to the middle of the texture, the pattern starts to feel like an expanding fractal, with the middle having small circular paths and the outer part having bigger linear trails">
</td>

<td width=25%>
<img src="https://github.com/tambosi-matheus/Cellular-Automaton/raw/main/GIFs/StigExplosion.gif" 
title="If you change the spawn position to the center, you can create this explosion effect that I find quite fascinating to watch">
</td>

<td width=25%>
<img src=https://github.com/tambosi-matheus/Cellular-Automaton/raw/main/GIFs/StigCell.gif 
title="My final implementation of the Stigmergy rules, changing background agent and trail colors, while also applying a middle attractor and spawn">
</td>
</tr>
</table>



<h3>🧬Game of Life</h3>

<table> 
<tr>

<td width=25%>
<img src="https://github.com/tambosi-matheus/Cellular-Automaton/raw/main/GIFs/GOFBasic.gif" 
title="Basic implementation of the Game of Life, white represent living cells, black represents dead ones">
</td>

<td width=25%>
<img src="https://github.com/tambosi-matheus/Cellular-Automaton/raw/main/GIFs/GOFZoom.gif" 
title="Changing cell's color gradually to black creates this cool fading/trail effect">
</td>

<td width=25%>
<img src="https://github.com/tambosi-matheus/Cellular-Automaton/raw/main/GIFs/GOFColor.gif"
title="Storing the game in a texture and using its state to color a second texture opens a wide range of opportunities to expand the code. This particular implementation reminds me a lot of military camouflages">
</td>

<td width=25%>
<img src=https://github.com/tambosi-matheus/Cellular-Automaton/raw/main/GIFs/GOFRails.gif
title="Changing the game rules also can create different patterns, from some that dies in a matter of seconds to this rail-like visualization">
</td>
</tr>
</table>
