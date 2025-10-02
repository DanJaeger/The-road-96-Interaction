# The Road 96 Interaction (Unity / C#)

A Unity prototype replicating the NPC interaction system inspired by *Road 96*.  
Players can approach NPCs, choose dialogue options, and see responses that depend on global variables, with synchronized animations and audio cues.  
Controls: move with WASD, aim with the mouse, and interact with left-click. 🎮🗨️

---

# Project Features

- 🎮 **Controls** → WASD to move, Mouse to aim, Left-Click to select dialogue options  
- 🧩 **NPC Interaction System** → Dialogue choices influence story variables  
- 🔄 **Variable-dependent Dialogue** → NPC responses adapt based on previous interactions  
- 🗣️ **Voice & Audio** → NPC lines accompanied by audio clips  
- 🕺 **Animations** → Smooth transitions between idle and talking states via Blend Trees  
- 🧱 **Collision system** → Player can approach NPCs and interact when in range  
- 🖥️ **UI** → Subtitles and choice display using TextMeshPro  
- 📑 **Code Structure** → DialogueManager, NPCStateManager, DialogueVariables  
- 🧪 **Ink Integration** → Ink story files to manage branching dialogue logic  

---

## Gameplay  
![Gameplay1](GitVisuals/Road_96_NPC_Interaction.gif)  
![Gameplay2](GitVisuals/interaction1.png)  
![Gameplay3](GitVisuals/interaction2.png)  

## Ink Implementation  
![Ink1](GitVisuals/ink1.PNG)  
![Ink2](GitVisuals/ink2.PNG)  
---

## How to run the project (Windows)    
- git lfs install
- git clone https://github.com/DanJaeger/The-road-96-Interaction.git
- Open the Unity project in Unity 2020.x or newer (preferably 2020.3.14f1) 
- Open the scene called "Scene_Ink" if it not already open
- Press Play in the Editor to start the prototype  
--- 

## License
This project is licensed under the MIT License – see the [LICENSE](LICENSE) file for details.
