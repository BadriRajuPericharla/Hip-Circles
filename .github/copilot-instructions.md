# copilot-instructions.md
# Rhythm Circle — Unity Setup Guide
# Engine: Unity 2022.3 LTS | Pipeline: URP | Input: Keyboard (Phase 1)

---

## CONTEXT BLOCK [LOAD THIS FIRST IN EVERY SESSION]

```
Project: RhythmCircle
Engine: Unity 2022.3 LTS
Render: URP 14+
Input: Legacy (Phase 1 keyboard). Phase 2 = MocapInputProvider via IInputProvider.
Namespace root: RhythmCircle
Folder root: Assets/RhythmCircle/
Key pattern: Event-driven (C# events, not UnityEvent). Structs for hot-path data.
No singletons. All refs wired via SerializeField.
Scoring: ScoreEvaluator is static pure function. No MB.
Pool: CircleSpawner manages 16-slot BeatCircle queue. Zero Instantiate in hot path.
Mocap swap: Replace KeyboardInputHandler with MocapInputProvider on PlayerController. Zero other changes.
```

Paste this block at the start of any Copilot/Claude session to restore full context without re-explaining the project.

---

## PHASE 0 — PROJECT SETUP

### Step 1 — Create Unity Project
1. Unity Hub → New Project → **3D (URP)** template → name `RhythmCircle`
2. Unity Editor opens with URP pre-configured

### Step 2 — Package verification
Window → Package Manager → confirm present:
- Universal RP (14+)
- TextMeshPro
- Unity Test Framework

### Step 3 — Create folder structure
In `Assets/`, create this exact tree:
```
RhythmCircle/
  Scripts/
    Core/
    Gameplay/
    Input/
    Scoring/
    Data/
    UI/
    Editor/
  ScriptableObjects/
    Songs/
  Prefabs/
  Materials/
  Scenes/
  Audio/
```

### Step 4 — Import scripts
Copy all `.cs` files into matching subfolders under `Assets/RhythmCircle/Scripts/`.
Unity will compile automatically. **Fix any namespace errors before proceeding.**

---

## PHASE 1 — SCRIPTABLE OBJECTS

### Step 5 — Create SongConfig asset
1. Right-click `Assets/RhythmCircle/ScriptableObjects/Songs/`
2. Create → RhythmCircle → SongConfig → name it `Song_Default`
3. Fields to fill:
   - `Song Name`: "Test Song"
   - `Clip`: drag an AudioClip from Assets/Audio/ (any .wav or .mp3)
   - `BPM`: 120
   - `Offset`: 0
   - `Beats Ahead`: 2
4. Leave `Beat Map` empty for now — Step 11 will populate it

---

## PHASE 2 — SCENE SETUP (GameScene)

### Step 6 — Create GameScene
File → New Scene → Basic (URP) → Save as `Assets/RhythmCircle/Scenes/GameScene.unity`

### Step 7 — Camera setup
Select `Main Camera`:
- Projection: **Orthographic**
- Size: **5**
- Clear Flags: Solid Color
- Background: `#0D0D0D`
- Position: (0, 0, -10)

### Step 8 — Create GameManager GameObject
1. Hierarchy → Create Empty → name `GameManager`
2. Add components (drag scripts):
   - `Bootstrap`
   - `BeatManager`
   - `GameStateManager`
   - `ScoreSystem`
3. Add `AudioSource` component to `GameManager`
   - Un-check Play On Awake

### Step 9 — Create PlayerCircle GameObject
1. Hierarchy → Create Empty → name `PlayerCircle`
2. Position: (0, 0, 0)
3. Add `LineRenderer` component:
   - Positions: leave empty (code fills it)
   - Width: 0.05
   - Material: NeonLine (create in Step 13)
   - Loop: ✓ checked
   - Use World Space: ☐ unchecked
4. Add `PlayerController` script
5. Add `PlayerCircleRenderer` script
6. Create child Empty → name `AngleMarker` → position (0.5, 0, 0)
7. Wire `PlayerCircleRenderer.MarkerTransform` → `AngleMarker`

### Step 10 — Create KeyboardInputHandler GameObject
1. Hierarchy → Create Empty → name `InputHandler`
2. Add `KeyboardInputHandler` script
3. On `PlayerController.InputProviderSource` → drag `InputHandler`

### Step 11 — Create CircleSpawner GameObject
1. Hierarchy → Create Empty → name `CircleSpawner`
2. Add `CircleSpawner` script
3. Wire `BeatManager` ref → `GameManager`
4. Leave `BeatCirclePrefab` blank until Step 14

---

## PHASE 3 — MATERIALS

### Step 12 — Create NeonLine material
1. Assets/RhythmCircle/Materials/ → Create → Material → name `NeonLine`
2. Shader: `Universal Render Pipeline/Particles/Unlit`
   *(or `Sprites/Default` if particles unavailable)*
3. Base Map color: `#00FFB2` (teal)
4. Surface Type: Transparent
5. Assign to `PlayerCircle` LineRenderer

### Step 13 — Create BeatRing material
1. Duplicate `NeonLine` → name `BeatRing`
2. Base Map color: `#FF3C6E` (pink-red)
3. Assign to `BeatCircle` prefab LineRenderer (Step 14)

---

## PHASE 4 — BEAT CIRCLE PREFAB

### Step 14 — Create BeatCircle prefab
1. Hierarchy → Create Empty → name `BeatCircle`
2. Add `LineRenderer`:
   - Width: 0.05
   - Material: BeatRing
   - Loop: ✓
   - Use World Space: ☐
3. Add `BeatCircle` script
4. Drag to `Assets/RhythmCircle/Prefabs/` → creates prefab
5. Delete from scene
6. Wire `CircleSpawner.BeatCirclePrefab` → this prefab

---

## PHASE 5 — BEAT MAP

### Step 15 — Generate beat map via editor tool
1. Menu bar → RhythmCircle → Beat Map Generator
2. Drag `Song_Default` into SongConfig field
3. Set BPM to match your audio clip
4. Set Song Length (check clip length in Inspector)
5. Toggle Alternate Direction: ✓
6. Click **Generate Beat Map**
7. Verify `Song_Default` shows beat count > 0 in Inspector

---

## PHASE 6 — WIRE ALL REFERENCES

### Step 16 — Wire BeatManager
On `GameManager → BeatManager`:
- `Audio Source` → `GameManager` AudioSource component
- `Song Config` → `Song_Default`

### Step 17 — Wire GameStateManager
On `GameManager → GameStateManager`:
- `Beat Manager` → `GameManager`

### Step 18 — Wire ScoreSystem
On `GameManager → ScoreSystem`:
- `Beat Manager` → `GameManager`
- `Player Controller` → `PlayerCircle`
- `Circle Spawner` → `CircleSpawner`
- `Input Provider Source` → `InputHandler`
- `Evaluation Mode` → Discrete

### Step 19 — Wire Bootstrap
On `GameManager → Bootstrap`:
- `Beat Manager` → `GameManager`
- `Game State Manager` → `GameManager`
- `Score System` → `GameManager`
- `Default Song` → `Song_Default`

---

## PHASE 7 — UI SETUP

### Step 20 — Create Canvas
1. Hierarchy → UI → Canvas
2. Canvas Scaler: Scale with Screen Size, 1920×1080, Match 0.5

### Step 21 — Create HUD elements
Under Canvas, create:

| Name | Component | Anchor | Position |
|------|-----------|--------|----------|
| `ScoreText` | TMP_Text, size 36, right-align | top-left | (20, -20) |
| `ComboText` | TMP_Text, size 48, center | top-center | (0, -20) |
| `GradeText` | TMP_Text, size 72, center, bold | center | (0, 60) |
| `CountdownText` | TMP_Text, size 64, center | center | (0, 0) |

All text color: white (`#FFFFFF`).

### Step 22 — Create Panels
Create empty child GameObjects:
- `MenuPanel` → add Start button, wire `OnClick` → `UIManager.OnStartButtonPressed()`
- `HUDPanel` → parent ScoreText, ComboText, GradeText, CountdownText
- `ResultsPanel` → add two TMP_Text: `ResultsScoreText`, `ResultsComboText`, and a Restart button

### Step 23 — Create UIManager GameObject
1. Hierarchy → Create Empty → name `UIManager`
2. Add `UIManager` script
3. Wire all refs:
   - `Game State Manager` → `GameManager`
   - `Score System` → `GameManager`
   - All TMP_Text refs → matching UI objects
   - All panel refs → matching panel GameObjects

---

## PHASE 8 — POST-PROCESSING

### Step 24 — Enable URP Post Processing
1. Select `Main Camera` → check **Post Processing** ✓
2. Hierarchy → Volume → Global Volume → name `PostProcessVolume`
3. Profile → Create new → name `DefaultProfile`

### Step 25 — Add effects
In DefaultProfile, Add Override:
- **Bloom**: ✓ Intensity=2.5, Threshold=0.8, Scatter=0.7
- **Vignette**: ✓ Intensity=0.4
- **Chromatic Aberration**: ✓ Intensity=0.3

---

## PHASE 9 — FIRST PLAY TEST

### Step 26 — Keyboard controls reminder
| Key | Action |
|-----|--------|
| A / ← | Circle rotates CCW |
| D / → | Circle rotates CW |
| Q | Shrink radius |
| E | Expand radius |
| Space | Confirm hit |
| P | Pause |
| Esc | Menu |

### Step 27 — Play test checklist
- [ ] Hit Play — no console errors
- [ ] Click Start button — countdown appears
- [ ] Music plays
- [ ] Beat circles appear on screen
- [ ] A/D rotates player circle
- [ ] Space bar shows PERFECT/GREAT/GOOD/MISS text
- [ ] Score increments
- [ ] Song ends → results panel appears

---

## PHASE 10 — MOCAP SWAP (PHASE 2 — HANDOFF NOTES)

When mocap team is ready:

```
1. Create MocapInputProvider.cs implementing IInputProvider
2. Extract hip joint data → pack into InputState
3. On PlayerController.InputProviderSource → swap to MocapInputProvider
4. On ScoreSystem.EvaluationMode → set Continuous
5. Disable KeyboardInputHandler GameObject
6. Zero other code changes required
```

IInputProvider contract:
```csharp
public interface IInputProvider {
    InputState GetCurrentState();
}

public struct InputState {
    public float rotationDirection; // -1 to +1
    public float radiusDelta;       // -1 to +1
    public bool  confirmHit;        // unused in Continuous mode
}
```

---

## COMMON ERRORS & FIXES

| Error | Cause | Fix |
|-------|-------|-----|
| NullRef on `_input` | InputProviderSource not wired | Drag InputHandler to PlayerController.InputProviderSource |
| Circles never appear | BeatMap empty | Run Beat Map Generator (Step 15) |
| No audio plays | AudioSource.PlayOnAwake off + song not loaded | Ensure Bootstrap.DefaultSong is assigned |
| Score never updates | ScoreSystem refs not wired | Check Step 18 |
| LineRenderer invisible | Material missing | Assign NeonLine/BeatRing materials (Steps 12-13) |
| Compilation error TMP | TextMeshPro not imported | Window → TMP → Import TMP Essentials |

---

## SESSION SHORTCUT PROMPTS

Use these exact prompts to resume efficiently in any AI session:

**"Load context"** → Paste the CONTEXT BLOCK at top of this file

**"Add [feature]"** → Prefix with context block, then describe feature

**"Debug [system]"** → State which system (BeatManager/ScoreSystem/etc) + error message

**"Wire [component]"** → State component name; AI knows the full reference map

**"Mocap swap"** → Refer to Phase 10 section above

---

## SCRIPT DEPENDENCY MAP (quick ref)

```
Bootstrap
  ├── BeatManager       ← SongConfig (SO)
  │     └── OnBeatSpawned ──► CircleSpawner ──► BeatCircle (pool)
  ├── GameStateManager  ← BeatManager
  │     └── OnStateChanged ──► UIManager
  └── ScoreSystem
        ├── BeatManager (time source)
        ├── CircleSpawner (active beats)
        ├── PlayerController
        │     └── IInputProvider ← KeyboardInputHandler [swap: MocapInputProvider]
        └── OnHitEvaluated ──► UIManager

ScoreEvaluator (static) — no MB, no refs, unit-testable
```

---
_Last updated: v1.0 | Engine: Unity 2022.3 LTS | Phase 1: Keyboard_
