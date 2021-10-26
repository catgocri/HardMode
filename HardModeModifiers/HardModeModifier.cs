using BepInEx.Configuration;

namespace catgocrihxpmods.HardMode.PotionCraft
{
    public abstract class HardModeModifier
    {
        public string name;
        public static HardModeModifier instance;
        public bool active = false;

        protected HardModeModifier()
        {
            instance = this;
        }

        public void SetActive(bool a)
        {
            active = a;
        }

        public abstract void LoadFromBindings(ConfigFile config);
    }
}
