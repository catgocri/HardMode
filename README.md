# PotionCraft Hard Mode 
![png](https://cdn.discordapp.com/attachments/895710238060216370/895752054323183646/Untitled_design_23.png)

**Developed as a collaboration project by [catgocri](https://github.com/catgocri) & [hxpmods](https://github.com/hxpmods).**

**This mod uses BasicMod, a Potion Craft modding library.**

### NOT RECOMMENDED FOR USE ON OLD SAVES. MAKE A NEW SAVE FOR THIS MOD.

For those who are seeking a challenge. Hard Mode multiplies Potion Craft's difficulty by tenfold, giving you a nice challenge. Nearly all of the mod's challenge features are either toggleable or configurable, allowing you to customize your challenge! You can find the config file in **Potion Craft\BepInEx\Config.**

If anyone can get a legitimate Philosophers Stone with this mod enabled you earn our respect haha.
## Features
The features this mod adds are:
- Merchants are always selling at higher prices. (Configurable & Toggleable)
- Daily gold tax of 5% after reaching 500 gold. (Configurable & Toggleable)
- Highlander mode, adding too many of one ingredient fails the potion. (Configurable & Toggleable)
- Bone zones are buffed. (Configurable & Toggleable)
- Less ingredients from garden. (Toggleable)
- Potion Deterioration. This causes your potion to lose health when it moves through the map. (Configurable & Toggleable)
- As a follow up to that pouring your base will heal the potion. (Configurable & Toggleable)
- Potion health is directly correlated with potion tier. You will need health for higher tiers, keep yer potions healthy. (Configurable & Toggleable)
- Integrated tutorial skip, so you can get going faster on a new save. (Toggleable)
- A health counter showing the exact amount of potion health your potion has. (Toggleable)
- Custom hard mode goals, completing these goals increases an experience point gain modifier. (Toggleable)
- An infinite haggling talent, allowing you to infinitely level up your haggling talent, if you have enough talent points. (Toggleable)
- No automatic health regain when outside bone zone. (Toggleable)
## Config (Found at BepInEx/Config)
- `damagePotionOnMove` Makes the potion take damage when it moves. (Boolean)
- `damageRate` The rate at which the potion deteriorates. (Float)
- `pouringWaterHeals` Heal the potion by pouring in base. (Boolean)
- `healingRate` The rate to heal by. (Float)
- `doExperienceModifier` If completing hard mode goals should add an experience multiplier. (Boolean)
- `doGardenModifier` If the garden's harvest count should be modified. (Boolean)
- `useConfig` Whether to use the config. Turning this off will cause Hardmode to do nothing unless used by other mods. (Boolean)
- `showWatermark` Shows the HardMode watermark. (Boolean)
- `bonusGoals` Gives bonus goals that also increase your experience modifier. (Boolean)
- `infiniteHaggle` Adds a haggle talent at the end of the tree that can be gained infinitely. (Boolean)
- `boneDamage` The amount of damage you take while moving a full unit touching bones. Vanilla default is 0.4. (Float)
- `healWhenSafe` Potion heals instantly when out of danger. (Boolean)
- `highlanderActive` If active, potions will fail when adding more than the amount detailed below of a single ingredient. (Boolean)
- `maxDuplicateIngredients` What the maximum amount of duplicate ingredients can be before the potion fails. (Integer)
- `modifyPrices` Makes merchants always sell at markup. (Boolean)
- `potionHealthAffectsTier` If potion health affects tier. (Boolean)
- `potionHealthAffectsTierRate` The amount that health contributes to the tier. (Float)
- `skipTutorial` If the integrated tutorial skip should be triggered. (Boolean)
- `doTaxes` If the game should subtract money from you each day. (Boolean)
- `taxPercentage` How much, in percentage, you lose in taxes each day. (Float)
- `taxThreshold` The minimum amount of Gold you must have before being charged taxes. (Integer)
## Known Bugs
- Game shows that you have been taxed 5 gold on startup (even though you haven't).
- Game shows the experience modifier gain notification on startup.
- Potion liquid doesn't start shaking when health is not full.
## Installation
This mod uses BepInEx 5! BepInEx is a mod loader, basically it is what puts the edited code into the game.
- Install [BepInEx 5](https://github.com/BepInEx/BepInEx/releases) and extract the zip file.
- Drag the extracted folder into your Potion Craft folder, you should be able to find the Potion Craft folder at `C:\Program Files (x86)\Steam\steamapps\common\Potion Craft`
- Run Potion Craft once with BepInEx installed.
- Download the mod from the [GitHub](https://github.com/catgocri/HardMode/releases) page.
- Drag `potioncraft-hardmode.dll`, `BasicMod.dll` and the `assets` folder into `Potion Craft\BepInEx\Plugins`
- You are good to go!
## Deinstallation
Uninstalling this mod is as easy as deleting a few files, you don't even need to delete [BepInEx](https://github.com/BepInEx/BepInEx/releases)! **This will break any saves Hard Mode has been used in.**
- Navigate to `Potion Craft\BepInEx\Plugins`
- Remove `potioncraft-hardmode.dll`, `BasicMod.dll` and the `assets` folder from the folder.
- You are done!

c:
