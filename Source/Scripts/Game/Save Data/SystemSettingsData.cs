using System.Xml;
using System.Xml.Serialization;

namespace Cogwheel.Save
{
    [System.Serializable]
    public class SystemSettingsData
    {
        public bool Fullscreen;
        public int[] Resolution;
        public int VsyncCount;
        public int GraphicsPreset;
        public float MasterVolume;
        public float EffectsVolume;
    }
}
