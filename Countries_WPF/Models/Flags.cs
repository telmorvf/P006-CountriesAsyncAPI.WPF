namespace Countries_WPF
{

    /// <summary>
    /// Return the Web path for the Country Flags
    /// </summary>
    public class Flags
    {
        public string png { get; set; }
        public string svg { get; set; }

        public override string ToString()
        {
            return $"{png}";
        }
    }
}
