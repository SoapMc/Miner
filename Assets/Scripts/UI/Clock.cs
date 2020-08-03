using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Miner.UI
{
    public class Clock : MonoBehaviour
    {
        [SerializeField] private IntReference _timeOfDay = null;
        [SerializeField] private IntReference _elapsedDays = null;
        [SerializeField] private TextMeshProUGUI _hourText = null;
        [SerializeField] private TextMeshProUGUI _dayText = null;

        private void MinuteElapsed(int oldVal, int timeOfDay)
        {
            int hours = timeOfDay / 60;
            int seconds = timeOfDay % 60;
            _hourText.text = MakeTimeFormat(hours) + ":" + MakeTimeFormat(seconds);
        }

        private void DayElapsed(int _, int elapsedDays)
        {
            _dayText.text = "Sol " + (elapsedDays + 1).ToString();
        }

        private string MakeTimeFormat(int time)
        {
            if (time < 10)
                return "0" + time.ToString();
            else
                return time.ToString();
        }

        private void Start()
        {
            _timeOfDay.ValueChanged += MinuteElapsed;
            _elapsedDays.ValueChanged += DayElapsed;
        }

        private void OnDestroy()
        {
            _timeOfDay.ValueChanged -= MinuteElapsed;
            _elapsedDays.ValueChanged -= DayElapsed;
        }
    }
}