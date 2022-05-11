using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChroMapper_SongNameDuringEditing
{
    [Plugin("SongNameDuringEditing")]
    public class SongNameDuringEditing
    {
        [Init]
        private void Init()
        {
            SceneManager.sceneLoaded += SceneLoaded;
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == 3)  //Only in the scene where you actually edit the map
            {
                SongTimelineController controller = UnityEngine.Object.FindObjectOfType<SongTimelineController>();
                TextMeshProUGUI songTimeText = controller.transform.Find("Song Time").GetComponent<TextMeshProUGUI>();

                TextMeshProUGUI songNameText = UnityEngine.Object.Instantiate(songTimeText, songTimeText.transform.parent);
                songNameText.fontSize = 10;
                songNameText.rectTransform.anchoredPosition = new Vector2(121.2f, 0.72f);
                songNameText.rectTransform.sizeDelta = new Vector2(224, 12);
                songNameText.alignment = TextAlignmentOptions.BottomLeft;

                BeatSaberSong.DifficultyBeatmap data = BeatSaberSongContainer.Instance.DifficultyData;
                string diffStr = data.CustomData != null && data.CustomData.HasKey("_difficultyLabel")
                    ? data.CustomData["_difficultyLabel"].Value
                    : data.Difficulty;
                BeatSaberSong song = BeatSaberSongContainer.Instance.Song;
                songNameText.text = $"{song.SongAuthorName} - {song.SongName} {song.SongSubName} [{diffStr}]";
            }
        }

        [Exit]
        private void Exit()
        {

        }
    }
}
