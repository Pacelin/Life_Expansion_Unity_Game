using System.Collections;
using UnityEngine;

namespace UniOwl
{
    public class WallpaperBlender : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private WallpaperBlenderSettings[] settings;

        [SerializeField] private Wallpaper active;

        private void Start()
        {
            active.Blend(1f);
            foreach (var s in settings)
            {
                if (s.wallpaper == active)
                    continue;
                s.wallpaper.Blend(0f);
            }
        }

        public void Blend(int id)
        {
            if (settings[id].wallpaper == active)
                return;
            StartCoroutine(Blend_(settings[id]));
        }

        private IEnumerator Blend_(WallpaperBlenderSettings setting)
        {
            float counter = 0f;

            while (counter < setting.blendTime)
            {
                float t = counter / setting.blendTime;

                setting.wallpaper.Blend(t);
                active.Blend(1f - t);

                counter += Time.deltaTime;
                yield return null;
            }

            active = setting.wallpaper;
        }
    }
}
