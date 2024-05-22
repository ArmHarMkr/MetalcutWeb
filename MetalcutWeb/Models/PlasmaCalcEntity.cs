namespace MetalcutWeb.Models;

public class PlasmaCalcEntity
{
    private Dictionary<int, int> priceThicknessDictionary = new Dictionary<int, int>()
    {
        [1] = 450,
        [2] = 600,
        [3] = 800,
        [4] = 900,
        [5] = 1000,
        [6] = 1300,
        [7] = 1400,
        [8] = 1500,
        [9] = 1600,
        [10] = 1800,
        [11] = 2000,
        [12] = 2200,
        [13] = 2300,
        [14] = 2400,
        [15] = 2500,
        [16] = 2500,
        [17] = 2600,
        [18] = 2700,
        [19] = 2800,
        [20] = 3300,
        [21] = 3400,
        [22] = 3500,
        [23] = 3600,
        [24] = 3800,
        [25] = 3900,
        [30] = 5000,
        [35] = 7000
    };

    public int Thickness { get; set; }
    public double Meter { get; set; }
    public double Price()
    {
        return priceThicknessDictionary[Thickness] * Meter;
    }
}
