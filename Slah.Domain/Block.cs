namespace Slah.Domain;

public class Block
{
    public DateTime Date { get; set; }
    public IEnumerable<PeriodicPrice> Periods { get; set; }
}



public class Period
{
    public DateTime Start { get; set; }
    public DateTime End  { get; set; }
}

public class PeriodicPrice
{
    public Period Period { get; set; }
    public decimal? Price  { get; set; }
    public string Name  { get; set; } = null!;
}

public class PeriodicPriceAndVolume : PeriodicPrice
{
    public decimal? Volume  { get; set; }
}