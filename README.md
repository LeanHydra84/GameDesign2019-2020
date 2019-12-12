# GameDesign2019-2020
2019-2020 SkillsUSA Game Design Project

## List of Scripts:

_mainScript.cs_

_roomChange.cs_

_charCont.cs_

_roomClass.cs_

_bossFight.cs_

_projectileScript.cs_

## Script Functions

**mainScript.cs** - _Handles most gameplay aspects, excluding functions handled by roomChange.cs and charCont.cs_

**charCont.cs** - _Handles all character controlling functions, including Camera functions and accessing roomClass.cs_

**roomChange.cs** - _Handles random room generation as well as flipping the rooms during runtime_

**roomClass.cs** - _Public class, instances on each room trigger. Holds information about attached cameras, relative movement positions, and room identifiers_

**bossFight.cs** - _Handles reading of the bossfight scripts, as well as instantiation of attacks._

**projectileScript.cs** - _Handles instanitated projectile functions._
