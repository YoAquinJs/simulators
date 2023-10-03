# Physics Simulators With Unity Engine
## Deployment: https://yoaquinjs.github.io/simulators/

### Description:

Static web page with 2 physics simulators, a chemistry simulator, and a math game. The simulators let you control variables that are used in the simulation, all main simulator scripts are written in c# and the project is compiled using Unity WebGL build, the webpage is programmed using plain HTML, CSS with Bootstrap, and JavaScript.

* **Simple Pendulum**

  Simulation using the accurate solution for the pendulum system, solving the Theta(t) differential equation (without the Sine(Theta) = Theta approximation) using JacobiSN integral elliptic function, as well as for the pendulum period (computed with the extended equation), also including an option for a damped pendulum, taking in account friction, given a coefficient for solving the equation.

  Kinetic and gravitational potential energy are calculated in each position of the pendulum in time, to illustrate the conservation of energy law in the pendulum system, time can be stopped for analyzing a particular time in the pendulum.

  Variables:
  * Initial angle.
  * Gravity acceleration.
  * Pendulum Length.
  * Mass.
  * Friction coefficient.

* **Parabolic Motion**

  Simulation using standard equations for parabolic motion MRU for x(t), and MRUA for y(t), for the ideal motion, neglecting friction, also solves for the max height, the total flight time, and horizontal reach.

  Kinetic and gravitational potential energy are calculated in each motion position with a time slider to return to a given time and analyze.

  Variables:
  * Initial velocity magnitude.
  * Initial velocity angle.
  * Gravity acceleration.
  * Height from the floor.

* **Gas Diffusion**

  Grahan law for gas diffusion is calculated using the molar mass of the particles of gas, and gas motion is simulated with the initial velocity and mass for particle scale (particles are not scaled according to reality).

  Variables:
  * Initial velocity for both gases.
  * Amount of particles for both gases.
  * Types of gas (which change the molar mass).

* **Math Game**

  The game consists of graphing mathematical functions to make the ball go through all the dots in the scene.

  Variables:
  * Equation for function 1.
  * Equation for function 2.

#### References:

* https://nrich.maths.org/content/id/6478/Paul-not%20so%20simple%20pendulum%202.pdf
* https://notasfisicaymatematicas.blogspot.com/2020/02/pendulo-simple-solucion-exacta.html
