# The Last Lantern

![Unity](https://img.shields.io/badge/Made_with-Unity_6-000000?logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/Language-C%23-purple?logo=csharp&logoColor=white)
![Shaders](https://img.shields.io/badge/Graphics-ShaderLab_%7C_HLSL-green)
![Licence](https://img.shields.io/badge/Licence-MIT-blue)

An atmospheric 2D Metroidvania built in Unity 6 where light is your only guide through a sprawling, interconnected dark world. Take control of Yato, the final lantern bearer, and navigate an oppressive environment where illumination defines both safety and survival.

[![Download Latest Release](https://img.shields.io/github/v/release/Furkan5E/TheLastLantern?style=for-the-badge&label=DOWNLOAD&color=success&logo=github)](https://github.com/Furkan5E/TheLastLantern/releases/latest)


## Technical Architecture

* **Engine & Graphics:** Unity 6 LTS (6000.3.8f1) running the Universal Render Pipeline (URP) with 2D dynamic lighting.
* **Finite State Machine (FSM):** Zero allocation runtime design. All player and enemy states are instantiated and cached during initialisation (`Awake()`) to prevent Garbage Collection spikes during gameplay.
* **Physics & Raycasting:** Frame rate independent physics updates processed in `FixedUpdate()`. Line of sight enemy awareness is managed via directional raycasts.
* **Event Architecture:** Strict loose coupling using C# `Action` delegates (such as `OnHealthChanged`) and generic interfaces (`IDamageable`, `ICounterable`) adhering to SOLID principles.

## Controls

The game supports gamepads as well as mouse and keyboard setups through the Unity Input System.

| **Action** | **Keyboard & Mouse** | **Gamepad** |
|:---|:---|:---|
| Move | `A` / `D` · `←` / `→` | Left Stick · D-Pad |
| Jump / Wall Jump | `Space` · `W` · `↑` | `A` / `Cross` |
| Melee Attack | `Z` · Left Click | `X` / `Square` |
| Dash | `Shift` · `C` | `B` / `Circle` |
| Interact | `E` | `Y` / `Triangle` |

## Local Development

### Prerequisites
* Unity Hub
* Unity **6000.3.8f1 LTS**

### Installation

1. Clone the repository:
```bash
git clone https://github.com/Furkan5E/TheLastLantern.git
```
2. Open Unity Hub and click Add > Add project from disk.
3. Launch the project using Unity Editor version 6000.3.8f1.
