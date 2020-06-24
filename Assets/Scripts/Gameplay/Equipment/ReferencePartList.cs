using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Miner.Management.Exceptions;

namespace Miner.Gameplay
{
    [CreateAssetMenu(menuName = "Reference Part List")]
    public class ReferencePartList : ScriptableObject
    {
        [SerializeField] private List<HullReferencePart> _hulls = new List<HullReferencePart>();
        [SerializeField] private List<FuelTankReferencePart> _fuelTanks = new List<FuelTankReferencePart>();
        [SerializeField] private List<EngineReferencePart> _engines = new List<EngineReferencePart>();
        [SerializeField] private List<CargoReferencePart> _cargos = new List<CargoReferencePart>();
        [SerializeField] private List<CoolingReferencePart> _coolings = new List<CoolingReferencePart>();
        [SerializeField] private List<BatteryReferencePart> _batteries = new List<BatteryReferencePart>();
        [SerializeField] private List<DrillReferencePart> _drills = new List<DrillReferencePart>();

        public List<HullReferencePart> Hulls => _hulls;
        public List<FuelTankReferencePart> FuelTanks => _fuelTanks;
        public List<EngineReferencePart> Engines => _engines;
        public List<CargoReferencePart> Cargos => _cargos;
        public List<CoolingReferencePart> Coolings => _coolings;
        public List<BatteryReferencePart> Batteries => _batteries;
        public List<DrillReferencePart> Drills => _drills;

        public ReferencePart GetPart(int id)
        {
            ReferencePart result = Hulls.FirstOrDefault(x => x.Id == id);
            if (result != null)
                return result;
            result = FuelTanks.FirstOrDefault(x => x.Id == id);
            if (result != null)
                return result;
            result = Engines.FirstOrDefault(x => x.Id == id);
            if (result != null)
                return result;
            result = Cargos.FirstOrDefault(x => x.Id == id);
            if (result != null)
                return result;
            result = Coolings.FirstOrDefault(x => x.Id == id);
            if (result != null)
                return result;
            result = Batteries.FirstOrDefault(x => x.Id == id);
            if (result != null)
                return result;
            result = Drills.FirstOrDefault(x => x.Id == id);
            if (result != null)
                return result;
            throw new PartNotFoundException("Part cannot be found (id: " + id.ToString() + ")");
        }
    }
}