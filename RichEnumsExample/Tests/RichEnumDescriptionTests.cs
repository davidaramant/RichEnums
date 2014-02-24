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
        public void ShouldParseDescriptionWithoutFields()
        {
            var parsedFile = RichEnumDescription.ParseFile( GetDescriptionWithoutFields() );

            Assert.That( parsedFile, Is.Not.Null, "Should have returned a parsed file." );

            Assert.That( parsedFile.Name, Is.EqualTo( "Animal" ), "Did not parse name." );
            Assert.That( parsedFile.Description, Is.EqualTo( "A collection of animals." ),
                "Did not parse description." );
            Assert.That( parsedFile.EntryFields.Any(), Is.False, "Should not have any field types." );
            Assert.That( parsedFile.GetEntries().Count(), Is.EqualTo( 1 ), "Should have one entry." );
            Assert.That( parsedFile.GetEntries().First().Name, Is.EqualTo( "Dog" ), "Did not parse entry name." );
            Assert.That( parsedFile.GetEntries().First().Description, Is.EqualTo( "A canine." ), "Did not parse entry description." );
        }

        private static IEnumerable<string> GetDescriptionWithoutFields()
        {
            yield return MakeCsvRow( "Name", "Description" );
            yield return MakeCsvRow( "Animal", "A collection of animals." );
            yield return MakeCsvRow( "Dog", "A canine." );
        }

        private static string MakeCsvRow( params string[] columns )
        {
            return String.Join( ",", columns.Select( _ => String.Format( "\"{0}\"", _.Replace( "\"", "\"\"" ) ) ) );
        }
    }
}
