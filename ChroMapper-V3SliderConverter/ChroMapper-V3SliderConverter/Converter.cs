using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beatmap.Base;
using Beatmap.Enums;
using Beatmap.V3;
using UnityEngine;

namespace ChroMapper_V3SliderConverter
{
    public class Converter : MonoBehaviour
    {
        private NoteGridContainer NotesContainer;

        //Since you can't pass null to BeatmapObjectPlacementAction, because clearly you'd ALWAYS need to pass conflicting notes...
        private readonly BaseObject[] dummyArray = Array.Empty<BaseObject>();

        void Start()
        {
            NotesContainer = UnityEngine.Object.FindObjectOfType<NoteGridContainer>();
        }

        void Update()
        {
            if (!Input.GetKeyDown(KeyCode.C) || !Input.GetKey(KeyCode.LeftAlt)) return;
            
            PersistentUI.Instance.ShowDialogBox("Convert all sliders to chains?", Convert, PersistentUI.DialogBoxPresetType.YesNo);
        }

        private void Convert(int buttonIndex)
        {
            if (buttonIndex != 0) return;   //PersistentUI.Instance.ShowDialogBox action takes int parameter of which button got pressed
            
            List<string> proccedBeatNumbers = new List<string>();   //To log in the end

            List<BeatmapAction> allActions = new List<BeatmapAction>();
            
            BaseNote prevBlueArrow = NotesContainer.LoadedObjects.FirstOrDefault(x => x is BaseNote blue && blue.Type == 1) as BaseNote;
            BaseNote prevRedArrow = NotesContainer.LoadedObjects.FirstOrDefault(x => x is BaseNote red && red.Type == 0) as BaseNote;

            BeatmapObjectContainerCollection chainCollection = BeatmapObjectContainerCollection.GetCollectionForType(ObjectType.Chain);
            BeatmapObjectContainerCollection noteCollection = BeatmapObjectContainerCollection.GetCollectionForType(ObjectType.Note);

            //Iterating through all objects.ToArray, since we modify the collection.
            //Then we remember each arrow note as 'previous' and then when stumbling upon dot note, we check if previous arrow note is close in time and that's how we detect stacks
            foreach (BaseObject obj in NotesContainer.LoadedObjects.ToArray())
            {
                BaseNote note = obj as BaseNote;  //Iterating through all objects and taking only notes
                if (note == null) continue;

                BaseNote prevRefNote = note.Type == 0 ? prevRedArrow : prevBlueArrow;

                if (prevRefNote != null && note.Time - prevRefNote.Time < 0.126f && prevRefNote.CutDirection != 8 && note.CutDirection == 8)
                {
                    BaseNote dot = note;

                    V3Chain chain = new V3Chain
                    {
                        Time = prevRefNote.Time,
                        Color = dot.Type,
                        PosX = prevRefNote.PosX,
                        PosY = prevRefNote.PosY,
                        CutDirection = prevRefNote.CutDirection,
                        TailTime = dot.Time,
                        TailPosX = dot.PosX,
                        TailPosY = dot.PosY,
                        Squish = 1
                    };

                    //To calculate the amount of chains we take the longest distance between arrow block an dot block on either X or Y axis
                    int dist = Mathf.Max(Math.Abs(prevRefNote.PosX - dot.PosX), Math.Abs(prevRefNote.PosY - dot.PosY));
                    chain.SliceCount = 2 * (dist + 1);

                    allActions.Add(new BeatmapObjectDeletionAction(dot, "Remove dot in a slider"));
                    noteCollection.DeleteObject(dot, false, false);

                    chainCollection.SpawnObject(chain, false, false);
                    allActions.Add(new BeatmapObjectPlacementAction(chain, dummyArray, "Add chain in place of dot"));

                    proccedBeatNumbers.Add(prevRefNote.Time.ToString());
                }

                if (note.CutDirection != 8 && note.Type == 1) prevBlueArrow = note;
                if (note.CutDirection != 8 && note.Type == 0) prevRedArrow = note;
            }

            noteCollection.RefreshPool();
            chainCollection.RefreshPool();   //Since we further don't 'perform' in BeatmapActionContainer.AddAction, need to refresh
            ActionCollectionAction actionCollection = new ActionCollectionAction(allActions, true, true, "Convert sliders dot to chain");
            BeatmapActionContainer.AddAction(actionCollection);

            print($"Replaced sliders with chains at beats: {string.Join(",", proccedBeatNumbers)}");
        }
    }
}
