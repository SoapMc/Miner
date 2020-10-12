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
        [SerializeField] private List<ReferencePart> _hulls = new List<ReferencePart>();
        [SerializeField] private List<ReferencePart> _fuelTanks = new List<ReferencePart>();
        [SerializeField] private List<ReferencePart> _engines = new List<ReferencePart>();
        [SerializeField] private List<ReferencePart> _cargos = new List<ReferencePart>();
        [SerializeField] private List<ReferencePart> _coolings = new List<ReferencePart> ();
        [SerializeField] private List<ReferencePart> _batteries = new List<ReferencePart>();
        [SerializeField] private List<ReferencePart> _drills = new List<ReferencePart>();

        public ReferencePart GetReferencePart(int id)
        {
            ReferencePart result = _hulls.FirstOrDefault(x => x.Id == id);
            if (result != null)
                return result;
            result = _fuelTanks.FirstOrDefault(x => x.Id == id);
            if (result != null)
                return result;
            result = _engines.FirstOrDefault(x => x.Id == id);
            if (result != null)
                return result;
            result = _cargos.FirstOrDefault(x => x.Id == id);
            if (result != null)
                return result;
            result = _coolings.FirstOrDefault(x => x.Id == id);
            if (result != null)
                return result;
            result = _batteries.FirstOrDefault(x => x.Id == id);
            if (result != null)
                return result;
            result = _drills.FirstOrDefault(x => x.Id == id);
            if (result != null)
                return result;
            throw new PartNotFoundException("Part cannot be found (id: " + id.ToString() + ")");
        }

        public List<ReferencePart> GetPartsOfType(EPartType type)
        {
            switch(type)
            {
                case EPartType.Battery:
                    return _batteries;
                case EPartType.Cargo:
                    return _cargos;
                case EPartType.Cooling:
                    return _coolings;
                case EPartType.Drill:
                    return _drills;
                case EPartType.Engine:
                    return _engines;
                case EPartType.FuelTank:
                    return _fuelTanks;
                case EPartType.Hull:
                    return _hulls;
                default:
                    throw new ArgumentException("Requested part type is not present in ReferencePartList");
            }
        }

    }
}