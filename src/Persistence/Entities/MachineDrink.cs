namespace Persistence.Entities
{
    public class MachineDrink
    {
        public long Id { get; set; }

        public int DrinkId { get; set; }
        public virtual Drink Drink { get; set; }

        public int MachineId { get; set; }
        public virtual Machine Machine { get; set; }
    }
}
