
namespace helpers
{
    public class Range
    {
        public double Min{get; set;} = 0;
        public double Max{get; set;} = 0;

        public Range(){}
        public Range(double min, double max)
        {
            this.Min = min;
            this.Max = max;
        }
        public Range (Range r)
        {
            this.Min = r.Min;
            this.Max = r.Max;
        }
    
        

        public override string ToString()
        {
            return Min.ToString("F2")+" : "+Max.ToString("F2");
        }
    }
}