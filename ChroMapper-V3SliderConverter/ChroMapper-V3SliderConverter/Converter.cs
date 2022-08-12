using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChroMapper_V3SliderConverter
{
    public class Converter : MonoBehaviour
    {
        private NotesContainer NotesContainer;

        //Since you can't pass null to BeatmapObjectPlacementAction, because clearly you'd ALWAYS need to pass conflicting notes...
        private readonly BeatmapObject[] dummyArray = Array.Empty<BeatmapObject>();

        void Start()
        {
            NotesContainer = UnityEngine.Object.FindObjectOfType<NotesContainer>();
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
            
            BeatmapNote prevBlueArrow = NotesContainer.LoadedObjects.FirstOrDefault(x => x is BeatmapNote blue && blue.Type == 1) as BeatmapNote;
            BeatmapNote prevRedArrow = NotesContainer.LoadedObjects.FirstOrDefault(x => x is BeatmapNote red && red.Type == 0) as BeatmapNote;

            BeatmapObjectContainerCollection chainCollection = BeatmapObjectContainerCollection.GetCollectionForType(BeatmapObject.ObjectType.Chain);
            BeatmapObjectContainerCollection noteCollection = BeatmapObjectContainerCollection.GetCollectionForType(BeatmapObject.ObjectType.Note);

            //Iterating through all objects.ToArray, since we modify the collection.
            //Then we remember each arrow note as 'previous' and then when stumbling upon dot note, we check if previous arrow note is close in time and that's how we detect stacks
            foreach (BeatmapObject obj in NotesContainer.LoadedObjects.ToArray())
            {
                BeatmapNote note = obj as BeatmapNote;  //Iterating through all objects and taking only notes
                if (note == null) continue;

                BeatmapNote prevRefNote = note.Type == 0 ? prevRedArrow : prevBlueArrow;

                if (prevRefNote != null && note.Time - prevRefNote.Time < 0.126f && prevRefNote.CutDirection != 8 && note.CutDirection == 8)
                {
                    BeatmapNote dot = note;

                    BeatmapChain chain = new BeatmapChain
                    {
                        Time = prevRefNote.Time,
                        Color = dot.Type,
                        X = prevRefNote.LineIndex,
                        Y = prevRefNote.LineLayer,
                        Direction = prevRefNote.CutDirection,
                        TailTime = dot.Time,
                        TailX = dot.LineIndex,
                        TailY = dot.LineLayer
                    };

                    //To calculate the amount of chains we take the longest distance between arrow block an dot block on either X or Y axis
                    int dist = Mathf.Max(Math.Abs(prevRefNote.LineIndex - dot.LineIndex), Math.Abs(prevRefNote.LineLayer - dot.LineLayer));
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
