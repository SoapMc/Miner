using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Equipment/Hull Reference Part")]
    public class HullReferencePart : ReferencePart
    {
        [SerializeField] private int _maxHull = 0;
        [SerializeField] private int _resistanceToHit = 0;
        [SerializeField, Range(0, 50)] private int _thermalInsulation = 0;
        [Tooltip("Damaged permanently if hull (in %) is below")]
        [SerializeField, Range(0f, 1f)] private float _permaDamageThreshold = 0.5f;
        [SerializeField] private int _radiationTolerance = 0;

        public int MaxHull => _maxHull;
        public int ResistanceToHit => _resistanceToHit;
        public int ThermalInsulation => _thermalInsulation;
        public float PermaDamageThreshold => _permaDamageThreshold;
        public int RadiationTolerance => _radiationTolerance;

        public override string GetOfferDescription()
        {
            return "Max hull: " + _maxHull.ToString() +
                   "\nThermal insulation: " + _thermalInsulation.ToString() +
                   "\nResistance to hit: " + _resistanceToHit.ToString() +
                   "\nRadiation tolerance: " + _radiationTolerance.ToString();
        }

        public override string GetPerformanceDescription(float durability)
        {
            return "Total performance: " + ((int)(durability * 100)).ToString() + " %" +
                   "\nMax hull: " + _maxHull.ToString() + " (" + CalculateMaxHull(durability).ToString() + ")" +
                   "\nResistance to hit: " + _resistanceToHit.ToString() + " (" + CalculateResistanceToHit(durability).ToString() + ")" +
                   "\nThermal insulation: " + _thermalInsulation.ToString() +
                   "\nDamaged permenently when hit above " + ((int)(_maxHull * _permaDamageThreshold)).ToString() + " damage" +
                   "\nRadiation tolerance: " + _radiationTolerance.ToString() + base.GetPerformanceDescription(durability);
        }

        public override void Equip(IEquipmentOwner playerStats, float durability)
        {
            playerStats.MaxHull += (int)(MaxHull * durability);
            if (durability == 1f)   //brand new
                playerStats.Hull = playerStats.MaxHull;
            playerStats.ResistanceToHit += (int)(ResistanceToHit * durability);
            playerStats.ThermalInsulation += ThermalInsulation;
            playerStats.HullPermaDamageThreshold += _permaDamageThreshold;
            playerStats.RadiationTolerance += _radiationTolerance;
        }

        public override void Unequip(IEquipmentOwner playerStats, float durability)
        {
            playerStats.MaxHull -= (int)(MaxHull * durability);
            playerStats.ResistanceToHit -= (int)(ResistanceToHit * durability);
            playerStats.ThermalInsulation -= ThermalInsulation;
            playerStats.HullPermaDamageThreshold -= _permaDamageThreshold;
            playerStats.RadiationTolerance -= _radiationTolerance;
        }

        private int CalculateMaxHull(float durability)
        {
            return Mathf.CeilToInt(_maxHull * durability);
        }

        private int CalculateResistanceToHit(float durability)
        {
            return (int)(_resistanceToHit * durability);
        }
    }
}