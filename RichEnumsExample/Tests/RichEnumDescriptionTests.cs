using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RichEnumsExample.TestableCode;

namespace Tests
{
    [TestFixture]
    public sealed class RichEnumDescriptionTests
    {
        [Test]
        public void ShouldParseEnumNameAndDescription()
        {
            var parsedFile = RichEnumDescription.ParseFile( GetAnimalFileContents() );

            Assert.That( parsedFile, Is.Not.Null, "Should have returned a parsed file." );

            Assert.That( parsedFile.Name, Is.EqualTo( "Animal" ), "Did not parse name." );
            Assert.That( parsedFile.Description, Is.EqualTo( "A collection of animals." ),
                "Did not parse description." );
        }

        private static IEnumerable<string> GetAnimalFileContents()
        {
            yield return MakeCsvRow( "Name", "Description", "Size", "Number" );
            yield return MakeCsvRow( "Animal", "A collection of animals.", "System.String", "int" );
            yield return MakeCsvRow( "Dog", "\"A canine.\"", "\"Medium\"", "1" );
        }

        private static string MakeCsvRow( params string[] columns )
        {
            return String.Join( ",", columns.Select( _ => String.Format( "\"{0}\"", _.Replace( "\"", "\"\"" ) ) ) );
        }
    }
}
