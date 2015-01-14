﻿// <autogenerated>
// This code was generated by a tool. Any changes made manually will be lost
// the next time this code is regenerated.
// </autogenerated>

namespace RichEnumsExample
{
    /// <summary>
    /// Planet
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute( "RichEnumGenerator", "1.0.0.0" )]
    public sealed partial class Planet
    {
        private readonly string _instanceName;
        private readonly System.Double _mass;
        private readonly System.Double _radius;

        /// <summary>
        /// Mercury
        /// </summary>
        public static readonly Planet Mercury = new Planet(
            instanceName: "Mercury",
            mass: 3.30E+23,   
            radius: 2.44E+06  
        );
        /// <summary>
        /// Venus
        /// </summary>
        public static readonly Planet Venus = new Planet(
            instanceName: "Venus",
            mass: 4.87E+24,   
            radius: 6.05E+06  
        );
        /// <summary>
        /// Earth
        /// </summary>
        public static readonly Planet Earth = new Planet(
            instanceName: "Earth",
            mass: 5.98E+24,   
            radius: 6.38E+06  
        );
        /// <summary>
        /// Mars
        /// </summary>
        public static readonly Planet Mars = new Planet(
            instanceName: "Mars",
            mass: 6.42E+23,   
            radius: 3.40E+06  
        );
        /// <summary>
        /// Jupiter
        /// </summary>
        public static readonly Planet Jupiter = new Planet(
            instanceName: "Jupiter",
            mass: 1.90E+27,   
            radius: 7.15E+07  
        );
        /// <summary>
        /// Saturn
        /// </summary>
        public static readonly Planet Saturn = new Planet(
            instanceName: "Saturn",
            mass: 5.69E+26,   
            radius: 6.03E+07  
        );
        /// <summary>
        /// Uranus
        /// </summary>
        public static readonly Planet Uranus = new Planet(
            instanceName: "Uranus",
            mass: 8.69E+25,   
            radius: 2.56E+07  
        );
        /// <summary>
        /// Neptune
        /// </summary>
        public static readonly Planet Neptune = new Planet(
            instanceName: "Neptune",
            mass: 1.02E+26,   
            radius: 2.47E+07  
        );

        private Planet(
            string instanceName,
            System.Double mass,   
            System.Double radius  
        )
        {
            _instanceName = instanceName;
            _mass = mass; 
            _radius = radius; 
        }

        /// <summary>
        /// Returns the name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return _instanceName;
        }

        /// <summary>
        /// Returns all the enumeration values.
        /// </summary>
        public static System.Collections.Generic.IEnumerable<Planet> GetAll()
        {
            yield return Mercury;
            yield return Venus;
            yield return Earth;
            yield return Mars;
            yield return Jupiter;
            yield return Saturn;
            yield return Uranus;
            yield return Neptune;
        }
    }
}
