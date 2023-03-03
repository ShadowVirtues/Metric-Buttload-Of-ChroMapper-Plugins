# Metric Buttload Of ChroMapper Plugins
A collection of plugins that modify or introduce mostly small and a few not-so-small things in ChroMapper. Or that's what it was initially, before devs implemented most of these features natively, so a lot of them are now deprecated on latest versions. But if you are using the version 0.8.454 that I initially made these plugins for, they might still be useful for you.

Made these to change the app to how I'd prefer it to be, but also some were highly requested by other mappers, and I particularly was working with Joetastic ([Twitter](https://twitter.com/Joetastic_), [BeatSaver](https://beatsaver.com/profile/58338)) on some of them.

On the screenshot you can see:
GridBookmarks, LongerGrid, ThinBookmarks, SongNameDuringEditing, CloserWaveform, LessTransparentNotes.

![EN8c3JsQk6](https://user-images.githubusercontent.com/33060527/167791164-4ba05bc2-a83b-489b-8de8-84df72ac01fc.jpg)

Also Joetastic made a **side-by-side comparison** for some of the plugins in the [Twitter Thread](https://twitter.com/Joetastic_/status/1524663790733721600).

# Still relevant plugins

## 1. TallMapsAndSettings
Map list window and settings window are stretched to the whole height of your window (less scrolling).

## 2. ShiftFastCamera
Hold Left Shift to fly through the mapping scene 4 times as fast (keybind hard-coded). Don't try quitting the mapping scene with it held tho.

## 3. SeekToSelection (may be broken on latest versions)
Ever Undo-ed something god knows where in the map? Press G to jump right to it (keybind hard-coded). But technically it jumps in time to first selected object.

## 4. ReplaceHitSound (may be broken on latest versions)
Lets you use custom sounds for when cursor passes through notes during playback. 

Your sounds will replace 'Slice' hit-sounds from settings, so select it if you use the plugin. Put the sounds into the folder '\ChroMapper_Data\CustomHitSounds'. Sounds have to be named "Long*.wav", "Short*.wav" (case-insensitive), where there can be anything in place of asterisks. And yes, it only supports .wav files. Random 'long' sound plays for spaced-out notes, random 'short' sound plays for densely-placed notes. If you wish so, there can be only all long or only all short sounds.

## 5. V3SliderConverter
Press Alt+C (keybind hardcoded) to auto-convert all sliders in the map to beatmapV3 chains. What the plugin considers as a slider: an arrow-block followed by a dot-block of the same color when the time difference between these is less or equal to 0.125 beats (1/8). Undo/Redo works after applying. Make sure the map is converted to V3 before applying, since otherwise all sliders are gonna be gone after you quit the map.

## 6. TimelineInputBlocker
Just an input blocker around the timeline, so if you accidentally missclick near it, it won't place a random block.

# Plugins natively implemented on latest versions of ChroMapper

## 1. GridBookmarks (deprecated)
Finally! See how they look like on the screenshot above. 

Also in the same plugin: hovering over bookmarks on timeline shows the beat and time they are at.

## 2. LongerGrid (deprecated)
No more grid cutting off after a few beats. 

This one easily breaks if during map editing you change particular settings or press Ctrl+H, in which case just re-enter the map to bring back the long grid.

For notes to also not get cut off, go to file C:\Users\Admin\AppData\LocalLow\BinaryElement\ChroMapper\ChroMapperSettings.json and edit:  
'ChunkDistance' - for non-playback cut-off distance;  
'Offset_Despawning', 'Offset_Spawning' - for playback cut-off distance.

And the pefromance drop for all this is nowhere near impactful (but obviously depends on your machine and how crazy the map is).

## 3. ThinBookmarks (deprecated)
No more overlapping for densely put bookmarks and no more losing your playhead position behind them.

## 4. SongNameDuringEditing (deprecated)
In the bottom-left corner.

## 5. CloserWaveform (deprecated)
Easier to sync to it when it's right near the notes grid. Works for both 2D and 3D waveform. Rulers had to be sacrificed in the process.

## 6. LessTransparentNotes (deprecated)
So the notes behind the cursor can actually be worked with.

## 7. DisableTransitions (deprecated)
Majorly speeds up switching between scenes.

## 9. DisablePlatformOnStart (deprecated)
Toggles off platform objects on map load (what default L keybind does).

## 10. OverrideColorPicker (deprecated)
Lets you right-click the map color overrides to set the color picker into an existing color that you clicked. (The color picker is the one in Tab menu)

## Installation
Download plugins' .dlls from [Releases](https://github.com/ShadowVirtues/Metric-Buttload-Of-ChroMapper-Plugins/releases/) page. Then put .dlls into 'Plugins' folder located right near 'ChroMapper.exe'. Can put them either to the root of 'Plugins' folder or into any nested folder in there.

## Other Stuff
If someone wants to use any of this code to merge it into ChroMapper by default, feel free to do so.
