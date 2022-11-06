# Physics Simulators With Unity Engine
## Video Demo:  https://youtu.be/JECzmVXVo2s
<br/>

### Description:
<br/>

Static Webpage with 3 physics simulators, simple pendulum with accurate solution to non aproximated differential equation and pendulum with friction, parabolic motion with alterable initial conditions (gravity, velocity, height) and time evaluation and gas diffusion graham law for different molecules and elements in gas state with different proportion of both and different initial velocity in particles of both gases, all simulators contain UI elements (maninly sliders) to control variables that are used in the simulation, all main simulator scripts are written in c# and the project its compiled in Unity WebGl build, the webpage its programmed using plain html, css with bootstrap and javascript for a cool background icon effect.

## Simple Pendulum

Simulation using accurate solution for the pendulums Theta(t) differential equation (without the Sine(Theta) = Theta aproximation) using JacobiSN integral eliptic function, as well as for the pendulum period (computed with the extended equation), include option for pendulum with air resistence in wich the differential equation is solved with the added friction term with a given friction coeficient.

Kinetic and gravitational potential energy are calculated in each position of the pendulum in time (except with friction) to ilustrate the conservation of energy law in the pendulum system, time can be stopped for analizyng a particular time in the pendulum.

Initial varibales that can be modified are: The initial angle, the gravity acceleration, the length from the mass to the axis, the mass attached and the friction coeficient.

##### References:

* https://nrich.maths.org/content/id/6478/Paul-not%20so%20simple%20pendulum%202.pdf
* https://notasfisicaymatematicas.blogspot.com/2020/02/pendulo-simple-solucion-exacta.html

## Parabolic Motion

Simulation using standar equations for parabolic motion in both axis (MRU and MRUA equations for X(t)) for the ideal motion neglecting friction, also solves for the max height, the total flight time and horizontal reach.

Kinetic and gravitational potential energy are calculated in each position of the motion with a time slider to return to a given time and analizy.

Initial varibales that can be modified are: The initial velocity magnitude, the initial velocity angle, the gravity acceleration, the height from the floor.

## Gas Difusion

Grahan law for gas diffusion calculated using the molar mass of the particles of gas, and gas motion simulated with the initial velocity and mass for particle scale (particles are not scaled according to reality).

Initial varibales that can be modified are: The initial velocity for both gases, the amount of particles for both gases, the two types of gas wich changes the molar mass.
