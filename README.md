# 🚀 RCMP: Pathfinding Algo Rush

**RCMP (Robotics and Motion Planning)** presents **Algo Rush**, a high-stakes competitive simulation that pits classic robotics navigation logic against a high-speed AI agent. This project demonstrates the real-world trade-offs between different motion planning strategies in a "race to the goal" format.

---

## 🏁 The Concept: Man vs. Machine

In this "Algo Rush," the player must strategically select an algorithm to navigate a 2D configuration space (-space). While the player chooses between **Grassfire** or **Dijkstra**, they are racing against an AI agent that has a significant **Speed Advantage** and utilizes the efficiency of **A*** or **BFS**.

### **The Competitors**

| Entity | Algorithms | Attributes |
| --- | --- | --- |
| **Player** | **Grassfire** & **Dijkstra** | Standard speed; relies on uniform or cost-based expansion. |
| **AI Agent** | **A*** & **BFS** | **High-speed multiplier**; uses heuristic-guided "rush" logic to find the goal. |

---

## 🧠 Technical Architecture

### **Motion Planning Core**

The system treats the grid as a decomposed cellular environment where the robot must find a collision-free path.

* **A* (The Rusher):** The AI's primary tool. It uses a Manhattan Distance heuristic to "rush" directly toward the goal, minimizing unnecessary cell exploration.
* **Dijkstra:** A cost-aware algorithm that ensures the shortest path by evaluating the cumulative weight of every step.
* **Grassfire (Wavefront):** A recursive-style expansion that ripples out from the goal to create a distance map, allowing for robust navigation.
* **BFS:** An uninformed search that guarantees the shortest path in unweighted grids by exploring layer-by-layer.

### **Real-Time Grid Interaction**

* **Dynamic Obstacles:** Toggle "non-walkable" tiles during the race to re-route the AI or create shortcuts for your own path.
* **Priority Queue Logic:** Optimized path calculations using a custom `PriorityQueue` for  performance during the "Rush" phase.
* **Smooth Interpolation:** Agent movement is handled via Coroutines, allowing for visual speed differences between the Player's algorithm and the AI's "Speed Boost."

---

## 🕹️ How to Play

1. **Setup the Board:** Left-click to place obstacles; use **Shift+Click** for Start and **Ctrl+Click** for Goal.
2. **Choose Your Logic:** Select **Grassfire** or **Dijkstra**.
3. **The Rush:** Trigger the round. The AI will instantly calculate its path using **A*** or **BFS** and begin its high-speed sprint to the red tile.
4. **Analyze:** Observe how the heuristic-guided AI (A*) often beats the more "thorough" expansion algorithms (Dijkstra/Grassfire) despite having the same obstacles.

---

## 📂 Repository Structure

* **`Pathfinder.cs`**: The algorithmic engine containing the logic for A*, Dijkstra, BFS, and Grassfire.
* **`GridManager.cs`**: Manages the configuration space, coordinate mapping, and camera scaling.
* **`GameManager.cs`**: Controls the race logic, AI speed advantages, and scoring.

**Would you like me to add a "Speed Multiplier" variable to your code so you can easily tune exactly how much faster the AI agent moves during the rush?**
