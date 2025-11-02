# Not the Right Way (Ludum Dare 55)

> Follow the path through power and deceit in "Not the Right Way”, a tactical RPG/tower defense set in a stark, cubist world. As the border officer of Bodenwitz, command authority and conjure defenses to hold back relentless Kharsarite refugees. Amid chilling spectacles, moral dilemmas, and a web of manipulation, remember: the right path may not be what it seems.

![Cover](Assets/MainTitle/Cover%20Image.png)

## Play it

- Windows (local): run `WinGame/Summoning.exe`
- Web (local): open `Game/index.html` in a browser that supports local WebGL file access
- Editor: open the project in Unity and press Play (see “Run in Unity” below)

> Built with Unity 2022.3.21f1 (LTS).

## Highlights (as featured on my resume)

- Developed a 2D tower defense game in 72 hours for Ludum Dare 55; reached ~1,000 players and ranked in the top 30%
- Implemented enemy object-avoidance pathing so enemies detour around obstacles while heading to targets (shortest-path/Dijkstra-style design)
- Applied a strategy-style pattern for pluggable obstacle behaviors (walls, barbed wire, spike strips, checkpoints, hammer), improving code flexibility
- Built a lightweight localization system (English/Chinese) with dynamic font switching using TextMesh Pro

## Core features

- Tactical summoning: place different obstacles under cooldown to influence enemy paths and outcomes
- Moral choices embedded in progression, with story sequences orchestrated via Fungus flowcharts
- Progressive mechanics: new tools unlock across chapters (e.g., barbed wire, spike strip, hammer)
- Lightweight, performant 2D presentation and bespoke NPC avoidance logic (no heavy NavMesh)

## Controls

During battle (scene `Assets/Scenes/Battle.unity`):

- 1 = summon Checkpoint trap
- 2 = summon Wall
- 3 = summon Barbed Wire (unlocks from Chapter 2)
- 4 = summon Spike Strip (unlocks from Chapter 3)
- 5 = summon Hammer to break the nearest wall (unlocks from Chapter 4)
- Left Click = place selected object
- Right Click = cancel placement

Flow and progression:

- Any key = start the level when instructions are shown
- R = retry (when the failure notice is shown)
- S = skip (from failure notice)
- C = continue (after winning)

Notes:

- All summoned objects have cooldowns; UI icons will gray out while recharging
- Some tools become available only in later chapters (see progression above)

## Localization

- Toggle between English and Chinese via the in-game language toggle
- Text and fonts update dynamically (TextMesh Pro), ensuring correct glyph coverage

## Run in Unity (editor)

1. Install Unity 2022.3.21f1
2. Open this folder as a Unity project
3. Open `Assets/Scenes/SampleScene.unity` (narrative intro) and then `Assets/Scenes/Battle.unity` (gameplay)
4. Press Play

Packages and assets used in this repo:

- Fungus (narrative flow and UI helpers)
- TextMesh Pro (typography, dynamic font switching)
- Rive samples and various prototype assets are present but not required to play

## Builds

- Windows build is committed under `WinGame/` (portable)
- WebGL build is committed under `Game/` (open `index.html` locally or host on a static server)

## Project layout (selected)

- `Assets/Narrative/` — story flow, level controller, chapter transitions (Fungus)
- `Assets/Objects/` — obstacle prefabs and scripts (e.g., `TrapPlacer`, `Block`, `BarberWire`, `Hammer`)
- `Assets/NPC/` — NPC movement and avoidance (`NPCPathfinder`, `ConstantsList`)
- `Assets/Scenes/` — `SampleScene.unity` (intro), `Battle.unity` (gameplay)
- `Assets/Tools/` — simple localization (`LocalizationManager`, `TextSelfCheck`, `SwitchLanguage`)

## Technical notes

- NPC avoidance: when encountering obstacles, NPCs compute a vertical detour relative to bounds and lane thresholds, then resume horizontal motion; slowdown and stop states are applied via colliders and animation speed.
- Obstacles: each tool encapsulates distinct effects (blocking, slowing, checkpointing, destruction). The summoning flow centralizes selection/cooldown logic (`TrapPlacer`, `Hammer`).
- Timing & pacing: global map speed and event broadcasting are coordinated via `ConstantsList`.

## Credits & acknowledgements

- Created for Ludum Dare 55
- Built with Unity + C#
- Thanks to the Fungus community for the excellent visual storytelling toolkit
