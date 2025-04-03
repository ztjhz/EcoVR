# EcoVR: A Virtual Ecosystem Simulator

**Step into a vibrant virtual forest and witness the intricate dance of life. EcoVR is an immersive Virtual Reality (VR) simulation that allows you to explore, interact with, and analyze a dynamic ecosystem.**

![game-scene-bird-view](https://github.com/user-attachments/assets/b949f206-0958-41cf-a378-2e63dc04f11a)
*Observe the ecosystem from a bird's eye view.*

![feeding](https://github.com/user-attachments/assets/bdf9e136-b682-4651-9b76-62f5575f18a6)
*Interact directly by feeding animals.*

## The Vision

Ecosystem simulations are powerful tools for understanding ecological complexities. VR enhances these simulations, making abstract concepts tangible and engaging. While various simulations exist, EcoVR specifically addresses the gap in VR-based tools designed for observing and analyzing **animal interactions within forest ecosystems**.

This project explores the benefits of VR simulations for ecological education and research, introducing EcoVR as an immersive tool demonstrating this potential.

## ✨ Key Features

* **🌲 Immersive Forest Environment:** Explore a rich, stylized forest setting built with high-quality assets.
* **🦅 Dual View Modes:**
    * **Bird's Eye View:** Observe the entire ecosystem, spawn animals, control weather, adjust time speed, and zoom. Includes an interactive tutorial for new users.
    * **First-Person View:** Experience the forest at ground level, move freely, use tools, and interact directly with animals using intuitive gestures.
* **🧠 Intelligent Animal Agents:**
    * **Predator-Prey Dynamics:** Witness autonomous predators (Wolves) hunting prey (Sheep, Goats) using Finite State Machine (FSM) based AI. *(FSM diagrams pending)*
    * **Evasive Behaviours:** Prey actively detect and flee from nearby predators.
* **🧬 Realistic Population Dynamics:**
    * **Natural Birth/Death:** Population controls inspired by Conway's Game of Life principles, considering loneliness (underpopulation) and competition (overpopulation).
    * **Hunger & Hydration:** Animals possess needs that deplete over time, influenced by actions (hunting, foraging) and weather, potentially leading to starvation or dehydration.
* **☀️ Dynamic Weather & Time System:** Experience Sunny, Stormy, Foggy, Snowy conditions, and Day/Night cycles, all impacting animal behaviour, detection ranges, speed, and survival needs (powered by Tenkoku Dynamic Sky).
* **🛠️ Interactive Tools & Actions:**
    * **Binoculars & Magnifying Glass:** Observe animals up close or from afar using realistic render-texture based tools.
    * **Feeding & Watering:** Directly provide food and water to influence animal hunger/hydration levels.
    * **Detection Radius Visualisation:** Toggleable overlays (Red for predators, Blue for prey) to understand animal perception ranges.
* **📊 In-Depth Analytics Lab:**
    * Step into a futuristic research pod environment to visualize simulation data.
    * Track animal counts, population trends over time, spatial distribution heatmaps, compare species attributes (radar charts), and view population forecasts (ARIMA model). Powered by XCharts.

## 📸 Screenshots

<details>
<summary>Click to view more screenshots</summary>

| Feature             | Screenshot                                                                                             |
| :------------------ | :----------------------------------------------------------------------------------------------------- |
| **Gameplay** |                                                                                                        |
| Bird's Eye View     | ![game-scene-bird-view](https://github.com/user-attachments/assets/b949f206-0958-41cf-a378-2e63dc04f11a) |
| First Person        | *Placeholder for a good First Person screenshot* |
| Feeding             | ![feeding](https://github.com/user-attachments/assets/bdf9e136-b682-4651-9b76-62f5575f18a6)             |
| Detection Radius    | ![radius](https://github.com/user-attachments/assets/21eec1ae-5de6-4b09-a0a3-be0bc2fb5d2a)             |
| Predator Hunt       | ![predator-prey](https://github.com/user-attachments/assets/INSERT_FIGURE_4.6_URL_HERE)                  | | Binoculars/Magnifier| ![tools](https://github.com/user-attachments/assets/INSERT_FIGURE_4.10_URL_HERE)                 | | Status System       | ![status-system](https://github.com/user-attachments/assets/046c3644-3235-414a-bc37-716bfbdc21df)       |
| Weather (Rain)      | ![weather-rain](https://github.com/user-attachments/assets/INSERT_FIGURE_4.9_URL_HERE)                   | | **UI & Analytics** |                                                                                                        |
| Main Menu           | ![main-menu](https://github.com/user-attachments/assets/6084b066-7136-4758-b292-e116de2dca34)           |
| Tutorial            | ![tutorial](https://github.com/user-attachments/assets/ccd95207-156a-42a1-97ac-fd0eb11806a0)           |
| Analytics Lab       | ![analytics-lab](https://github.com/user-attachments/assets/6347ecf7-9f3b-43e0-ad92-98b29fe3059e)       |
| Analytics Scene Env | <img width="581" alt="analytics scene" src="https://github.com/user-attachments/assets/38e848c3-fc14-43ef-8683-aff5c1cc1305" /> |
| Population Count    | ![population count](https://github.com/user-attachments/assets/b4370df9-97cc-4984-9d25-d07b30bb73d1)     |
| Population Graph    | ![population graph](https://github.com/user-attachments/assets/d594b5ee-1f31-40d2-affc-e73ceb8ac1aa)     |
| Population Heatmap  | ![population heatmap](https://github.com/user-attachments/assets/1a64fbf9-3959-4748-b17d-2d1ed1b72745) |
| Heatmap Over Time   | ![population heatmap 2](https://github.com/user-attachments/assets/6b4f63f2-0a39-4ac6-9ce9-2e40bdded84c) | | Population Attributes| ![population attributes](https://github.com/user-attachments/assets/a561bd48-590e-4b31-a05c-7eaeaf0dd059)|
| Population Prediction| ![population prediction](https://github.com/user-attachments/assets/7f6d797b-573c-4e98-9d14-bfc25a4f73a8)|

</details>

## 💻 Tech Stack

* **Engine:** Unity (Specify Version if known, e.g., Unity 2022.3.x)
* **Language:** C#
* **VR Integration:** Unity XR Interaction Toolkit, Meta Quest Link
* **Target Hardware:** Meta Quest 3 (Tested with Meta Quest Link)
* **Charting:** XCharts ([GitHub Link](https://github.com/XCharts-Team/XCharts))
* **Development Tools:** XR Device Simulator, Git

## 🚀 Getting Started

*(This section needs specific details based on how someone would run/use your project. Below is a template assuming it's run from the Unity Editor)*

**Prerequisites:**

* Unity Hub and Unity Editor (Specify Version if known) installed.
* Git installed.
* Meta Quest 3 headset and Oculus Desktop App installed (for Meta Quest Link).
* Required Unity Asset Store assets imported (see Acknowledgements).

**Installation & Running:**

1.  **Clone the repository:**
    ```bash
    git clone [https://github.com/your-username/EcoVR.git](https://www.google.com/search?q=https://github.com/your-username/EcoVR.git) # Replace with your repo URL
    cd EcoVR
    ```
2.  **Open the project** in Unity Hub, ensuring you select the correct Unity Editor version.
3.  **Configure VR:** Set up your Meta Quest 3 with Quest Link according to Meta's instructions.
4.  **Open the Main Scene:** Navigate to `Assets/Scenes/` (or your specific path) and open the `MainMenuScene`.
5.  **Press Play** in the Unity Editor to start the simulation in your VR headset.

*Alternatively:*

1.  Download the latest release from the [Releases page](link-to-releases).
2.  Transfer the apk to your Meta Quest 3
3.  Play!

## 📦 Assets & Acknowledgements

EcoVR utilizes several excellent assets from the Unity Asset Store to create its immersive environments and features:

* **Sci-Fi Styled Modular Pack:** [Link](https://assetstore.unity.com/packages/3d/environments/sci-fi/sci-fi-styled-modular-pack-82913) (Used in Main Menu & Analytics Scene)
* **Stylised Nature Kit Lite:** [Link](https://assetstore.unity.com/packages/3d/environments/stylized-nature-kit-lite-176906) (Used in Analytics Scene)
* **Dreamscape Nature Mountains:** [Link](https://assetstore.unity.com/packages/3d/environments/fantasy/dreamscape-nature-mountains-stylized-open-world-environment-264352) (Used in Game Scene)
* **Tenkoku Dynamic Sky:** [Link](https://assetstore.unity.com/packages/tools/particles-effects/tenkoku-dynamic-sky-34435) (Used for weather & sky in Game Scene)
* **Animal Models:** (Link TBD - See Report Section 3.2)
* **Low Poly Food Lite:** [Link](https://assetstore.unity.com/packages/3d/props/food/low-poly-food-lite-258693) (Used for feeding interaction)
* **XCharts:** [Link](https://github.com/XCharts-Team/XCharts) (Used for data visualization)

Special thanks to the creators of these assets!

## 🔮 Future Work

EcoVR has potential for further development. Some planned or potential enhancements include:

* Expanding species diversity and environment types (tundra, savannah).
* Allowing real-time modification of animal attributes and terrain.
* Implementing an animal evolution/adaptation system.
* Adding multiplayer collaboration features.
* Developing scenario-based challenges and missions.
* Integrating real-world ecological datasets.
* Improving AI complexity (e.g., behaviour trees).
* Procedural generation for larger, more varied worlds.
* Data export tools for external analysis.

*(See Section 7.1 of the project report for more details)*
