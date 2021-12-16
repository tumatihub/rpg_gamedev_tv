using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;
        public float ExperiencePoints => experiencePoints;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            Debug.Log(GetComponent<BaseStats>().GetLevel());
        }

        public object CaptureState()
        {
            return experiencePoints;
        }

        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
