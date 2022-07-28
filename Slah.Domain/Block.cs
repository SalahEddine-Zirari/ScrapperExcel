namespace Slah.Domain;

public class Block
{
    public DateTime Date { get; set; }
    public IEnumerable<PeriodicPrice> Periods { get; set; }
}






