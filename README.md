# GameDesign2019-2020
2019-2020 SkillsUSA Game Design Project

Scripts for the game design skills project

# List of Scripts:
mainScript.cs

roomChange.cs

charCont.cs

roomClass.cs

# Script Functions

mainScript.cs - Handles most gameplay aspects, excluding functions handled by roomChange.cs and charCont.cs

charCont.cs - Handles all character controlling functions, including Camera functions and accessing roomClass.cs

roomChange.cs - Handles random room generation as well as flipping the rooms during runtime

roomClass.cs - Public class, instances on each room trigger. Holds information about attached cameras, relative movement positions, and room identifiers