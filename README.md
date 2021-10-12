WU Fall 2021 | CSMA 302 | Lab #3
---
# Compute Shader Cellular Automata

We're going to implement four types of cellular automata :
 - Wolfram Elementary CA
 - Conway's Game of Life
 - Rock-Paper-Scissors
 - Reaction Diffusion

and also use *Procedural Rendering* to render in 3D. I will do one version in class and you will choose one other CA to turn into 3D. 

## Due Date (part 1 due before class 10.6) 

In the RockPaperScissors scene, implement a rock paper scissors CA using the rockPaperScissrs.compute shader: 

 - A cell changes color when the number of neighbouring cells that beats it is above a threshold. (start with 2)
 - Green beats Red, Blue beats Green, Red beats Blue.
 - Count the number of R, G, and B neighbors and set tne new coloor accoordingly.
 - a cell can only be black OR red OR green, OR blue. 
 - use keys 1,2,3 to select R, G ,B color. (this part has already been implemented in the branch main)


**The full assignment is due on Sunday October 10th before midnight.**

## Resources
rock paper scissors: 
https://softologyblog.wordpress.com/2018/03/23/rock-paper-scissors-cellular-automata/
 
 Wolfram:
https://mathworld.wolfram.com/ElementaryCellularAutomaton.html

Reaction-Diffusion
https://www.karlsims.com/rd.html

Conway:
http://pi.math.cornell.edu/~lipa/mec/lesson6.html


## Grading
15 points for each working cellular automata : 
 - wolfram
 - conway (with mouse input)
 - reaction-diffusion (with mouse input)
 - rock-paper-scissors (with mouse input)

20 points for each 3D visualization
 - one from class
 - one that you pick

10 points for code with comments 
10 points for project organization

there should be 6 scenes total, each with a different version or visualization.
3D visualizations need to have multiple colors and lighting


## Submitting 
(this is also in the syllabus, but consider this an updated version)

1. Submit your work to a branch on github on this repo (branch should be your firstname-lastname)
When you are finished, "Tag" the commit in git as "Complete". You can still work on it after that if you want, I will just grade the latest commit.

2. The project has to run and all the shaders you are using should compile. If it doesn't I'm not going to try to fix it to grade it, I will just let you know that your project is busted and you have to resubmit.  Every time this happens I'll take off 5%. You have 24 hours from when I return it to get it back in, working. 

3. Late projects will lose 10% every 24 hours they are late, after 72 hours the work gets an F. 

4. Obviously plagarism will not be tolerated, there are a small number of students so I can read all your code. Because it is on git it's obvious if you copied some else's. If you copy code without citing the source in a comment, this will be considered plagarism. 



