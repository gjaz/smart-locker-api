using SmartLocker.Api.Models;

namespace SmartLocker.Api.Services;

public class LockerService : ILockerService
{
    private List<Locker> lockers;

    public List<Locker> Lockers
    {
        get
        {
            if (lockers == null)
            {
                lockers = new List<Locker>
                {
                    new Locker
                    {
                        Id = 1,
                        Codigo = "L-001",
                        Ubicacion = "Planta 1",
                        Estado = "Disponible",
                        Tamano = "Mediano"
                    },
                    new Locker
                    {
                        Id = 2,
                        Codigo = "L-002",
                        Ubicacion = "Planta 1",
                        Estado = "Ocupado",
                        Tamano = "Grande"
                    },
                    new Locker
                    {
                        Id = 3,
                        Codigo = "L-003",
                        Ubicacion = "Planta 2",
                        Estado = "Disponible",
                        Tamano = "Pequeño"
                    }
                };
            }

            return lockers;
        }
        set
        {
            lockers = value;
        }
    }

    public IEnumerable<Locker> GetAll()
    {
        return Lockers;
    }

    public Locker Add(Locker locker)
    {
        locker.Id = Lockers.Count == 0
            ? 1
            : Lockers.Max(x => x.Id) + 1;

        Lockers.Add(locker);

        return locker;
    }

    public bool Delete(int id)
    {
        var locker = Lockers.FirstOrDefault(x => x.Id == id);

        if (locker == null)
        {
            return false;
        }

        Lockers.Remove(locker);

        return true;
    }
}