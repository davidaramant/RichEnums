using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using RichEnumsExample.TestableCode;

namespace Tests
{
    [TestFixture]
    public sealed class CsvFileTests
    {
        [Test]
        public void ShouldParseCsvFile()
        {
            var parsedFile = CsvFile.ParseFile( GetAnimalFileContents() );

            Assert.That( parsedFile, Is.Not.Null, "Should have returned a parsed file." );

            Assert.That( parsedFile.Name, Is.EqualTo( "Animal" ), "Did not parse name." );
            Assert.That( parsedFile.Description, Is.EqualTo( "A collection of animals." ),
                "Did not parse description." );
            Assert.That( parsedFile.GetEntries().Count(), Is.EqualTo( 1 ), "Did not parse correct number of entries." );
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
