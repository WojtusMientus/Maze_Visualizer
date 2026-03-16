# Maze Visualizer

**Maze Visualizer** is a small interactive application built in **Unity** in **March 2025** that visualizes maze generation algorithms in real time.

It was created as a short exploratory project while I was transitioning from Unity toward Unreal Engine and C++. The goal was simple: take something I had been curious about for a while - **procedural maze generation** - and turn it into a small interactive tool that made the underlying algorithms easier to observe and compare.

![Maze Generation Preview](Assets/Gifs/Main%20App.gif)


## Project Overview

This project was developed in roughly a week as a focused application rather than a full game. Users can choose between multiple maze generation algorithms, adjust maze size, control generation speed, and inspect a short pseudocode summary for each approach.

The project is fairly small in scope, but I still like it because it reflects my interest in systems, algorithms, and gave me a simple way to explore maze generation in practice.


## Included Algorithms

- **Depth First Search**
- **Simplified Prim’s Algorithm**
- **Hunt & Kill**


## Algorithm Previews

### Depth First Search
Creates long, winding passages with a strong bias toward deep branching paths.  
![DFS](Assets/Gifs/DFS%20Algo.gif)

### Simplified Prim’s Algorithm
Generates more uniform, open mazes by repeatedly selecting adjacent cells and connecting them back to the existing maze.
![Prim](Assets/Gifs/Prims%20Algo.gif)

### Hunt & Kill
Combines random walks with systematic scanning, producing a less predictable generation pattern.  
![Hunt and Kill](Assets/Gifs/Hunt%20&%20Kill%20Algo.gif)


## Try It Out

A playable build is available on [itch.io](https://wojciech-maciejewski.itch.io/maze-visualizer).
