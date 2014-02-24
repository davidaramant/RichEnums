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

            Assert.That( parsedFile.Description, Is.EqualTo( "A collection of animals." ),
                "Did not parse description." );
            Assert.That( parsedFile.EntryFields.Any(), Is.False, "Should not have any field types." );
            Assert.That( parsedFile.GetEntries().Count(), Is.EqualTo( 1 ), "Should have one entry." );
            Assert.That( parsedFile.GetEntries().First().Name, Is.EqualTo( "Dog" ), "Did not parse entry name." );
            Assert.That( parsedFile.GetEntries().First().Description, Is.EqualTo( "A canine." ), "Did not parse entry description." );
        }

        [Test]
        public void ShouldThrowWhenNameRowIsIncorrectLength()
        {
            Assert.Throws<RichEnumDescription.CsvParseException>(
                () => RichEnumDescription.ParseFile( new[] { MakeCsvRow( "Name" ) } ),
                "Did not handle name row being too short." );
        }

        [Test]
        public void ShouldThrowWhenNameRowDoesNotStartWithCorrectColumns()
        {
            Assert.Throws<RichEnumDescription.CsvParseException>(
                () => RichEnumDescription.ParseFile( new[] { MakeCsvRow( "NotName", "Description" ) } ),
                "Did not handle bad first column name for name row." );

            Assert.Throws<RichEnumDescription.CsvParseException>(
                () => RichEnumDescription.ParseFile( new[] { MakeCsvRow( "Name", "NotDescription" ) } ),
                "Did not handle bad second column name for name row." );
        }

        [Test]
        public void ShouldThrowWhenMissingDescriptionRow()
        {
            Assert.Throws<RichEnumDescription.CsvParseException>(
                () => RichEnumDescription.ParseFile( new[] { 
                        MakeCsvRow( "Name", "Description" ),
                        MakeCsvRow( "Dog", "Hello"),
                    } ),
                "Did not handle description row being missing." );
        }

        #region Test Data Generators

        private static IEnumerable<string> GetDescriptionWithoutFields()
        {
            yield return MakeCsvRow( "Name", "Description" );
            yield return MakeCsvRow( "Descriptions", "A collection of animals." );
            yield return MakeCsvRow( "Dog", "A canine." );
        }

        private static string MakeCsvRow( params string[] columns )
        {
            return String.Join( ",", columns.Select( _ => String.Format( "\"{0}\"", _.Replace( "\"", "\"\"" ) ) ) );
        }

        #endregion
    }
}
