# Metric Buttload Of ChroMapper Plugins
A collection of plugins that modify or introduce mostly small and a few not-so-small things in ChroMapper. 

Made these to change the app to how I'd prefer it to be, but also some were highly requested by other mappers. 

On the screenshot you can see:
GridBookmarks, LongerGrid, ThinBookmarks, SongNameDuringEditing, CloserWaveform, LessTransparentNotes.

![EN8c3JsQk6](https://user-images.githubusercontent.com/33060527/167791164-4ba05bc2-a83b-489b-8de8-84df72ac01fc.jpg)

## 1. GridBookmarks
Finally! See how they look like on the screenshot above. 

Also in the same plugin: hovering over bookmarks on timeline shows the beat and time they are at.

## 2. LongerGrid
No more grid cutting off after a few beats. 

This one easily breaks if during map editing you change particular settings or press Ctrl+H, in which case just re-enter the map to bring back the long grid.

For notes to also not get cut off, go to file C:\Users\Admin\AppData\LocalLow\BinaryElement\ChroMapper\ChroMapperSettings.json and edit:  
'ChunkDistance' - for non-playback cut-off distance;  
'Offset_Despawning', 'Offset_Spawning' - for playback cut-off distance.

And the pefromance drop for all this is nowhere near impactful (but obviously depends on your machine and how crazy the map is).

## 3. ThinBookmarks
No more overlapping for densely put bookmarks and no more losing your playhead position behind them.

## 4. SongNameDuringEditing
In the bottom-left corner.

## 5. CloserWaveform
Easier to sync to it when it's right near the notes grid. Works for both 2D and 3D waveform. Rulers had to be sacrificed in the process.

## 6. LessTransparentNotes
So the notes behind the cursor can actually be worked with.

## 7. DisableTransitions
Majorly speeds up switching between scenes.

## 8. TallMapsAndSettings
Map list window and settings window are stretched to the whole height of your window (less scrolling).

## 9. DisablePlatformOnStart
Toggles off platform objects on map load (what default L keybind does).

## 10. ShiftFastCamera
Hold Left Shift to fly through the mapping scene 4 times as fast (keybind hard-coded). Don't try quitting the mapping scene with it held tho.

## 11. SeekToSelection
Ever Undo-ed something god knows where in the map? Press G to jump right to it (keybind hard-coded). But technically it jumps in time to first selected object.

## 12. ReplaceHitSound
Lets you use custom sounds for when cursor passes through notes during playback. 

Your sounds will replace 'Slice' hit-sounds from settings, so select it if you use the plugin. Put the sounds into the folder '\ChroMapper_Data\CustomHitSounds'. Sounds have to be named "Long*.wav", "Short*.wav" (case-insensitive), where there can be anything in place of asterisks. And yes, it only supports .wav files. Random 'long' sound plays for spaced-out notes, random 'short' sound plays for densely-placed notes. If you wish so, there can be only all long or only all short sounds.

## Installation
Download plugins' .dlls from [Releases](https://github.com/ShadowVirtues/Metric-Buttload-Of-ChroMapper-Plugins/releases/) page. Then put .dlls into 'Plugins' folder right near ChroMapper.exe. Can put them either to the root of 'Plugins' folder or into any nested folder in there.

## Other Stuff
If someone wants to use any of this code to merge it into ChroMapper by default, feel free to do so.
