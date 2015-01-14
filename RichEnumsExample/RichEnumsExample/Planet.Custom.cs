namespace RichEnumsExample
{
    public partial class Planet
    {
        // universal gravitational constant  (m3 kg-1 s-2)
        public const double G = 6.67300E-11;

        public double SurfaceGravity()
        {
            return G * _mass / (_radius * _radius);
        }

        public double SurfaceWeight(double otherMass)
        {
            return otherMass * SurfaceGravity();
        }
    }
}
